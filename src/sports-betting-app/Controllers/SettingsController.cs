using Microsoft.AspNetCore.Mvc;
using sports_betting_app.Data;
using sports_betting_app.Models;

namespace sports_betting_app.Controllers
{
    public class SettingsController : Controller
    {

        private readonly IAPIClientService<User> _api;

        public SettingsController(IAPIClientService<User> api)
        {
            _api = api;
        }
    
        public IActionResult Index()
        {
            string data = "Settings page";
            return View((object)data);
        }
    }
}
