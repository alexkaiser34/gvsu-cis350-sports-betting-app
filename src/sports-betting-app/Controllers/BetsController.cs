using Microsoft.AspNetCore.Mvc;
using sports_betting_app.Data;
using sports_betting_app.Models;

namespace sports_betting_app.Controllers
{
    public class BetsController : Controller
    {
        private readonly IAPIClientService<GameOdd> _api;

        public BetsController(IAPIClientService<GameOdd> api)
        {
            _api = api;
        }

        public async Task<IActionResult> Index()
        {
            /** make the API request and pass data to view **/
            string endpoint = "GameOdd";
            IEnumerable<GameOdd> results = await _api.GetAll(endpoint);
            return View(results);
        }
    }
}
