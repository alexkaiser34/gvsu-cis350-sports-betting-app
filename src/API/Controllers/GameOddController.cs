using Amazon.Auth.AccessControlPolicy;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Util;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Xml.Linq;

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

            List<ScanCondition> conditions = new List<ScanCondition>();
            var scan = await _dbContext.ScanAsync<GameOdd>(conditions).GetRemainingAsync();
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
        [Route("date", Name = "GetOddsByDate")]
        public async Task<IEnumerable<GameOdd>> GetByDate(string begin, string? end = null)
        {
            IEnumerable<ScanCondition> conditions = _getConditions(begin, end);
            DynamoDBOperationConfig config = new DynamoDBOperationConfig()
            {
                ConditionalOperator = ConditionalOperatorValues.And
            };
            var scan = await _dbContext.ScanAsync<GameOdd>(conditions, config).GetRemainingAsync();
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
