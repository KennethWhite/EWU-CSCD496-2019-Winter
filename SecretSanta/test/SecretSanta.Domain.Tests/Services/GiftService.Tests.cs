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
    public class GiftServiceTests
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
        public void AddGift_GiftIsAddedIntoDatabase()
        {
            using (var dbContext = new ApplicationDbContext(Options))
            {
                var service = new GiftService(dbContext);
                var giftFromDb = service.AddOrUpdateGift(CreateGift());
                Assert.AreNotEqual(0, giftFromDb.Id);
            }
        }

        [TestMethod]
        public void FindGift_CreatedGiftIsRetrievedFromDatabase()
        {
            using (var context = new ApplicationDbContext(Options))
            {
                var service = new GiftService(context);
                var myGift = CreateGift();
                service.AddOrUpdateGift(myGift);
            }

            using (var context = new ApplicationDbContext(Options))
            {
                var service = new GiftService(context);
                var fetchedGift = service.Find(1);
                Assert.AreEqual(1, fetchedGift.Id);
            }
        }

        [TestMethod]
        public void FindGiftID2_CreatedGiftIsRetrievedFromDatabase()
        {
            var giftList = CreateFiveGifts();
            using (var context = new ApplicationDbContext(Options))
            {
                new GiftService(context).AddGifts(giftList);
            }

            using (var context = new ApplicationDbContext(Options))
            {
                var fetchedGift = new GiftService(context).Find(2);
                Assert.AreEqual(2, fetchedGift.Id);
                Assert.AreEqual(giftList[1].Title, fetchedGift.Title);
                Assert.AreEqual(giftList[1].Description, fetchedGift.Description);
            }
        }

        [TestMethod]
        public void UpdateGift_GiftIsUpdatedInTheDatabase()
        {
            var myGift = CreateGift();
            using (var context = new ApplicationDbContext(Options))
            {
                new GiftService(context).AddOrUpdateGift(myGift);
            }

            myGift.Title = "New Item";
            myGift.Description = "A super cool gift";

            using (var context = new ApplicationDbContext(Options))
            {
                new GiftService(context).AddOrUpdateGift(myGift);
            }

            using (var context = new ApplicationDbContext(Options))
            {
                var fetchedGift = new GiftService(context).Find(1);
                Assert.AreEqual(1, fetchedGift.Id);
                Assert.AreEqual("New Item", fetchedGift.Title);
                Assert.AreEqual("A super cool gift", fetchedGift.Description);
            }
        }

        [TestMethod]
        public void RemoveGift_GiftIsRemovedFromUser()
        {
            var user = new User {FirstName = "Steve", LastName = "Irwin", Gifts = CreateFiveGifts()};
            using (var context = new ApplicationDbContext(Options))
            {
                new UserService(context).AddOrUpdateUser(user);
            }

            var initialGifts = CreateFiveGifts();
            int initialCount = initialGifts.Count, numOfGiftsRemoved = 0;
            while (user.Gifts.Count > 0)
            {
                using (var context = new ApplicationDbContext(Options))
                {
                    var fetchedUser = new UserService(context).Find(user.Id);
                    Assert.AreEqual(initialCount - numOfGiftsRemoved, fetchedUser.Gifts.Count);
                    Assert.AreEqual(initialGifts[numOfGiftsRemoved].Id, fetchedUser.Gifts[0].Id);
                    Assert.AreEqual(initialGifts[numOfGiftsRemoved].Title, fetchedUser.Gifts[0].Title);
                    Assert.AreEqual(initialGifts[numOfGiftsRemoved].Description, fetchedUser.Gifts[0].Description);
                }

                using (var context = new ApplicationDbContext(Options))
                {
                    new GiftService(context).RemoveGift(user.Gifts[0]);
                    numOfGiftsRemoved++;
                }
            }
        }

        [TestMethod]
        public void FetchGifts_CreatedGiftsAreRetrievedFromDatabase()
        {
            using (var context = new ApplicationDbContext(Options))
            {
                new GiftService(context).AddGifts(CreateFiveGifts());
            }

            using (var context = new ApplicationDbContext(Options))
            {
                var initialGifts = CreateFiveGifts();
                var fetchedGifts = new GiftService(context).FetchAll();
                for (var i = 0; i < fetchedGifts.Count; i++)
                {
                    var giftToAdd = initialGifts[i];
                    var giftFetched = fetchedGifts[i];
                    Assert.AreEqual(giftToAdd.Id, giftFetched.Id);
                    Assert.AreEqual(giftToAdd.Title, giftFetched.Title);
                }
            }
        }

        private static Gift CreateGift()
        {
            return new Gift
            {
                Title = "The Pragmatic Programmer",
                Description = "Book by Andrew Hunt and David Thomas",
                OrderOfImportance = 1,
                URL =
                    "https://www.amazon.com/Pragmatic-Programmer-Journeyman-Master/dp/020161622X/ref=sr_1_1?ie=UTF8&qid=1547613186&sr=8-1&keywords=the+pragmatic+programmer",
            };
        }

        private static List<Gift> CreateFiveGifts()
        {
            var gifts = new List<Gift>();
            for (var i = 1; i < 6; i++)
            {
                gifts.Add(new Gift
                {
                    Id = i, Title = "Item " + i, Description = $"Item {i} description",
                    OrderOfImportance = i, URL = $"https://www.item{i}.com"
                });
            }
            return gifts;
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
    }
}