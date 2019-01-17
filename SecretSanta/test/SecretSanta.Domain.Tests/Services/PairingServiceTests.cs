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
    }
}