using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace sports_betting_app.Models
{

    public class BetButtonData
    {
        public BetData data { get; set; }
        public bool isOver { get; set; }
    }
    public class Wager : WagerData
    {
        public string id { get; set; }

        public string date { get; set; }

        public bool completed { get; set; }

        public float amount_win { get; set; }

        public float american_odds { get; set; }

        public float decimal_odds { get; set; }


    }

    public class WagerData
    {
        [Required]
        public string user_id { get; set; }

        public List<BetData> bet_data { get; set; }

        public float wager_amount { get; set; }


    }

    public class BetData
    {
        [Required]
        public string game_id { get; set; }

        public string bet_type { get; set; }
        public string name { get; set; }
        public float price { get; set; }
        public float? point { get; set; }

        public string gameTitle { get; set; }
    }
}
