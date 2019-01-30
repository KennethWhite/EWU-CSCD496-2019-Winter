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

        public User()
        {

        }

        public User(SecretSanta.Domain.Models.User user)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
        }

        public static Domain.Models.User ToDomain(User user)
        {
            return new Domain.Models.User
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }
    }
}