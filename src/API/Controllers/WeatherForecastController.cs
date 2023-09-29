using Amazon.DynamoDBv2.DataModel;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private IDynamoDBContext _dynamoDbContext;
        public WeatherForecastController(
            IDynamoDBContext dynamoDBContext,
            ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            _dynamoDbContext = dynamoDBContext;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get(string city = "Grand Rapids")
        {
            return await _dynamoDbContext
                .QueryAsync<WeatherForecast>(city)
                .GetRemainingAsync();
        }

        [HttpPost(Name = "PostWeatherForecast")]
        public async Task Post(string city = "GrandRapids")
        {
            var data = GenererateDummyData(city);
            foreach (var d in data)
            {
                await _dynamoDbContext.SaveAsync(d);
            }
        }

        private static WeatherForecast[] GenererateDummyData(string city)
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                City = city,
                Date = DateTime.UtcNow.Date.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpDelete(Name = "DeleteWeatherForecast")]
        public async Task Delete(string city)
        {

            await _dynamoDbContext
                .DeleteAsync<WeatherForecast>(city, DateTime.Now.Date);
        }
    }
}