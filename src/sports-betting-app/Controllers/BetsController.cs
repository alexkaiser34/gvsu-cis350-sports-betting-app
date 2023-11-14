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
            string endpoint = "GameOdd/date";

            ApiParam current = new ApiParam()
            {
                key = "begin",
                value = DateTime.Now.ToString("yyyy-MM-dd")
            };

            ApiParam nextWeek = new ApiParam()
            {
                key = "end",
                value = DateTime.Now.AddDays(7).ToString("yyyy-MM-dd")
            };


            List<ApiParam> dateParams = new List<ApiParam>();

            dateParams.Add(current);
            dateParams.Add(nextWeek);

            IEnumerable<GameOdd> results = await _api.GetAll(endpoint, dateParams);
            return View(results);
        }
    }
}
