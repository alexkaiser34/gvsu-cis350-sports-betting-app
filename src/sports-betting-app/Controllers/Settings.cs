using Microsoft.AspNetCore.Mvc;

namespace sports_betting_app.Controllers
{
    public class Settings : Controller
    {
        public IActionResult Index()
        {
            string data = "Settings page";
            return View((object)data);
        }
    }
}
