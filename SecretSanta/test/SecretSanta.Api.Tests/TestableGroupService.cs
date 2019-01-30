using System;
using System.Collections.Generic;
using System.Linq;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services;

namespace SecretSanta.Api.Tests
{
    public class TestableGroupService : IGroupService
    {
        public List<Group> Groups { get; set; }
        public int LastGroupModified;
        
        public Group AddGroup(Group group)
        {
            LastGroupModified = group.Id;
            Groups.Add(group);
            return group;
        }

        public Group UpdateGroup(Group group)
        {
            LastGroupModified = group.Id;
            var groupToUpdate = Groups.Single(g => g.Id == group.Id);
            groupToUpdate.Name = group.Name;
            groupToUpdate.GroupUsers = group.GroupUsers;
            return group;
        }

        public void RemoveGroup(Group group)
        {
            LastGroupModified = group.Id;
            Groups.Remove(Groups.Single(g => g.Id == group.Id));
        }

        public Group AddUserToGroup(int userId, Group group)
        {
            throw new NotImplementedException();
        }

        public void RemoveUserFromGroup(int userId, Group group)
        {
            throw new NotImplementedException();
        }

        public List<User> FetchAllUsersInGroup(Group group)
        {
            throw new NotImplementedException();
        }

        public List<Group> FetchAllGroups()
        {
            throw new NotImplementedException();
        }
    }
}