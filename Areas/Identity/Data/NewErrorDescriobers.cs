using Microsoft.AspNetCore.Identity;

namespace MyLittleBusiness.Areas.Identity.Data
{
    public class NewErrorDescriobers : IdentityErrorDescriber
    {
        public override IdentityError DuplicateEmail(string email)
        {
            var error = base.DuplicateEmail(email);
            error.Description = "Ten adres email został już wcześniej zarejestrowany.";
            return error;
        }
        public override IdentityError DuplicateUserName(string userName)
        {
            var error = base.DuplicateUserName(userName);
            error.Description = "Istnieje już taki użytkownik w rejestrze.";
            return error;
        }
    }
}
