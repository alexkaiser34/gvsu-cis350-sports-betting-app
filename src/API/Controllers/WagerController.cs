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
        [Route("game/{game_id}")]
        public async Task<IEnumerable<Wager>> GetByGameID(string game_id = "123")
        {
            List<ScanCondition> conditions = new List<ScanCondition>();
            conditions.Add(new ScanCondition("game_id", ScanOperator.Equal, game_id));
            return await _dbService.dbContext.ScanAsync<Wager>(conditions).GetRemainingAsync();
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
            Wager newWager = new Wager()
            {
                user_id = inputWager.user_id,
                game_id = inputWager.game_id,
                bet_type = inputWager.bet_type,
                wager_amount = inputWager.wager_amount,
                amount_win = inputWager.amount_win,
                didWagerWin = inputWager.didWagerWin
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
                    await _dbService.dbContext.SaveAsync<Wager>(newWager);
                    return;
                }
            }

        }

        [HttpDelete]
        [Route("Delete")]
        public async Task Delete(Wager inputWager)
        {
            await _dbService.dbContext.DeleteAsync<Wager>(inputWager);
        }

    }
}
