using System.Collections.Generic;
using SecretSanta.Domain.Services;
using SecretSanta.Domain.Models;

namespace SecretSanta.Api.Tests
{
    public class TestableGiftService : IGiftService
    {
        public List<Gift> ToReturn { get; set; }
        public int GetGiftsForUser_UserId { get; set; }
        public int AddGiftsForUser_UserId { get; set; }

        public List<Gift> GetGiftsForUser(int userId)
        {
            GetGiftsForUser_UserId = userId;
            return ToReturn;
        }

        public Gift AddGiftToUser(int userId, Gift gift)
        {
            AddGiftsForUser_UserId = userId;
            ToReturn.Add(gift);
            return gift;
        }
    }
}