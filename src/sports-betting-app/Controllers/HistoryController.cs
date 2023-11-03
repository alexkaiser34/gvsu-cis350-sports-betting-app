using Microsoft.AspNetCore.Mvc;
using sports_betting_app.Data;
using sports_betting_app.Models;

namespace sports_betting_app.Controllers
{
    public class HistoryController : Controller
    {

        private readonly IAPIClientService<Wager> _api;

        public HistoryController(IAPIClientService<Wager> api)
        {
            _api = api;
        }


        public IActionResult Index()
        {
            string data = "History page";
            return View((object)data);
        }
    }
}
