using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SecretSanta.Domain.Models;

namespace SecretSanta.Domain.Services
{
    public class GroupService : IGroupService
    {
        private ApplicationDbContext DbContext { get; }

        public GroupService(ApplicationDbContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public Group AddGroup(Group group)
        {
            DbContext.Groups.Add(group);
            DbContext.SaveChanges();
            return group;
        }

        public Group UpdateGroup(Group group)
        {
            DbContext.Groups.Update(group);
            DbContext.SaveChanges();
            return group;
        }

        public Group RemoveGroup(Group group)
        {
            if (group == null) throw new ArgumentNullException(nameof(group));

            EntityEntry<Group> groupRemoved = DbContext.Groups.Remove(group);
            DbContext.SaveChanges();
            return groupRemoved.Entity;
        }

        public User AddUserToGroup(int groupId, User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var group = DbContext.Groups.Include(g => g.GroupUsers)
                .SingleOrDefault(g => g.Id == groupId);
            var groupUser = new GroupUser {GroupId = groupId, UserId = user.Id};
            group?.GroupUsers.Add(groupUser);
            DbContext.SaveChanges();
            return user;
        }

        public User RemoveUserFromGroup(int groupId, User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            
            var group = DbContext.Groups.Include(g => g.GroupUsers)
                .SingleOrDefault(g => g.Id == groupId);
            var userGroupToRemove = group?.GroupUsers.Single(ug => ug.GroupId == groupId && ug.UserId == user.Id);
            group?.GroupUsers.Remove(userGroupToRemove);
            DbContext.SaveChanges();
            return user;
        }

        public List<User> FetchAllUsersInGroup(int groupId)
        {
          var group = DbContext.Groups.Include(g => g.GroupUsers).ThenInclude(gu => gu.User)
                .SingleOrDefault(g => g.Id == groupId);
            var users = group?.GroupUsers.Select(gu => gu.User).ToList();
            return users;
        }


        public List<Group> FetchAllGroups()
        {
            return DbContext.Groups.ToList();
        }
    }
}