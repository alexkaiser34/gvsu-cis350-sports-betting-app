using Backend.Models;

namespace Backend.Services
{
    internal class WagerHelper
    {
        public WagerHelper() { 
        
        }

        private float _calculateHeadToHead(BetData wagerData, GameScore score, float amountBet)
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
                return wagerData.price * amountBet;
            }
            else
            {
                return 0.0f;
            }

        }

        private float _calculateSpread(BetData wagerData, GameScore score, float amountBet)
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
                return wagerData.price * amountBet;
            }
            else
            {
                return 0.0f;
            }

        }

        private float _calculateTotal(BetData wagerData, GameScore score, float amountBet)
        {
            float scoreTotal = 0.0f;

            foreach (var teamScore in score.scores)
            {
                scoreTotal += teamScore.score;
            }

            if ((scoreTotal > wagerData.point) && wagerData.name.Equals("Over", StringComparison.OrdinalIgnoreCase))
            {
                return wagerData.price * amountBet;
            }
            else if ((scoreTotal < wagerData.point) && wagerData.name.Equals("Under", StringComparison.OrdinalIgnoreCase))
            {
                return wagerData.price * amountBet;
            }
            else
            {
                return 0.0f;
            }

        }

        public IEnumerable<Wager> updateActiveWagers(List<Wager> wagers, List<GameScore> scores)
        {
            List<Wager> result = new List<Wager>();

            foreach (var wager in wagers)
            {
                GameScore game = scores.Find(x => x.id == wager.game_id);

                if (game != null) 
                {
                    BetData wagerData = wager.bet_data;
                    float amountWon = 0.0f;

                    /** h2h is the same as moneyline **/
                    switch (wager.bet_type)
                    {
                        case "h2h":
                            amountWon = this._calculateHeadToHead(wagerData, game, wager.wager_amount);
                            break;

                        case "spreads":
                            amountWon = this._calculateSpread(wagerData, game, wager.wager_amount);
                            break;

                        case "totals":
                            amountWon = this._calculateTotal(wagerData, game, wager.wager_amount);
                            break;

                        default:
                            break;
                    }



                    Wager tmp = new Wager()
                    {
                        id = wager.id,
                        date = wager.date,
                        user_id = wager.user_id,
                        game_id = wager.game_id,
                        bet_type = wager.bet_type,
                        wager_amount = wager.wager_amount,
                        bet_data = wager.bet_data,
                        amount_win = amountWon,
                        completed = true
                    };
                    result.Add(tmp);
                }
            }

            return result;
        }
    }
}
