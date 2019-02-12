namespace SecretSanta.Api.ViewModels
{
    public class GroupUserViewModel
    {
        public int GroupId { get; set; }
        public GroupViewModel Group { get; set; }
        public int UserId { get; set; }
        public UserViewModel User { get; set; }
    }
}
