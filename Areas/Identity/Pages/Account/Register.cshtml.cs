// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using MyLittleBusiness.Areas.Identity.Data;

namespace MyLittleBusiness.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<MyLittleBusinessUser> _signInManager;
        private readonly UserManager<MyLittleBusinessUser> _userManager;
        private readonly IUserStore<MyLittleBusinessUser> _userStore;
        private readonly IUserEmailStore<MyLittleBusinessUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<MyLittleBusinessUser> userManager,
            IUserStore<MyLittleBusinessUser> userStore,
            SignInManager<MyLittleBusinessUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {

            [Required(ErrorMessage = "Pole *Imię* jest wymagane.")]
            [DisplayName("Imię")]
            [MaxLength(50, ErrorMessage = "Pole *Imię* może zawierać maxymalnie 50 znaków")]
            public string FirstName { get; set; }

            [DisplayName("Drugie imię")]
            [MaxLength(50, ErrorMessage = "Pole *Drugie imię* może zawierać maxymalnie 50 znaków")]
            public string SecondName { get; set; }

            [Required(ErrorMessage = "Pole *Nazwisko* jest wymagane.")]
            [DisplayName("Nazwisko")]
            [MaxLength(50, ErrorMessage = "Pole *Nazwisko* może zawierać maxymalnie 50 znaków")]
            public string Surname { get; set; }

            [Required(ErrorMessage = "Pole *Nazwa firmy* jest wymagane.")]
            [DisplayName("Nazwa firmy")]
            [MaxLength(100, ErrorMessage = "Pole *Nazwa firmy może zawierać maxymalnie 100 znaków*")]
            public string ConcernName { get; set; }

            [Required(ErrorMessage = "Pole *Nr. NIP* jest wymagane.")]
            [DisplayName("Nr. NIP")]
            [MaxLength(10, ErrorMessage = "Pole *Nr. NIP* musi zawierać 10 cyfr")]
            [MinLength(10, ErrorMessage = "Pole *Nr. NIP* musi zawierać 10 cyfr")]
            public string Nip { get; set; }

            [Required(ErrorMessage = "Pole *Email* jest wymagane.")]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Pole *Hasło* jest wymagane.")]
            [StringLength(100, ErrorMessage = "Hasło musi zawierać co najmniej 6 znaków, maksymalnie 100.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }


            [Required(ErrorMessage = "Pole *Potwierdź hasło* jest wymagane.")]
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "Hasła są niezgodne.")]
            public string ConfirmPassword { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("/Charts/SalesChart");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();
                user.FirstName = Input.FirstName;
                user.SecondName = Input.SecondName;
                user.Surname = Input.Surname;
                user.ConcernName = Input.ConcernName;
                user.Nip = Input.Nip;

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Utworzona nowe konto chronione hasłem.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Proszę potwierdzić założenie konta na <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Kliknij tutaj</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return Page();
        }

        private MyLittleBusinessUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<MyLittleBusinessUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Nie można stworzyć '{nameof(MyLittleBusinessUser)}'. " +
                    $"Upewnij się, że '{nameof(MyLittleBusinessUser)}' nie jest klasą abstrakcyjną, konstruktorem bezparametrowym");
            }
        }

        private IUserEmailStore<MyLittleBusinessUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("Domyślne UI");
            }
            return (IUserEmailStore<MyLittleBusinessUser>)_userStore;
        }
    }
}
