using System.ComponentModel.DataAnnotations;


namespace Backend.Models
{
    internal class GameBet
    {
        [Required]
        public string id { get; set; }

        public string sport_title { get; set; }

        public string commence_time { get; set; }

        public string home_team { get; set; }

        public string away_team { get; set; }

    }
}
