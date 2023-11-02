using Microsoft.AspNetCore.Mvc;

namespace sports_betting_app.Controllers
{
    public class HistoryController : Controller
    {
        public IActionResult Index()
        {
            string data = "History page";
            return View((object)data);
        }
    }
}
