using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using sports_betting_app.Data;
using sports_betting_app.Models;
using Newtonsoft.Json;

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
            UserData tmp = new UserData();
            return View(tmp);
        }

        public IActionResult LogOut()
        {
            Response.Cookies.Delete("sports-bet-user");
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserData user)
        {
            try
            {
                // post the user to the api
                await _api.AddByType<UserData>(user, "User/Post");

                // get the user added
                var userList = await _api.GetAll("User");

                var userAdded = userList.Find(m =>
                    m.firstName.Equals(user.firstName, StringComparison.OrdinalIgnoreCase) &&
                    m.lastName.Equals(user.lastName, StringComparison.OrdinalIgnoreCase));

                // create and store the cookie when they log in
                var cookie = new CookieOptions();
                cookie.Expires = DateTime.Now.AddDays(7);
                cookie.Path = "/";
                Response.Cookies.Append("sports-bet-user", JsonConvert.SerializeObject(userAdded), cookie);


                // redirect to previous page
                return Redirect(Request.Headers["Referer"].ToString());

            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500); // Internal server error
            }
        }
    }
}
