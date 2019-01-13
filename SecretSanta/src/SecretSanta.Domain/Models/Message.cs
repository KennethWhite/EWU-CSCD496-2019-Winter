using System;
using System.Collections.Generic;
using System.Text;

namespace SecretSanta.Domain.Models
{
    // Chat used so that Recipient and Santa can hold an anonymous discussion
    class Message : Entity
    {
        Pairing Pairing { get; set; }
    }
}
