using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecretSanta.Domain.Models
{
    //- hold the user's first and last name, list of gifts, groups the user belongs to
    public class User : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Gift> Gifts { get; set; }
        [NotMapped]
        public List<Group> Groups { get; set; }
        private List<Gift> Wishlist { get; set; }
    }
}