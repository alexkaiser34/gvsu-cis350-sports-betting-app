using Amazon.DynamoDBv2.DataModel;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameBetsController : ControllerBase
    {

        private IDynamoDBContext _dbContext;
        public GameBetsController(IDynamoDBContext dynamoDBContext) 
        {
            _dbContext = dynamoDBContext;
        }

        [HttpGet(Name = "GetGameBets")]
        public async Task<IEnumerable<GameBets>> Get(string GameID = "123")
        {
            return await _dbContext
                .QueryAsync<GameBets>(GameID)
                .GetRemainingAsync();
        }

        [HttpPost(Name = "PostGameBets")]
        public async Task Post(string GameID = "123")
        {
            var data = GenerateDummyData(GameID);
            foreach (var item in data)
            {
                await _dbContext.SaveAsync(item);
            }
        }

        [HttpDelete(Name = "DeleteGameBets")]
        public async Task Delete(string gameID, string betType = "*")
        {
            await _dbContext
                .DeleteAsync<GameBets>(gameID, betType);
        }

        private static GameBets[] GenerateDummyData(string gameID)
        {
            return Enumerable.Range(1,5).Select(index => {
                var betType = "h2h";
                switch (index)
                {
                    case 1:
                        betType = "h2h";
                        break;
                    case 2:
                        betType = "o/u";
                        break;
                    case 3:
                        betType = "spread";
                        break;
                    case 4:
                        betType = "ATTD";
                        break;
                    default:
                        betType = "FTTD";
                        break;
                }
                return new GameBets
                {
                    GameID = gameID,
                    BetType = betType
                };
            }).ToArray();
        }
    }
}
