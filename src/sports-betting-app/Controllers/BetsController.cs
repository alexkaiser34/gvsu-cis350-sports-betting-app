using Microsoft.AspNetCore.Mvc;

namespace sports_betting_app.Controllers
{
    public class BetsController : Controller
    {
        public IActionResult Index()
        {
            string data = "Bets page";
            return View((object)data);
        }
    }
}
