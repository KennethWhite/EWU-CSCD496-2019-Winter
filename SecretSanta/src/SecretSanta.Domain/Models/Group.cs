using System.Collections.Generic;

namespace SecretSanta.Domain.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<GroupUser> GroupUsers { get; set; }
    }
}