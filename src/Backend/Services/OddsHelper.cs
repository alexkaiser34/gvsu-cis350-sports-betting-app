using Backend.Models;

namespace Backend.Services
{
    internal class OddsHelper
    {

        public OddsHelper() { }

        /** convert decimal style to american style **/
        private float _decimal_to_american(float dec)
        {
            if (dec >= 2.0f)
            {
                return (float)Math.Round((dec - 1.0f) * 100.0f, 2);
            }

            return (float)Math.Round((-100.0f) / (dec - 1.0f), 2);
        }

        /** convert american style to decimal style **/
        private float _american_to_decimal(float american)
        {
            if (american > 0.0f)
            {
                return (float)Math.Round((american / 100.0f) + 1, 2);
            }

            return (float)Math.Round((100.0f / Math.Abs(american)) + 1, 2);
        }

        /** transform external API object to a more readable object to store in the database **/
        public IEnumerable<GameOdd> ApiToDB(IEnumerable<ApiGameOdd> ApiOdds)
        {
            List<GameOdd> result = new List<GameOdd>(); 

            foreach(var odd in ApiOdds)
            {
                List<Odd> oddResults = new List<Odd>();

                /** grab fanduel data **/
                BookMaker bm = odd.bookmakers.ToList().Find(x => x.key.Equals("fanduel", StringComparison.OrdinalIgnoreCase));

                /** each market corresponds to a different bet type **/
                foreach(var market in bm.markets)
                {
                    List<Outcome> outcomes = new List<Outcome>();

                    /** each outcome corresponds to a wager **/
                    foreach(var outcome in market.outcomes)
                    {

                        /** store wager outcome information **/
                        Outcome tmp_outcome = new Outcome()
                        {
                            name = outcome.name,
                            decimal_odd = outcome.price,
                            american_odd = _decimal_to_american(outcome.price),
                            point = outcome.point
                        };
                        outcomes.Add(tmp_outcome);
                    }


                    /** store list of outcomes for each market **/
                    Odd tmp_odd = new Odd()
                    {
                        bet_type = market.key,
                        outcomes = outcomes.ToArray()
                    };

                    oddResults.Add(tmp_odd);

                }

                /** use list of odds with API data to create cleaner model **/
                GameOdd tmp = new GameOdd()
                {
                    id = odd.id,
                    sport_key = odd.sport_key,
                    sport_title = odd.sport_title,
                    commence_time = odd.commence_time,
                    home_team = odd.home_team,
                    away_team = odd.away_team,
                    odds = oddResults.ToArray()
                };

                result.Add(tmp);
            }

            return result;

        }
    }
}
