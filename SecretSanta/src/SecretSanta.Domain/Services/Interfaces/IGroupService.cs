using SecretSanta.Domain.Models;
using System.Collections.Generic;

namespace SecretSanta.Domain.Services.Interfaces
{
    public interface IGroupService
    {
        Group Find(int id);
        List<Group> FetchAll();

        Group AddGroup(Group group);

        Group UpdateGroup(Group group);

        bool DeleteGroup(int groupId);

        bool AddUserToGroup(int groupId, int userId);

        bool RemoveUserFromGroup(int groupId, int userId);
    }
}
