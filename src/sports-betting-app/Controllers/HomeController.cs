using Microsoft.AspNetCore.Mvc;
using sports_betting_app.Models;
using System.Diagnostics;
using RestSharp;
using System.Web;
using Microsoft.AspNetCore.Http.Extensions;
using sports_betting_app.Data;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using System;

namespace sports_betting_app.Controllers
{
    public class HomeController : Controller
    {

        private readonly IAPIClientService<Wager> _api;
        public HomeController(IAPIClientService<Wager> api)
        {
            _api = api;
        }

        public async Task<IActionResult> Index()
        {
            if (Request.Cookies["sports-bet-user"] == null)
            {
                return View(new List<Wager>());
            }
            User loggedInUser = JsonConvert.DeserializeObject<User>(Request.Cookies["sports-bet-user"]);

            if (loggedInUser != null) 
            {
                string endpoint = "Wager/user/" + loggedInUser.id;
                List<Wager> results = await _api.GetAll(endpoint);
                List<Wager> activeWagers = results.FindAll(m => m.completed == false);
                return View(activeWagers);

            }

            return View(new List<Wager>());
            

            
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}