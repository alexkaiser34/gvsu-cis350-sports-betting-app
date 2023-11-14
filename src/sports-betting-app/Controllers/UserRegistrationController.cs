using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using sports_betting_app.Data;
using sports_betting_app.Models;

namespace sports_betting_app.Controllers
{
    public class UserRegistrationController : Controller
    {
        private readonly IAPIClientService<UserData> _api;

        public UserRegistrationController(IAPIClientService<UserData> api)
        {
            _api = api;
        }

        public IActionResult Index()
        {
            UserData tmp = new UserData();
            return View(tmp);
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserData user)
        {
            try
            {
                // post the user to the api
                await _api.Add(user, "User/Post");

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
