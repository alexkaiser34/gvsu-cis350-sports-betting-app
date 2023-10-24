using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Outcome
    {
        public string name { get; set; }
        public float price { get; set; }
        public float? point { get; set; }
    }

    public class Market
    {
        [Required]
        public string key { get; set; }
        public string last_update { get; set; }

        public Outcome[] outcomes { get; set; }

    }
    public class BookMaker
    {
        [Required]
        public string key { get; set; }

        public string title { get; set; }

        public string last_update { get; set; }

        public Market[] markets { get; set; }
    }
    internal class GameOdd
    {
        [Required]
        public string id { get; set; }

        public string sport_key { get; set; }

        public string sport_title { get; set; }

        public string commence_time { get; set; }

        public string home_team { get; set; }

        public string away_team { get; set; }

        public BookMaker[] bookmakers { get; set; }

    }
}
