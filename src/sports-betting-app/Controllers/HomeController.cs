using Microsoft.AspNetCore.Mvc;
using sports_betting_app.Models;
using System.Diagnostics;
using RestSharp;
using System.Web;
using Microsoft.AspNetCore.Http.Extensions;
using sports_betting_app.Data;

namespace sports_betting_app.Controllers
{
    public class HomeController : Controller
    {

        private readonly IAPIClientService<Wager> _api;
        public HomeController(IAPIClientService<Wager> api)
        {
            _api = api;
        }

        public IActionResult Index()
        {
            string data = "this is some test data";
            return View((object)data);
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