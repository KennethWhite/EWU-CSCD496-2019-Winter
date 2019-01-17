using System.Collections.Generic;

namespace SecretSanta.Domain.Models
{
    //Will have a title and a list of User's who are part of that group. Users can belong to more than one group
    public class Group : Entity
    {
        public string Title { get; set; }
        public List<User> Users { get; set; }
    }
}