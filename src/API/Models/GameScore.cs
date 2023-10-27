using Amazon.DynamoDBv2.DataModel;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Score
    {
        public string name { get; set; }
        public int score { get; set; }

    }

    [DynamoDBTable("GameScores")]
    public class GameScore
    {
        [Required]
        public string id { get; set; }

        public string sport_key { get; set; }

        public string sport_title { get; set; }

        public string commence_time { get; set; }

        public bool completed { get; set; }

        public string home_team { get; set; }

        public string away_team { get; set; }

        public Score[] scores { get; set; }

        public string last_update { get; set; }


    }

}