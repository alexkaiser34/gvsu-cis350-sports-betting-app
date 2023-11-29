using Microsoft.Extensions.Logging;
using sports_betting_app.Controllers;
using Moq;
using Microsoft.AspNetCore.Mvc;
using sports_betting_app.Data;
using sports_betting_app.Models;

namespace WebAppUnitTests
{
    public class HomeControllerTest
    {
        HomeController controller;
        private readonly IAPIClientService<Wager> _api;

        public HomeControllerTest()
        {
            controller = new HomeController(_api);
        }
    }
}