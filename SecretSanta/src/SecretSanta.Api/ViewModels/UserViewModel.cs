using System.ComponentModel.DataAnnotations;

namespace SecretSanta.Api.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
