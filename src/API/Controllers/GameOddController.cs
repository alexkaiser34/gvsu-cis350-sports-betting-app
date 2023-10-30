using Amazon.DynamoDBv2.DataModel;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameOddController : ControllerBase
    {

        private readonly ILogger<GameOddController> _logger;
        private readonly DbService<GameOdd> _dbService;


        public GameOddController(
            IDynamoDBContext dynamoDBContext,
            ILogger<GameOddController> logger)
        {
            _dbService = new DbService<GameOdd>(dynamoDBContext);
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<GameOdd>> GetAll()
        {

            return await _dbService.GetAll();
        }

        [HttpGet]
        [Route("date", Name = "GetOddsByDate")]
        public async Task<IEnumerable<GameOdd>> GetByDate(string begin, string? end = null)
        {
            return await _dbService.GetByDate(begin, end);
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<IEnumerable<GameOdd>> GetByID(string id = "123")
        {
            return await _dbService.GetByID(id);
        }
    }
}
