using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SecretSanta.Domain.Models;

namespace SecretSanta.Domain.Services
{
    public class PairingService
    {
        private ApplicationDbContext DbContext { get; }

        public PairingService(ApplicationDbContext context)
        {
            DbContext = context;
        }

        public Pairing AddOrUpdatePairing(Pairing pairing)
        {
            if (pairing.Id == default(int))
                DbContext.Pairings.Add(pairing);
            else
                DbContext.Pairings.Update(pairing);
            DbContext.SaveChanges();

            return pairing;
        }

        public ICollection<Pairing> AddPairings(ICollection<Pairing> pairings)
        {
            DbContext.AddRange(pairings);
            DbContext.SaveChanges();
            return pairings;
        }

        public Pairing Find(int id)
        {
            return DbContext.Pairings.Include(pairing => pairing.Recipient)
                .Include(pairing => pairing.Santa)
                .SingleOrDefault(pairing => pairing.Id == id);
        }

        public List<Pairing> FetchAll()
        {
            var pairingTask = DbContext.Pairings
                .Include(pair => pair.Recipient)
                .Include(pair => pair.Santa)
                .ToListAsync();
            pairingTask.Wait();
            return pairingTask.Result;
        }
    }
}