using Amazon.DynamoDBv2.DataModel;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    [DynamoDBTable("GameOdds")]
    internal class GameOdd
    {
        [Required]
        public string id { get; set; }

        public string sport_key { get; set; }

        public string sport_title { get; set; }

        public string commence_time { get; set; }

        public string home_team { get; set; }

        public string away_team { get; set; }

        public Odd[] odds { get; set; }

    }

    public class Odd
    {
        [Required]
        public string bet_type { get; set; }

        public Outcome[] outcomes { get; set; }

    }
    public class Outcome
    {
        public string name { get; set; }
        public float decimal_odd { get; set; }
        public float american_odd { get; set; }
        public float? point { get; set; }
    }


}
