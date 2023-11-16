﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using sports_betting_app.Data;
using sports_betting_app.Models;
using System.Net;

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
                IDictionary<string, string> wagerGames = new Dictionary<string, string>();
                _contextAccessor.HttpContext.Session.SetString("wagerList", JsonConvert.SerializeObject(wager));
                _contextAccessor.HttpContext.Session.SetString("wagerGames", JsonConvert.SerializeObject(wagerGames));
            }

            return View(results);
        }

        [HttpPost]
        public async Task<IActionResult> updateWagerBet(BetData bet)
        {
            // retrieve wager list as string
            var tmp = JsonConvert.DeserializeObject<WagerData>(
                _contextAccessor.HttpContext.Session.GetString("wagerList")
            );


            // if successful, add bet clicked
            if (tmp != null)
            {
                BetData exists = tmp.bet_data.FirstOrDefault(x => x.bet_type == bet.bet_type && x.name == bet.name && x.price == bet.price);
                if (exists != null)
                {
                    return StatusCode(400, Json("Bet is already in wager"));
                }
                tmp.bet_data.Add(bet);
            }

            // save the variable as a string
            _contextAccessor.HttpContext.Session.SetString("wagerList", JsonConvert.SerializeObject(tmp));


            

            var tmp_games = JsonConvert.DeserializeObject<IDictionary<string,string>>(
                _contextAccessor.HttpContext.Session.GetString("wagerGames")
            );
                

            if (tmp_games != null)
            {
                if (!tmp_games.ContainsKey(bet.game_id))
                {
                    var wagerGames = await _api.GetAll("GameOdd/" + bet.game_id);

                    if (wagerGames != null)
                    {
                        string home_team = wagerGames[0].home_team.Split(' ').Last();
                        string away_team = wagerGames[0].away_team.Split(' ').Last();
                        tmp_games.Add(bet.game_id, home_team + " vs " + away_team);
                        _contextAccessor.HttpContext.Session.SetString("wagerGames", JsonConvert.SerializeObject(tmp_games));
                    }
                }
            }

            return StatusCode(200);

        }

        [HttpPost]
        public IActionResult deleteWagerBet(BetData bet)
        {
            // retrieve wager list as string
            var tmp = JsonConvert.DeserializeObject<WagerData>(
                _contextAccessor.HttpContext.Session.GetString("wagerList")
            );

            if (tmp != null)
            {
                var betRemove = tmp.bet_data.FirstOrDefault(u => u.game_id.Equals(bet.game_id) && u.bet_type.Equals(bet.bet_type) && u.name.Equals(bet.name));

                if (betRemove != null ) {
                    tmp.bet_data.Remove(betRemove);
                }
                else
                {
                    return StatusCode(400, Json("Bet is not in wager"));

                }
            }

            _contextAccessor.HttpContext.Session.SetString("wagerList", JsonConvert.SerializeObject(tmp));

            return StatusCode(200);
        }

        
    }
}
