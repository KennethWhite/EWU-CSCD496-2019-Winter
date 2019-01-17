using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services;

namespace SecretSanta.Domain.Tests.Services
{
    [TestClass]
    public class PairingServiceTests
    {
        private SqliteConnection SqliteConnection { get; set; }
        private DbContextOptions<ApplicationDbContext> Options { get; set; }

        [TestInitialize]
        public void OpenConnection()
        {
            SqliteConnection = new SqliteConnection("DataSource=:memory:");
            SqliteConnection.Open();

            Options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(SqliteConnection)
                .UseLoggerFactory(GetLoggerFactory())
                .EnableSensitiveDataLogging()
                .Options;

            using (var context = new ApplicationDbContext(Options))
            {
                context.Database.EnsureCreated();
            }
        }

        [TestCleanup]
        public void CloseConnection()
        {
            SqliteConnection.Close();
        }

        [TestMethod]
        public void FindPairing_CreatedPairingIsRetrievedFromDatabase()
        {
            using (var context = new ApplicationDbContext(Options))
            {
                var service = new PairingService(context);
                service.AddOrUpdatePairing(CreatePairing());
            }

            using (var context = new ApplicationDbContext(Options))
            {
                var service = new PairingService(context);
                var fetchedPairing = service.Find(1);
                Assert.AreEqual(1, fetchedPairing.Id);
                Assert.AreEqual("Steve", fetchedPairing.Recipient.FirstName);
                Assert.AreEqual("Kris", fetchedPairing.Santa.FirstName);
            }
        }

        [TestMethod]
        public void FindPairingID2_CreatedPairingIsRetrievedFromDatabase()
        {
            var pairingList = CreateThreePairings();
            using (var context = new ApplicationDbContext(Options))
            {
                var service = new PairingService(context);
                service.AddOrUpdatePairing(pairingList[0]);
                service.AddOrUpdatePairing(pairingList[1]);
            }

            using (var context = new ApplicationDbContext(Options))
            {
                var service = new PairingService(context);
                var fetchedPairing = service.Find(2);
                Assert.AreEqual(2, fetchedPairing.Id);
                Assert.AreEqual(pairingList[1].Recipient.FirstName, fetchedPairing.Recipient.FirstName);
                Assert.AreEqual(pairingList[1].Santa.FirstName, fetchedPairing.Santa.FirstName);
            }
        }
        
        [TestMethod]
        public void UpdatePairing_PairingIsUpdatedInTheDatabase()
        {
            var myPairing = CreatePairing();

            using (var context = new ApplicationDbContext(Options))
            {
                var service = new PairingService(context);
                service.AddOrUpdatePairing(myPairing);
            }

            myPairing.Recipient = CreateUser("New","Recipient");

            using (var context = new ApplicationDbContext(Options))
            {
                var service = new PairingService(context);
                service.AddOrUpdatePairing(myPairing);
            }

            using (var context = new ApplicationDbContext(Options))
            {
                var service = new PairingService(context);
                var fetchedPairing = service.Find(1);
                Assert.AreEqual(1, fetchedPairing.Id);
                Assert.AreEqual("New", fetchedPairing.Recipient.FirstName);
                Assert.AreEqual("Recipient", fetchedPairing.Recipient.LastName);
            }
        }
       
        [TestMethod]
        public void FetchPairings_CreatedPairingsAreRetrievedFromDatabase()
        {
            var pairingsToAdd = CreateThreePairings();
            using (var context = new ApplicationDbContext(Options))
            {
                var service = new PairingService(context);
                service.AddPairings(pairingsToAdd);
            }

            using (var context = new ApplicationDbContext(Options))
            {
                var service = new PairingService(context);
                var fetchedPairings = service.FetchAll();
                for (var i = 0; i < fetchedPairings.Count; i++)
                {
                    var pairingToAdd = pairingsToAdd[i];
                    var pairingFetched = fetchedPairings[i];
                    Assert.AreEqual(pairingToAdd.Id, pairingFetched.Id);
                    Assert.AreEqual(pairingToAdd.Recipient.FirstName, pairingFetched.Recipient.FirstName);
                    Assert.AreEqual(pairingToAdd.Santa.FirstName, pairingFetched.Santa.FirstName);
                }
            }
        }

        private static ILoggerFactory GetLoggerFactory()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(builder =>
            {
                builder.AddConsole()
                    .AddFilter(DbLoggerCategory.Database.Command.Name,
                        LogLevel.Information);
            });
            return serviceCollection.BuildServiceProvider().GetService<ILoggerFactory>();
        }

        private static Pairing CreatePairing(User recipient = null, User santa = null)
        {
            return new Pairing
            {
                Recipient = recipient ?? CreateUser("Steve", "Irwin"),
                Santa = santa ?? CreateUser("Kris", "Kringle")
            };
        }

        private static User CreateUser(string firstName = "Bob", string lastName = "Ross")
        {
            var user = new User
            {
                FirstName = firstName,
                LastName = lastName
            };

            var gift = new Gift
            {
                Title = "The Pragmatic Programmer",
                Description = "Book by Andrew Hunt and David Thomas",
                OrderOfImportance = 1,
                URL =
                    "https://www.amazon.com/Pragmatic-Programmer-Journeyman-Master/dp/020161622X/ref=sr_1_1?ie=UTF8&qid=1547613186&sr=8-1&keywords=the+pragmatic+programmer",
                User = user
            };

            user.Gifts = new List<Gift> {gift};
            return user;
        }

        private static List<Pairing> CreateThreePairings()
        {
            return new List<Pairing>
            {
                new Pairing
                {
                    Recipient = new User {FirstName = "Bob", LastName = "Ross"},
                    Santa = new User {FirstName = "Steve", LastName = "Irwin"}
                },
                new Pairing
                {
                    Recipient = new User {FirstName = "Mark", LastName = "Michaelis"},
                    Santa = new User {FirstName = "Kevin", LastName = "Bost"}
                },
                new Pairing
                {
                    Recipient = new User {FirstName = "Michael", LastName = "Stokesbary"},
                    Santa = new User {FirstName = "Kenny", LastName = "White"}
                }
            };
        }
    }
}