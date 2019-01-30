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
        public int UpdateGiftsForUser_GiftId { get; set; }
        public int RemoveGifts_GiftId { get; set; }

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
            UpdateGiftsForUser_GiftId = gift.Id;
            var oldGift = UserGiftsToReturn.Single(g => g.Id == gift.Id);
            oldGift.Title = gift.Title;
            oldGift.UserId = gift.UserId;
            oldGift.Url = gift.Url;
            oldGift.OrderOfImportance = gift.OrderOfImportance;
            return oldGift;
        }

        public void RemoveGift(Gift gift)
        {
            RemoveGifts_GiftId = gift.Id;
            UserGiftsToReturn.Remove(UserGiftsToReturn.Single(g => g.Id == gift.Id));
        }
 
    }
}