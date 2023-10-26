using Backend.Models;
using Backend.Services;

var jsonHelper = new JsonHelper();

var apiHelper = new ApiHelper(jsonHelper.getApiKey());

var odds = await apiHelper.getUpcomingOdds();

apiHelper.printItems<GameOdd>(odds);

//var scores = await apiHelper.getRecentScores();

//apiHelper.printItems<GameScore>(scores);



