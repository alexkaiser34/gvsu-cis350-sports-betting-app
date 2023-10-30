using Amazon.DynamoDBv2.DataModel;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameScoreController : ControllerBase
    {
        private readonly ILogger<GameScoreController> _logger;
        private readonly DbService<GameScore> _dbService;

        public GameScoreController(
            IDynamoDBContext dynamoDBContext,
            ILogger<GameScoreController> logger)
        {
            _dbService = new DbService<GameScore>(dynamoDBContext);
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<GameScore>> GetAll()
        {
            return await _dbService.GetAll();
        }

        [HttpGet]
        [Route("date", Name = "GetGamesByDate")]
        public async Task<IEnumerable<GameScore>> GetByDate(string begin, string? end = null)
        {
            return await _dbService.GetByDate(begin, end);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IEnumerable<GameScore>> GetByID(string id = "123")
        {
            return await _dbService.GetByID(id);
        }
    }
}