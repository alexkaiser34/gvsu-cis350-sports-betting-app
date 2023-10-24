using Backend.Models;
using Newtonsoft.Json;

namespace Backend.Services
{
    internal class ApiHelper
    {
        private readonly OddsAPI _api;

        public ApiHelper() {
            _api = new OddsAPI(new JsonHelper().getApiKey());
        }

        public async Task<IEnumerable<GameScore>> getRecentScores()
        {
            var scores = await _api.makeRequest<GameScore>("/americanfootball_nfl/scores", new List<QueryParam>
            {
                new QueryParam {key="daysFrom", value="3"}
            });

            return scores;
        }

        public async Task<IEnumerable<GameOdd>> getUpcomingOdds()
        {
            var odds = await _api.makeRequest<GameOdd>("/americanfootball_nfl/odds", new List<QueryParam>
            {
                new QueryParam {key="regions", value="us"},
                new QueryParam {key="markets", value="h2h,spreads,totals"},
                new QueryParam {key="bookmakers", value="fanduel,draftkings,betmgm"}
            });

            return odds;
        }

        public void printItems<T>(IEnumerable<T> data)
        {
            foreach (T i in data)
            {
                Console.WriteLine(JsonConvert.SerializeObject(i, Formatting.Indented));
            }
        }
    }
}
