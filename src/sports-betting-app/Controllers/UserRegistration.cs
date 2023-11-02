using Microsoft.AspNetCore.Mvc;

namespace sports_betting_app.Controllers
{
    public class UserRegistration : Controller
    {
        public IActionResult Index()
        {
            string data = "User Registration page";
            return View((object)data);
        }
    }
}
