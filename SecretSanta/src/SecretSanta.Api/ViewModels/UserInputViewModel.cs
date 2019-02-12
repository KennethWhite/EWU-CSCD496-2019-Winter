using System.ComponentModel.DataAnnotations;

namespace SecretSanta.Api.ViewModels
{
    public class UserInputViewModel
    {
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
