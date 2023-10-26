using Backend.Models;
using Backend.Services;

var jsonHelper = new JsonHelper();
var dbHelper = new DbHelper(jsonHelper.getAwsKeys());
var apiHelper = new ApiHelper(jsonHelper.getApiKey());

var scores = await apiHelper.getRecentScores();
await dbHelper.Post(scores);
var odds = await apiHelper.getUpcomingOdds();
await dbHelper.Post(odds);



