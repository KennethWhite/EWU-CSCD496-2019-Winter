using SecretSanta.Domain.Models;
using System.Collections.Generic;

namespace SecretSanta.Domain.Services.Interfaces
{
    public interface IGiftService
    {
        List<Gift> GetGiftsForUser(int userId);
    }
}
