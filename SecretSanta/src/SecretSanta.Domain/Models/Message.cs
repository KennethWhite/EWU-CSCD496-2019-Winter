namespace SecretSanta.Domain.Models
{
    // Chat used so that Recipient and Santa can hold an anonymous discussion
    public class Message : Entity
    {
        public Pairing Pairing { get; set; }
    }
}