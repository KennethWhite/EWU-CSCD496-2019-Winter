using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services;

namespace SecretSanta.Domain.Tests.Services
{
    [TestClass]
    public class PairingServiceTests : DatabaseServiceTests
    {

        [TestInitialize]
        public async Task SetupUsersAndGroupForTests()
        {
            using (var context = new ApplicationDbContext(Options))
            {
                var groupService = new GroupService(context);
                var userService = new UserService(context);

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
                    LastName = "Quixote"
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
        public async Task GeneratePairings_EachPairHasUniqueRecipient()
        {
            using (var context = new ApplicationDbContext(Options))
            {
                var pairingService = new PairingService(context);
                var userPairings = await pairingService.GenerateUserPairings(1);
                var sortedPairings = userPairings.OrderBy(pair => pair.RecipientId).ToList();
                for (var i = 0; i < sortedPairings.Count; i++)
                {
                    var pair = sortedPairings[i];
                    Assert.AreEqual(i + 1, pair.RecipientId);
                }
            }
        }


        [TestMethod]
        public async Task GeneratePairings_EachPairHasUniqueSanta()
        {
            using (var context = new ApplicationDbContext(Options))
            {
                var pairingService = new PairingService(context);
                var userPairings = await pairingService.GenerateUserPairings(1);
                var sortedPairings = userPairings.OrderBy(pair => pair.SantaId).ToList();
                for (var i = 0; i < sortedPairings.Count; i++)
                {
                    var pair = sortedPairings[i];
                    Assert.AreEqual(i + 1, pair.SantaId);
                }
            }
        }
        
        [TestMethod]
        public async Task GeneratePairings_RecipientIsNotOwnSanta()
        {
            using (var context = new ApplicationDbContext(Options))
            {
                var pairingService = new PairingService(context);
                List<Pairing> userPairings = await pairingService.GenerateUserPairings(1);
                foreach (var pair in userPairings)
                {
                    Assert.AreNotEqual<int>(pair.SantaId, pair.RecipientId);
                }
            }
        }
        
        
        [TestMethod]
        public async Task GeneratePairings_ConsecutiveListsAreDifferent()
        {
            using (var context = new ApplicationDbContext(Options))
            {
                var pairingService = new PairingService(context);
                List<Pairing> userPairings = await pairingService.GenerateUserPairings(1);
                List<Pairing> secondaryPairings = await pairingService.GenerateUserPairings(1);
                userPairings.OrderBy(pair => pair.SantaId);
                secondaryPairings.OrderBy(pair => pair.SantaId);
                
                Assert.AreEqual<int>(userPairings.Count, secondaryPairings.Count);
                bool samePairings = true;
                for (int index = 0; index < userPairings.Count; index++)
                {
                    samePairings = samePairings && 
                                   (userPairings[index].RecipientId == secondaryPairings[index].RecipientId);
                }
                Assert.IsFalse(samePairings);
            }
        }
        
    }
}