using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using sports_betting_app.Data;
using sports_betting_app.Models;
using static System.Collections.Specialized.BitVector32;

namespace sports_betting_app.Controllers
{
    public class UserRegistrationController : Controller
    {
        private readonly IAPIClientService<UserData> _api;

        public UserRegistrationController(IAPIClientService<Userdata> api)
        {
            _api = api;
        }

        public IActionResult Index()
        {
            string data = "Welcome to Sports Bets";
            return View((object)data);
        }

        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            try
            {
                // Call the API to check user data
                bool userDataCorrect = await _api.CheckUserData(user);

                if (userDataCorrect)
                {
                    // Data correct, proceed with registration
                    // Call another method to handle the registration process
                    bool registrationSuccess = await _api.RegisterUser(user);

                    if (registrationSuccess)
                    {
                        // Registration successful, redirect to a success page or do something else
                        return RedirectToAction("RegistrationSuccess");
                    }

                    // Handle failed registration, maybe return to the same page with an error message
                    ModelState.AddModelError(string.Empty, "Registration failed. Please try again.");
                    return View("RegistrationFailedView"); // Provide a view for failed registration
                }
                else
                {
                    // Show an error message indicating incorrect user data
                    ModelState.AddModelError(string.Empty, "Incorrect user data. Please verify your information.");
                    return View(); // Return to the same page with an error message
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                return StatusCode(500); // Internal server error
            }
        }
    }
}
