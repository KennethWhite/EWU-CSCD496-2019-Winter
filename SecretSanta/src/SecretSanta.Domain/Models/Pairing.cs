namespace SecretSanta.Domain.Models
{
    // Holds the User who is the recipient and the User who is the Santa for each group
    public class Pairing : Entity
    {
        public User Recipient { get; set; }
        public User Santa { get; set; }
    }
}