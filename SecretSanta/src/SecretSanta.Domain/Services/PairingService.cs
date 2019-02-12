using Microsoft.EntityFrameworkCore;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretSanta.Domain.Services
{

    public class PairingService : IPairingService
    {
        private ApplicationDbContext DbContext { get; }
        private static Random _rng = new Random();



        public PairingService(ApplicationDbContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<List<Pairing>> GenerateUserPairings(int groupId)
        {
            Group foundGroup = await DbContext.Groups
                .Include(x => x.GroupUsers)
                .FirstOrDefaultAsync(x => x.Id == groupId);
            List<Pairing> pairings = await Task.Run<List<Pairing>>(() => GenerateUserPairings(foundGroup));
            await DbContext.Pairings.AddRangeAsync(pairings);
            await DbContext.SaveChangesAsync();
            return pairings;
        }

        private List<Pairing> GenerateUserPairings(Group groupToPair)
        {
            List<int> userIds = groupToPair?.GroupUsers?.Select(gu => gu.UserId).ToList();
            List<Pairing> generatedPairings = new List<Pairing>();
            while (userIds.Count > 0)
            {
                //TODO
            }
            return generatedPairings;
        }

    }
}
