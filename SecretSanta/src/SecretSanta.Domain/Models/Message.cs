namespace SecretSanta.Domain.Models
{
    // Chat used so that Recipient and Santa can hold an anonymous discussion
    internal class Message : Entity
    {
        private Pairing Pairing { get; set; }
    }
}