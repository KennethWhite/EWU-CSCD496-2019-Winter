using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services;
using SecretSanta.Domain.Tests.Helpers;

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
        public async Task GeneratePairings_PairsGeneratedAreRandomized()
        {
            using (var context = new ApplicationDbContext(Options))
            {
                var pairingService = new PairingService(context);
                List<Pairing> firstPairings = await pairingService.GenerateUserPairings(1);
                List<Pairing> secondPairings = await pairingService.GenerateUserPairings(1);

                Assert.AreEqual<int>(firstPairings.Count, secondPairings.Count);
                bool samePairings = true;
                for (int index = 0; index < firstPairings.Count; index++)
                {
                    samePairings = samePairings && firstPairings[index].RecipientId == secondPairings[index].RecipientId;
                }

                Assert.IsFalse(samePairings);
            }
        }

        [TestMethod]
        public async Task GeneratePairings_CorrectRandomizedPairingsCreated()
        {
            //ensures that the randomize is working correctly. In this case ensures the pairs are not random
            //ie the recipientId of each pair is the same as its id, and its santa is the id + 1
            using (var context = new ApplicationDbContext(Options))
            {
                var testableRandom = new TestableRandom(new List<int> {0, 1, 2});
                var pairingService = new PairingService(context, testableRandom);
                List<Pairing> userPairings = await pairingService.GenerateUserPairings(1);
                for (var i = 0; i < userPairings.Count -1; i++)
                {
                    var curPair = userPairings[i];
                    Assert.AreEqual(curPair.Id, curPair.RecipientId);
                    Assert.AreEqual(curPair.Id + 1, userPairings[i].SantaId);
                }
                Assert.AreEqual(1, userPairings.Last().SantaId);
            }
        }

        [TestMethod]
        public async Task GeneratePairings_ExistingPairsAreRemoved()
        {
            using (var context = new ApplicationDbContext(Options))
            {
                var pairingService = new PairingService(context);
                await pairingService.GenerateUserPairings(1);
            }
            
            using (var context = new ApplicationDbContext(Options))
            {
                var existingPairs = context.Pairings.ToList();
                var pairingService = new PairingService(context);
                await pairingService.GenerateUserPairings(1);
                var newPairs = context.Pairings.ToList();

                //if existing pairs are not removed, the number of pairs in newPairs would be doubled because the number of new
                // pairs would be equal to number of old pairs + number of new pairs
                Assert.AreEqual<int>(existingPairs.Count, newPairs.Count);    
            }
        }

        [TestMethod]
        public async Task GeneratePairings_PairsGeneratedAreSpecificAGroup()
        {
            using (var context = new ApplicationDbContext(Options))
            {
                var groupService = new GroupService(context);
                var userService = new UserService(context);

                var user = new User
                {
                    FirstName = "Bob",
                    LastName = "Ross"
                };
                var user2 = new User
                {
                    FirstName = "Steve",
                    LastName = "Irwin"
                };

                user = await userService.AddUser(user);
                user2 = await userService.AddUser(user2);

                var group = new Group
                {
                    Name = "Test Group 2"
                };

                group = await groupService.AddGroup(group);

                await groupService.AddUserToGroup(group.Id, user.Id);
                await groupService.AddUserToGroup(group.Id, user2.Id);
            }

            using (var context = new ApplicationDbContext(Options))
            {
                var pairingService = new PairingService(context);
                var pairsGroup1 = await pairingService.GenerateUserPairings(1);
                var pairsGroup2 = await pairingService.GenerateUserPairings(2);
                var totalPairings = context.Pairings.Count();
                
                Assert.AreNotEqual(pairsGroup1.Count, pairsGroup2.Count);
                //total number of pairs = num pairs from group1 + num pairs from group2
                Assert.AreEqual(totalPairings, pairsGroup1.Count + pairsGroup2.Count);
                pairsGroup1.ForEach(pair1 => Assert.AreEqual(1, pair1.GroupId));
                pairsGroup2.ForEach(pair2 => Assert.AreEqual(2, pair2.GroupId));
            }
        }

        [TestMethod]
        public async Task GeneratePairings_PairsOnlyDifferByGroup()
        {
            using (var context = new ApplicationDbContext(Options))
            {
                var groupService = new GroupService(context);
                var userService = new UserService(context);

                var user = new User
                {
                    FirstName = "Bob",
                    LastName = "Ross"
                };
                var user2 = new User
                {
                    FirstName = "Steve",
                    LastName = "Irwin"
                };

                user = await userService.AddUser(user);
                user2 = await userService.AddUser(user2);

                var group1 = new Group
                {
                    Name = "Test Group 1"
                };
                var group2 = new Group
                {
                    Name = "Test Group 2"
                };

                group1 = await groupService.AddGroup(group1);
                group2 = await groupService.AddGroup(group2);


                await groupService.AddUserToGroup(group1.Id, user.Id);
                await groupService.AddUserToGroup(group1.Id, user2.Id);
                await groupService.AddUserToGroup(group2.Id, user.Id);
                await groupService.AddUserToGroup(group2.Id, user2.Id);
            }

            using (var context = new ApplicationDbContext(Options))
            {
                var testableRandom = new TestableRandom(new List<int>{0, 1}); //ensures the pairs generated for both groups are randomized the same
                var pairingService = new PairingService(context, testableRandom);
                var pairsGenerated1 = await pairingService.GenerateUserPairings(2);
                testableRandom.Index = 0;
                var pairsGenerated2 = await pairingService.GenerateUserPairings(3);
                
                for (var i = 0; i < pairsGenerated1.Count; i++)
                {
                    var pairGroup1 = pairsGenerated1[i];
                    var pairsGroup2 = pairsGenerated2[i];
                    Assert.AreEqual(pairGroup1.RecipientId, pairsGroup2.RecipientId);                    
                    Assert.AreEqual(pairGroup1.SantaId, pairsGroup2.SantaId);
                    Assert.AreNotEqual(pairGroup1.GroupId, pairsGroup2.GroupId);
                }
                
            }
        }
    }
}