using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

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

        private IEnumerable<ScanCondition> _getConditions(string beginDate, string endDate = null)
        {

            string end = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
            string begin = DateTime.Parse(beginDate).ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
            List<ScanCondition> conditions = new List<ScanCondition>();

            if (endDate != null)
            {
                /** return a range of dates **/

                /** Add time to end day to include all games within that day **/
                DateTime tmp = DateTime.Parse(endDate).AddHours(23).AddMinutes(59);
                end = tmp.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture);
                conditions.Add(new ScanCondition("commence_time", ScanOperator.Between, begin, end));
            }
            else
            {
                /** return a single day **/
                conditions.Add(new ScanCondition("commence_time", ScanOperator.BeginsWith, beginDate));
            }

            return conditions;


        }

        [HttpGet]
        [Route("date", Name = "GetGamesByDate")]
        public async Task<IEnumerable<GameScore>> GetByDate(string begin, string? end = null)
        {
            IEnumerable<ScanCondition> conditions = _getConditions(begin, end);
            DynamoDBOperationConfig config = new DynamoDBOperationConfig()
            {
                ConditionalOperator = ConditionalOperatorValues.And
            };
            var scan = await _dbContext.ScanAsync<GameScore>(conditions, config).GetRemainingAsync();
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