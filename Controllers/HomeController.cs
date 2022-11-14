using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyLittleBusiness.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public IActionResult Index(bool IsSignedIn)
        {
            if (IsSignedIn)
            {

            }
            else
            {

            }
            return View();
        }
    }
}
