using System.ComponentModel.DataAnnotations;


namespace sports_betting_app.Models
{

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

        public string game_id { get; set; }

        public BetData[] bet_data { get; set; }

        public float wager_amount { get; set; }

    }

    public class BetData
    {
        public string bet_type { get; set; }
        public string name { get; set; }
        public float price { get; set; }
        public float? point { get; set; }
    }
}
