using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SecretSanta.Domain.Models;

namespace SecretSanta.Domain.Services
{
    public class GiftService
    {
        private ApplicationDbContext DbContext { get; }
        
        public GiftService(ApplicationDbContext context)
        {
            DbContext = context;
        }

        public Gift AddOrUpdateGift(Gift gift)
        {
            if (gift.Id == default(int))
                DbContext.Gifts.Add(gift);
            else
                DbContext.Gifts.Update(gift);
            DbContext.SaveChanges();

            return gift;
        }

        public void RemoveGift(Gift gift)
        {
            DbContext.Gifts.Remove(gift);    //this removes from database completely, not just user
            DbContext.SaveChanges();
        }
        
        public ICollection<Gift> AddGifts(ICollection<Gift> gifts)
        {
            DbContext.AddRange(gifts);
            DbContext.SaveChanges();
            return gifts;
        }
        
        public Gift Find(int id)
        {
            return DbContext.Gifts.Include(gift => gift.User)
                .SingleOrDefault(gift => gift.Id == id);
        }

        public List<Gift> FetchAll()
        {
            var giftTask = DbContext.Gifts.ToListAsync();
            giftTask.Wait();
            return giftTask.Result;
        }
    }
}