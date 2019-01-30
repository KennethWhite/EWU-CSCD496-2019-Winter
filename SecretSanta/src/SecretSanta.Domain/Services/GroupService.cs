using System;
using System.Collections.Generic;
using System.Linq;
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

        public void RemoveGroup(Group @group)
        {
            throw new NotImplementedException();
        }

        public Group AddUserToGroup(int userId, Group @group)
        {
            throw new NotImplementedException();
        }

        public void RemoveUserFromGroup(int userId, Group @group)
        {
            throw new NotImplementedException();
        }

        public List<User> FetchAllUsersInGroup(Group @group)
        {
            throw new NotImplementedException();
        }

        public List<Group> FetchAllGroups()
        {
            return DbContext.Groups.ToList();
        }
    }
}