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

        private readonly IAPIClientService<GameOdd> _api;
        public HomeController(IAPIClientService<GameOdd> api)
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

        public void ThemeChange()
        {
            /** Parse theme cookie **/
            if (Request.Cookies.ContainsKey("Theme"))
            {
                foreach (var cookie in Request.Cookies)
                {
                    if (cookie.Key == "Theme")
                    {
                        var color = cookie.Value == "Dark" ? "Light" : "Dark";

                        /** toggle cookie value **/
                        var cookieOps = new CookieOptions();
                        cookieOps.Expires = DateTime.Now.AddDays(1);
                        Response.Cookies.Append(cookie.Key, color);
                    }
                }
            }
            else
            {
                /** Default color is light, so make dark on first round **/
                Response.Cookies.Append("Theme", "Dark");
            }

            /** redirect to main page
             * NOTE: we will want to redirect to whichever page they are currently on 
             */
            Response.Redirect("/", false);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}