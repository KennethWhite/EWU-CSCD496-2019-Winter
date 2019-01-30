using SecretSanta.Domain.Models;

namespace SecretSanta.Api.DTO
{
    public class GroupUser
    {
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }

        public GroupUser()
        {

        }
        public GroupUser(Domain.Models.GroupUser domGroupUser)
        {
            GroupId = domGroupUser.GroupId;
            Group = domGroupUser.Group;
            UserId = domGroupUser.UserId;
            User = new User(domGroupUser.User);
        }

    }
}