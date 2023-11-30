// Controllers/HistoryController.cs
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using sports_betting_app.Data;
using sports_betting_app.Models;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace sports_betting_app.Controllers
{
    public class HistoryController : Controller
    {
        private readonly IAPIClientService<Wager> _api;

        public HistoryController(IAPIClientService<Wager> api)
        {
            _api = api;
        }

        public async Task<IActionResult> Index()
        {
            if (Request.Cookies["sports-bet-user"] == null)
            {
                return View(new List<Wager>());
            }
            // Retrieve history data from your API or database
            User loggedInUser = JsonConvert.DeserializeObject<User>(Request.Cookies["sports-bet-user"]);

            if (loggedInUser != null)
            {
                string endpoint = "Wager/user/" + loggedInUser.id;
                List<Wager> results = await _api.GetAll(endpoint);

                foreach (Wager wager in results)
                {
                    if (wager.completed)
                    {
                        wager.amount_win -= wager.wager_amount;
                    }
                }
                results.Sort((x, y) => y.date.CompareTo(x.date));

                return View(results);

            }

            return View(new List<Wager>());
        }
    }
}
