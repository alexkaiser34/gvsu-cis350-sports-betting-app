using Amazon.DynamoDBv2.DataModel;
using System.ComponentModel.DataAnnotations;


namespace API.Models
{

    [DynamoDBTable("Wagers")]
    public class Wager : WagerData
    {
        public string id { get; set; }
    }

    public class WagerData
    {
        [Required]
        public string user_id { get; set; }

        public string game_id { get; set; }

        public string bet_type {  get; set; }
        
        public float wager_amount { get; set; }

        public float amount_win { get; set; }

        public bool didWagerWin { get; set; }
    }
}
