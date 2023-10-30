using Amazon.DynamoDBv2.DataModel;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    [DynamoDBTable("Users")]
    public class User : UserData
    {
        public string id { get; set; }

        

        // TODO: Add model for a wager and keep a list of wagers with each user
        // May be best to keep a seperate table
    }

    public class UserData
    {
        [Required]
        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }
    }

}
