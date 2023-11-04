﻿using Backend.Models;

namespace Backend.Services
{
    internal class WagerHelper
    {
        public WagerHelper() { 
        
        }

        private float _decimal_to_american(float dec)
        {
            if(dec >= 2.0f) {
                return (dec - 1.0f) * 100.0f;
            }

            return (-100.0f) / (dec - 1.0f);
        }

        private float _american_to_decimal(float american)
        {
            if(american > 0.0f)
            {
                return (american / 100.0f) + 1;
            }

            return (100.0f / Math.Abs(american)) + 1;
        }

        private double _calculateAmountWon(BetData[] wagerData, float amountBet)
        {
            double multiplier = 1.0;

            foreach (var data in wagerData)
            {
                multiplier *= Math.Round(data.price, 2);
            }

            return Math.Round(multiplier * amountBet, 2);
        }


        private bool _calculateHeadToHead(BetData wagerData, GameScore score)
        {
            string teamName = wagerData.name;

            float betScore = 0.0f;
            float oppScore = 0.0f;

            foreach(var teamScore in score.scores)
            {
                /** get the score of the team betted on **/
                if (teamScore.name.Equals(teamName, StringComparison.OrdinalIgnoreCase))
                {
                    betScore = teamScore.score;
                }
                else
                {
                    oppScore = teamScore.score;
                }
            }

            if (betScore > oppScore)
            {
                return true;
            }

            return false;

        }

        private bool _calculateSpread(BetData wagerData, GameScore score)
        {
            string teamName = wagerData.name;

            float betScore = 0.0f;
            float oppScore = 0.0f;

            foreach (var teamScore in score.scores)
            {
                /** get the score of the team betted on **/
                if (teamScore.name.Equals(teamName, StringComparison.OrdinalIgnoreCase))
                {
                    betScore = teamScore.score;
                }
                else
                {
                    oppScore = teamScore.score;
                }
            }

            if ((betScore - oppScore) >= (-1 * wagerData.point))
            {
                return true;
            }

            return false;

        }

        private bool _calculateTotal(BetData wagerData, GameScore score)
        {
            float scoreTotal = 0.0f;

            foreach (var teamScore in score.scores)
            {
                scoreTotal += teamScore.score;
            }

            if ((scoreTotal > wagerData.point) && wagerData.name.Equals("Over", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            else if ((scoreTotal < wagerData.point) && wagerData.name.Equals("Under", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private float _getAmountWon(Wager currentWager, GameScore game)
        {
            BetData[] wagerData = currentWager.bet_data;

            bool didWin = false;

            /** loop through each leg of wager **/
            foreach(var data in wagerData)
            {
                /** determine if each leg won **/
                switch(data.bet_type)
                {
                    case "h2h":
                        didWin = this._calculateHeadToHead(data, game);
                        break;

                    case "spreads":
                        didWin = this._calculateSpread(data, game);
                        break;

                    case "totals":
                        didWin = this._calculateTotal(data, game);
                        break;

                    default:
                        break;
                }

                /** if a part of the bet lost, we return 0 **/
                if (!didWin)
                {
                    return 0.0f;
                }
            }

            double amount = this._calculateAmountWon(wagerData, currentWager.wager_amount);

            /** this means all legs of the bet have won **/
            return (float)amount;
        }

        public IEnumerable<Wager> updateActiveWagers(List<Wager> wagers, List<GameScore> scores)
        {
            List<Wager> result = new List<Wager>();

            /** iterate through each wager **/
            foreach (var wager in wagers)
            {
                /** find the game for current wager **/
                GameScore game = scores.Find(x => x.id == wager.game_id);

                if (game != null) 
                {
                    /** get the amount won **/
                    float amountWon = _getAmountWon(wager, game);
                    
                    Wager tmp = new Wager()
                    {
                        id = wager.id,
                        date = wager.date,
                        completed = true,
                        amount_win = amountWon,
                        american_odds = wager.american_odds,
                        decimal_odds = wager.decimal_odds,
                        user_id = wager.user_id,
                        game_id = wager.game_id,
                        bet_data = wager.bet_data,
                        wager_amount = wager.wager_amount
                    };
                    result.Add(tmp);
                }
            }

            return result;
        }
    }
}
