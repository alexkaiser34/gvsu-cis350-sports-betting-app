using Backend.Models;
using Backend.Services;
using Newtonsoft.Json;

var api = new OddsAPI(new JsonHelper().getApiKey());
var tmp = await api.makeRequest<GameBet>("/americanfootball_nfl/odds");

foreach (GameBet i in tmp)
{
    Console.WriteLine(JsonConvert.SerializeObject(i, Formatting.Indented));
}

