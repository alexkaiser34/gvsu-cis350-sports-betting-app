using Microsoft.AspNetCore.Mvc;
using sports_betting_app.Models;
using sports_betting_app.Data;
using Newtonsoft.Json;

namespace sports_betting_app.Controllers
{
    public class WagerController : Controller
    {
        private readonly IAPIClientService<WagerData> _api;

        private readonly IHttpContextAccessor _contextAccessor;

        public WagerController(IAPIClientService<WagerData> api, IHttpContextAccessor contextAccessor)
        {
            _api = api;
            _contextAccessor = contextAccessor;
        }

        [HttpPost]
        public async Task<IActionResult> postWager(WagerData wager, float amountBet)
        {
            wager.wager_amount = (float)Math.Round((double)amountBet, 2);

            // hard code for right now
            wager.user_id = "1";

            // add wager to database
            await _api.Add(wager, "Wager/Post");

            // reset wager list
            WagerData tmp = new WagerData();
            tmp.bet_data = new List<BetData>();
            _contextAccessor.HttpContext.Session.SetString("wagerList", JsonConvert.SerializeObject(tmp));

            return StatusCode(200);
        }

    }
}
