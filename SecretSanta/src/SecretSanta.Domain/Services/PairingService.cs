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
        private IRandom Rand { get; }

        public PairingService(ApplicationDbContext dbContext, IRandom rand = null)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            Rand = rand ?? new ThreadSafeRandom();
        }

        public async Task<List<Pairing>> GenerateUserPairings(int groupId)
        {
            var userIds = await DbContext.Groups
                .Where(g => g.Id == groupId)
                .SelectMany(g => g.GroupUsers, (g, gu) => gu.UserId)
                .ToListAsync();

            List<Pairing> pairings = await Task.Run(() => GenerateUserPairings(userIds));

            await DbContext.Pairings.AddRangeAsync(pairings);
            await DbContext.SaveChangesAsync();
            return pairings;
        }

        private List<Pairing> GenerateUserPairings(List<int> userIds)
        {
            List<Pairing> generatedPairings = new List<Pairing>();
            var rand = Rand;
            var randUserIds = userIds.OrderBy(id => rand.Next()).ToList();

            for (var i = 0; i < randUserIds.Count - 1; i++)
            {
                generatedPairings.Add(new Pairing {RecipientId = randUserIds[i], SantaId = randUserIds[i + 1]});
            }
            generatedPairings.Add(new Pairing {RecipientId = randUserIds.Last(), SantaId = randUserIds.First()});

            return generatedPairings;
        }
    }

    /**
     * A global instance of random is created. It serves as the seed for a local random object
     * Whenever a ThreadSafeRandom object is created, the global random is locked and its Next() method
     * is called and used as the seed for a local random. Then the local random object can be used freely
     * Need to find a way to test it though.
     */
    public class ThreadSafeRandom : IRandom
    {
        private static readonly Random _globalRandom = new Random();
        [ThreadStatic] private static Random _localRandom;


        public ThreadSafeRandom()
        {
            EnsureRandIsValid();
        }

        private void EnsureRandIsValid()
        {
            if (_localRandom == null)
            {
                lock (_globalRandom)
                {
                    _localRandom = new Random(_globalRandom.Next());
                }
            }
        }

        public int Next()
        {
            EnsureRandIsValid();
            return _localRandom.Next();
        }

        public int Next(int min, int max)
        {
            EnsureRandIsValid();
            return _localRandom.Next(min, max);
        }
    }

    public interface IRandom
    {
        int Next();
        int Next(int min, int max);
    }
}