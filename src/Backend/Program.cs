using Backend.Models;
using Backend.Services;

var apiHelper = new ApiHelper();

var odds = await apiHelper.getUpcomingOdds();

apiHelper.printItems<GameOdd>(odds);

//var scores = await apiHelper.getRecentScores();

//apiHelper.printItems<GameScore>(scores);



