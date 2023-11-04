using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WagerController : ControllerBase
    {
        private readonly ILogger<WagerController> _logger;
        private readonly DbService<Wager> _dbService;

        public WagerController(
            IDynamoDBContext dynamoDBContext,
            ILogger<WagerController> logger)
        {
            _dbService = new DbService<Wager>(dynamoDBContext);
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<Wager>> GetAll()
        {
            return await _dbService.GetAll();
        }


        [HttpGet]
        [Route("{id}")]
        public async Task<IEnumerable<Wager>> GetByID(string id = "123")
        {
            return await _dbService.GetByID(id);
        }

        [HttpGet]
        [Route("user/{user_id}")]
        public async Task<IEnumerable<Wager>> GetByUserID(string user_id = "123")
        {
            List<ScanCondition> conditions = new List<ScanCondition>();
            conditions.Add(new ScanCondition("user_id", ScanOperator.Equal, user_id));
            return await _dbService.dbContext.ScanAsync<Wager>(conditions).GetRemainingAsync();
        }


        [HttpGet]
        [Route("user/{user_id}/date")]
        public async Task<IEnumerable<Wager>> GetByDateAndUser(string user_id, string begin, string? end = null)
        {
            List<ScanCondition> tmp = _dbService.getDateConditions("date", begin, end);
            tmp.Add(new ScanCondition("user_id", ScanOperator.Equal, user_id));
            DynamoDBOperationConfig config = new DynamoDBOperationConfig()
            {
                ConditionalOperator = ConditionalOperatorValues.And
            };

            return await _dbService.dbContext.ScanAsync<Wager>(tmp, config).GetRemainingAsync();

        }

        [HttpGet]
        [Route("game/{game_id}")]
        public async Task<IEnumerable<Wager>> GetByGameID(string game_id = "123")
        {
            List<ScanCondition> conditions = new List<ScanCondition>();
            conditions.Add(new ScanCondition("game_id", ScanOperator.Equal, game_id));
            return await _dbService.dbContext.ScanAsync<Wager>(conditions).GetRemainingAsync();
        }

        private double _decimal_to_american(double dec)
        {
            if (dec >= 2.0)
            {
                return Math.Round(((dec - 1.0) * 100.0), 2);
            }

            return Math.Round((-100.0) / (dec - 1.0));
        }

        private double _american_to_decimal(double american)
        {
            if (american > 0.0)
            {
                return Math.Round((american / 100.0) + 1);
            }

            return Math.Round((100.0 / Math.Abs(american)) + 1);
        }

        private float _get_decimal_odds(BetData[] wagerData)
        {
            double multiplier = 1.0;
            foreach(var data in wagerData)
            {
                multiplier *= Math.Round(data.price, 2);
            }

            return (float)Math.Round(multiplier, 2);
        }

        [HttpPost]
        [Route("Post")]
        public async Task PostWager(WagerData inputWager)
        {
            if (inputWager == null)
            {
                return;
            }

            /** Auto increment ID **/
            IEnumerable<ScanCondition> conditions = Enumerable.Empty<ScanCondition>();
            var scan = await _dbService.dbContext.ScanAsync<Wager>(conditions).GetRemainingAsync();

            float decimalOdds = this._get_decimal_odds(inputWager.bet_data);
            float americanOdds = (float)this._decimal_to_american((double)decimalOdds);

            Wager newWager = new Wager()
            {
                user_id = inputWager.user_id,
                game_id = inputWager.game_id,
                bet_data = inputWager.bet_data,
                wager_amount = inputWager.wager_amount,
                amount_win = 0.0f,
                completed = false,
                american_odds = americanOdds,
                decimal_odds = decimalOdds
            };

            List<int> ids = new List<int>();
            for (int i = 0; i < scan.Count; i++)
            {
                ids.Add(Int32.Parse(scan[i].id));
            }

            for (int i = 0; i <= scan.Count + 1; i++)
            {
                if (!ids.Contains(i))
                {
                    /** assign ID **/
                    newWager.id = i.ToString();
                    newWager.date = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
                    await _dbService.dbContext.SaveAsync<Wager>(newWager);
                    return;
                }
            }

        }

        [HttpDelete]
        [Route("Delete")]
        public async Task DeleteById(string id = "123")
        {
            List<Wager> tmp = await _dbService.dbContext.QueryAsync<Wager>(id).GetRemainingAsync();

            if (tmp.Count > 0)
            {
                await _dbService.dbContext.DeleteAsync<Wager>(tmp[0]);
            }
        }

    }
}
