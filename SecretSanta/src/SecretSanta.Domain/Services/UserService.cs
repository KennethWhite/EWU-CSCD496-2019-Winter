using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SecretSanta.Domain.Models;

namespace SecretSanta.Domain.Services
{
    //Need to be able to add and update a user, not worried about deleting
    public class UserService
    {
        public UserService(ApplicationDbContext context)
        {
            DbContext = context;
        }

        private ApplicationDbContext DbContext { get; }

        public User AddOrUpdateUser(User user)
        {
            if (user.Id == default(int))
                DbContext.Users.Add(user);
            else
                DbContext.Users.Update(user);
            DbContext.SaveChanges();

            return user;
        }

        public ICollection<User> AddUsers(ICollection<User> users)
        {
            DbContext.AddRange(users);
            DbContext.SaveChanges();
            return users;
        }
        

        public User Find(int id)
        {
            return DbContext.Users.Include(user => user.Wishlist)
                .SingleOrDefault(user => user.Id == id);
        }

        public List<User> FetchAll()
        {
            var userTask = DbContext.Users.ToListAsync();
            userTask.Wait();
            return userTask.Result;
        }
    }
}