using System.Collections.Generic;
using System.Linq;
using SecretSanta.Domain.Services;
using SecretSanta.Domain.Models;

namespace SecretSanta.Api.Tests
{
    public class TestableGiftService : IGiftService
    {
        public List<Gift> AllGifts { get; set; }
        public int GetGiftsForUser_UserId { get; set; }
        public int AddGiftsForUser_UserId { get; set; }
        public int UpdateGiftsForUser_GiftId { get; set; }
        public int RemoveGifts_GiftId { get; set; }

        public List<Gift> GetGiftsForUser(int userId)
        {
            GetGiftsForUser_UserId = userId;
            return AllGifts;
        }

        public Gift AddGiftToUser(int userId, Gift gift)
        {
            AddGiftsForUser_UserId = userId;
            AllGifts.Add(gift);
            return gift;
        }

        public Gift UpdateGiftForUser(int userId, Gift gift)
        {
            UpdateGiftsForUser_GiftId = gift.Id;
            var giftToUpdate = AllGifts.Single(g => g.Id == gift.Id);
            giftToUpdate.Title = gift.Title;
            giftToUpdate.UserId = gift.UserId;
            giftToUpdate.Url = gift.Url;
            giftToUpdate.OrderOfImportance = gift.OrderOfImportance;
            return gift;
        }

        public void RemoveGift(Gift gift)
        {
            RemoveGifts_GiftId = gift.Id;
            AllGifts.Remove(AllGifts.Single(g => g.Id == gift.Id));
        }
 
    }
}