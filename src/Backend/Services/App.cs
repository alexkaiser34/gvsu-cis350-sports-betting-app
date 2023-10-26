using System;

namespace Backend.Services
{
    internal class App
    {
        private readonly DbHelper _dbHelper;
        private readonly ApiHelper _apiHelper;

        public App()
        {
            var jsonHelper = new JsonHelper();
            _dbHelper = new DbHelper(jsonHelper.getAwsKeys());
            _apiHelper = new ApiHelper(jsonHelper.getApiKey());
        }

        public async Task updateScores()
        {
            try
            {
                var scores = await _apiHelper.getRecentScores();
                await _dbHelper.Post(scores);
            }
            catch
            {
                Console.WriteLine("Error updating game scores");

            }

        }

        public async Task updateOdds()
        {
            try
            {
                var odds = await _apiHelper.getUpcomingOdds();
                await _dbHelper.Post(odds);

            }
            catch {
                Console.WriteLine("Error updating game odds");
            }
        }
    }
}
