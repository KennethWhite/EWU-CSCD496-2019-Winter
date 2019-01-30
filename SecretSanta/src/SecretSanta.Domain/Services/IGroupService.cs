using System.Collections.Generic;
using SecretSanta.Domain.Models;

namespace SecretSanta.Domain.Services
{
    public interface IGroupService
    {
        
        
        Group AddGroup(Group group);

        Group UpdateGroup(Group group);

        void RemoveGroup(Group group);

        Group AddUserToGroup(int userId, Group group);

        void RemoveUserFromGroup(int userId, Group group);

        List<User> FetchAllUsersInGroup(Group group);

        List<Group> FetchAllGroups();
    }
}