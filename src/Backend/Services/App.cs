﻿using Backend.Models;
using System;

namespace Backend.Services
{
    internal class App
    {
        private readonly DbHelper _dbHelper;
        private readonly ApiHelper _apiHelper;
        private readonly WagerHelper _wagerHelper;


        public App()
        {
            var jsonHelper = new JsonHelper();
            _dbHelper = new DbHelper(jsonHelper.getAwsKeys());
            _apiHelper = new ApiHelper(jsonHelper.getApiKey());
            _wagerHelper = new WagerHelper();
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
            catch 
            {
                Console.WriteLine("Error updating game odds");
            }
        }

        public async Task updateWagers()
        {
            try
            {
                List<GameScore> scores = await _dbHelper.GetAll<GameScore>();
                List<Wager> wagers = await _dbHelper.GetAll<Wager>();

                List<Wager> activeWagers = wagers.FindAll(x => x.completed == false);
                List<GameScore> completedGames = scores.FindAll(x => x.completed == true);

                IEnumerable<Wager> updatedWagers = _wagerHelper.updateActiveWagers(activeWagers, completedGames);
                await _dbHelper.Post(updatedWagers);

            }
            catch
            {
                Console.WriteLine("Error updating wagers");
            }


        }
    }
}
