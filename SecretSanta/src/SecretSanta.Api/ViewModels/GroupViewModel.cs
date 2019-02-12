using System.Collections.Generic;

namespace SecretSanta.Api.ViewModels
{
    public class GroupViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<GroupUserViewModel> GroupUsers { get; set; }
    }
}
