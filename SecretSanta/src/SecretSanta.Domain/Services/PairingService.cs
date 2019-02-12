using Microsoft.EntityFrameworkCore;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SecretSanta.Domain.Services
{

    public class PairingService : IPairingService
    {
        private ApplicationDbContext DbContext { get; }

        public PairingService(ApplicationDbContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<List<Pairing>> GenerateUserPairings(int groupId)
        {
            Group foundGroup = await DbContext.Groups
                .Include(x => x.GroupUsers)
                .FirstOrDefaultAsync(x => x.Id == groupId);
            return await Task.Run<List<Pairing>>(() =>GenerateUserPairings());
        }

        private List<Pairing> GenerateUserPairings()
        {

        }

    }
}
