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

        public IActionResult Index(UserData? userIn = null)
        {
            if (userIn == null)
            {
                UserData tmp = new UserData();
                return View(tmp);
            }
            else
            {
                return View(userIn);
            }
        }

        public IActionResult LogOut()
        {
            Response.Cookies.Delete("sports-bet-user");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult LogIn(UserData? userIn = null)
        {
            if (userIn == null)
            {
                UserData tmp = new UserData();
                return View(tmp);
            }
            else
            {
                return View(userIn);
            }
        }

        [HttpPost]
        public async Task<IActionResult> logInExisting(UserData user)
        {
            try
            {
                // get the user added
                var userList = await _api.GetAll("User");

                var userFound = userList.FirstOrDefault(m =>
                    m.UserName.Equals(user.UserName) &&
                    m.Password.Equals(user.Password));

                if (userFound == null)
                {
                    TempData["ErrorMessage"] = "Invalid username or password";
                    return RedirectToAction("LogIn", "UserRegistration", user);

                }

                // create and store the cookie when they log in
                var cookie = new CookieOptions();
                cookie.Expires = DateTime.Now.AddDays(7);
                cookie.Path = "/";
                Response.Cookies.Append("sports-bet-user", JsonConvert.SerializeObject(userFound), cookie);


                // redirect to home page
                return RedirectToAction("Index", "Home");


            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500); // Internal server error
            }
        }



        [HttpPost]
        public async Task<IActionResult> Register(UserData user)
        {
            try
            {
                var userValidate = await _api.GetAll("User");

                var userExists = userValidate.Find(m => m.UserName.Equals(user.UserName));

                if (userExists != null)
                {
                    TempData["ErrorMessage"] = "User name already exists!";
                    return RedirectToAction("Index", "UserRegistration", user);
                }
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


                // redirect to home page
                return RedirectToAction("Index", "Home");


            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500); // Internal server error
            }
        }
    }
}
