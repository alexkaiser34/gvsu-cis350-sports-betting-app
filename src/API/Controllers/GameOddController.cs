using Amazon.DynamoDBv2.DataModel;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameOddController : ControllerBase
    {

        private IDynamoDBContext _dbContext;
        public GameOddController(IDynamoDBContext dynamoDBContext) 
        {
            _dbContext = dynamoDBContext;
        }

        [HttpGet]
        public async Task<IEnumerable<GameOdd>> GetAll()
        {
            IEnumerable<ScanCondition> conditions = Enumerable.Empty<ScanCondition>();
            var scan = await _dbContext.ScanAsync<GameOdd>(conditions).GetRemainingAsync();
            return scan;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IEnumerable<GameOdd>> GetByID(string id = "123")
        {
            return await _dbContext
                .QueryAsync<GameOdd>(id)
                .GetRemainingAsync();
        }
    }
}
