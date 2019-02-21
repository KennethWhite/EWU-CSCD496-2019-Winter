using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecretSanta.Api.Tests
{
    public class TestableGiftService : IGiftService
    {
        public List<Gift> ToReturn { get; set; }
        public int GetGiftsForUser_UserId { get; set; }

#pragma warning disable 1998
        public async Task<List<Gift>> GetGiftsForUser(int userId)
#pragma warning restore 1998
        {
            GetGiftsForUser_UserId = userId;
            return ToReturn;
        }
    }
}
