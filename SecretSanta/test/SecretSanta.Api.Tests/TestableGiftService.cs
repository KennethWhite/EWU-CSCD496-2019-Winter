using System.Collections.Generic;
using System.Linq;
using SecretSanta.Domain.Services;
using SecretSanta.Domain.Models;

namespace SecretSanta.Api.Tests
{
    public class TestableGiftService : IGiftService
    {
        public List<Gift> UserGiftsToReturn { get; set; }
        public int GetGiftsForUser_UserId { get; set; }
        public int AddGiftsForUser_UserId { get; set; }
        public int UpdateGiftsForUser_UserId { get; set; }

        public Gift GiftDeleted { get; set; }

        public List<Gift> GetGiftsForUser(int userId)
        {
            GetGiftsForUser_UserId = userId;
            return UserGiftsToReturn;
        }

        public Gift AddGiftToUser(int userId, Gift gift)
        {
            AddGiftsForUser_UserId = userId;
            UserGiftsToReturn.Add(gift);
            return gift;
        }

        public Gift UpdateGiftForUser(int userId, Gift gift)
        {
            UpdateGiftsForUser_UserId = userId;
            //UserGiftsToReturn.First(g => g.Id == gift.Id) = gift;
            throw new System.NotImplementedException();
        }

        public void RemoveGift(Gift gift)
        {
            GiftDeleted = gift;
            UserGiftsToReturn.Remove(gift);
        }
 
    }
}