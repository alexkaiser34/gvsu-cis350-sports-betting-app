using Microsoft.AspNetCore.Mvc;
using sports_betting_app.Data;
using sports_betting_app.Models;

namespace sports_betting_app.Controllers
{
    public class UserRegistrationController : Controller
    {

        private readonly IAPIClientService<User> _api;

        public UserRegistrationController(IAPIClientService<User> api)
        {
            _api = api;
        }
    
        public IActionResult Index()
        {
            string data = "User Registration page";
            return View((object)data);
        }
    }
}
