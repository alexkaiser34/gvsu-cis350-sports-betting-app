using Amazon.DynamoDBv2.DataModel;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameScoreController : ControllerBase
    {
        private readonly ILogger<GameScoreController> _logger;
        private IDynamoDBContext _dbContext;

        public GameScoreController(
            IDynamoDBContext dynamoDBContext,
            ILogger<GameScoreController> logger)
        {
            _logger = logger;
            _dbContext = dynamoDBContext;
        }

        [HttpGet]
        public async Task<IEnumerable<GameScore>> GetAll()
        {
            IEnumerable<ScanCondition> conditions = Enumerable.Empty<ScanCondition>();
            var scan = await _dbContext.ScanAsync<GameScore>(conditions).GetRemainingAsync();
            return scan;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IEnumerable<GameScore>> GetByID(string id = "123")
        {
            return await _dbContext
                .QueryAsync<GameScore>(id)
                .GetRemainingAsync();
        }
    }
}