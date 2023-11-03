using Amazon.DynamoDBv2.DataModel;
using System.ComponentModel.DataAnnotations;


namespace Backend.Models
{

    [DynamoDBTable("Wagers")]
    public class Wager : WagerData
    {
        public string id { get; set; }

        public string date { get; set; }

        public bool completed { get; set; }

        public float amount_win { get; set; }


    }

    public class WagerData
    {
        [Required]
        public string user_id { get; set; }

        public string game_id { get; set; }

        public string bet_type {  get; set; }
        public BetData bet_data { get; set; }

        public float wager_amount { get; set; }


    }


    public class BetData
    {
        public string name { get; set; }
        public float price { get; set; }
        public float? point { get; set; }
    }
}
