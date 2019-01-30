using System.Collections.Generic;
using System;
using System.Linq;

namespace SecretSanta.Api.DTO
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Gift> Gifts { get; set; }
        public List<GroupUser> GroupUsers { get; set; }

        public User()
        {

        }

        public User(SecretSanta.Domain.Models.User user)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            //Not sure this needs to be exposed to the API
            //Gifts = user.Gifts?.Select(domGift => new DTO.Gift(domGift)).ToList();
            //GroupUsers = user.GroupUsers?.Select(domGroupUser => new DTO.GroupUser(domGroupUser)).ToList();
        }

        public static Domain.Models.User ToDomain(User user)
        {
            return new Domain.Models.User
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                //Gifts = user.Gifts?.Select(domGift => Gift.ToDomain(domGift)).ToList
                //GroupUsers = user.GroupUsers.Select(domGroupUser => GroupUser.ToDomain(domGroupUser)).ToList()
            };
        }

    }
}