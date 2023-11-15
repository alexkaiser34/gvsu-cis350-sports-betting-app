using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using sports_betting_app.Data;
using sports_betting_app.Models;

namespace sports_betting_app.Controllers
{
    public class BetsController : Controller
    {


        private readonly IAPIClientService<GameOdd> _api;
        private readonly IHttpContextAccessor _contextAccessor;

        public BetsController(IAPIClientService<GameOdd> api, IHttpContextAccessor contextAccessor)
        {
            _api = api;
            _contextAccessor = contextAccessor;
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

            

            // store a session variable to hold a wager list
            string tmp = _contextAccessor.HttpContext.Session.GetString("wagerList");

            if (tmp == null) {
                WagerData wager = new WagerData();
                wager.bet_data = new List<BetData>();
                _contextAccessor.HttpContext.Session.SetString("wagerList", JsonConvert.SerializeObject(wager));
            }

            return View(results);
        }

        [HttpPost]
        public void updateWagerBet(BetData bet)
        {
            // retrieve wager list as string
            var tmp = JsonConvert.DeserializeObject<WagerData>(
                _contextAccessor.HttpContext.Session.GetString("wagerList")
            );

            // if successful, add bet clicked
            if (tmp != null)
            {
                tmp.bet_data.Add(bet);
            }

            // save the variable as a string
            _contextAccessor.HttpContext.Session.SetString("wagerList", JsonConvert.SerializeObject(tmp));
        }
    }
}
