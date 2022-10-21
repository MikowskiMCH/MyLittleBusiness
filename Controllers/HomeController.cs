using Microsoft.AspNetCore.Mvc;

namespace MyLittleBusiness.Controllers
{
    public class HomeController : Controller
    {
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
