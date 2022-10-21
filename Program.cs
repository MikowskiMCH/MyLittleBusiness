using Microsoft.EntityFrameworkCore;
using MyLittleBusiness.Models;
using Microsoft.AspNetCore.Identity;
using MyLittleBusiness.Areas.Identity.Data;
using System.Globalization;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Localization;
using MyLittleBusiness.Controllers;
using Rotativa.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddDbContext<MyLittleBusiness.Models.MyLittleBusinessContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("MyLittleDatabase")));
builder.Services.AddDefaultIdentity<MyLittleBusinessUser>(options =>
{
    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedAccount = false;
})
    .AddErrorDescriber<NewErrorDescriobers>()
    .AddEntityFrameworkStores<MyLittleBusinessContext>();
var app = builder.Build();

var supportedCultures = new[]
{
 new CultureInfo("en-US"),
};

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

IWebHostEnvironment env = app.Environment;
RotativaConfiguration.Setup((Microsoft.AspNetCore.Hosting.IHostingEnvironment)env);

app.MapRazorPages();
app.Run();
