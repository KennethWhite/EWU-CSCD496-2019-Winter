using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace SecretSanta.Domain.Tests.Services
{
    [TestClass]
    public class PairingServiceTests : DatabaseServiceTests
    {

        private Group _sut { get; set; }

        [TestInitialize]
        public async Task SetupUsersAndGroupForTests()
        {
            using (var context = new ApplicationDbContext(Options))
            {
                GroupService groupService = new GroupService(context);
                UserService userService = new UserService(context);

                var user = new User
                {
                    FirstName = "Inigo",
                    LastName = "Montoya"
                };
                var user2 = new User
                {
                    FirstName = "Princess",
                    LastName = "Buttercup"
                };
                var user3 = new User
                {
                    FirstName = "Don",
                    LastName = "Quixote",
                };

                user = await userService.AddUser(user);
                user2 = await userService.AddUser(user2);
                user3 = await userService.AddUser(user3);

                var group = new Group
                {
                    Name = "Test Group"
                };

                group = await groupService.AddGroup(group);

                await groupService.AddUserToGroup(group.Id, user.Id);
                await groupService.AddUserToGroup(group.Id, user2.Id);
                await groupService.AddUserToGroup(group.Id, user3.Id);
            }
        }


        [TestMethod]
        public async Task GeneratePairings_SantasHaveOnlyOneRecipient()
        {
            using (var context = new ApplicationDbContext(Options))
            {
                PairingService pairingService = new PairingService(context);
                List<Pairing> userPairings = await pairingService.GenerateUserPairings(1);
                var sortedPairings = userPairings.OrderBy(u => u.SantaId).ToList();
                int prevSantaId = 0;
                sortedPairings.ForEach(pair => Assert.AreNotEqual(prevSantaId, pair.SantaId));

                IEnumerable<IGrouping<User, Pairing>> groupsBySantaId = userPairings.GroupBy(pair => pair.Santa);
                foreach (var santaIdGroup in groupsBySantaId)
                {
                    var count = santaIdGroup.Count();
                }

            }
        }

        
        [TestMethod]
        public async Task GeneratePairings_EachPairHasARecipient()
        {
            using (var context = new ApplicationDbContext(Options))
            {
                PairingService pairingService = new PairingService(context);
                List<Pairing> userPairings = await pairingService.GenerateUserPairings(1);
                var sortedPairings = userPairings.OrderBy(u => u.RecipientId).ToList();
                var prevRecipientId = 0;
                sortedPairings.ForEach(pair => Assert.AreNotEqual(prevRecipientId, pair.RecipientId));
                Assert.AreNotEqual(sortedPairings.Last().SantaId, sortedPairings.First().RecipientId);

            }
        }
    }
}