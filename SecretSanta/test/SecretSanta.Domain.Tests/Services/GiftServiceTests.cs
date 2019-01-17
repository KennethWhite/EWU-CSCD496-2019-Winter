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
                var service = new GiftService(context);
                service.AddOrUpdateGift(giftList[0]);
                service.AddOrUpdateGift(giftList[1]);
            }

            using (var context = new ApplicationDbContext(Options))
            {
                var service = new GiftService(context);
                var fetchedGift = service.Find(2);
                Assert.AreEqual(2, fetchedGift.Id);
                Assert.AreEqual(giftList[1].Title, fetchedGift.Title);
                Assert.AreEqual(giftList[1].Description, fetchedGift.Description);
                Assert.AreEqual(giftList[1].OrderOfImportance, fetchedGift.OrderOfImportance);
                Assert.AreEqual(giftList[1].URL, fetchedGift.URL);
            }
        }

        [TestMethod]
        public void UpdateGift_GiftIsUpdatedInTheDatabase()
        {
            var myGift = CreateGift();

            using (var context = new ApplicationDbContext(Options))
            {
                var service = new GiftService(context);
                service.AddOrUpdateGift(myGift);
            }

            myGift.Title = "New Item";
            myGift.Description = "A super cool gift";

            using (var context = new ApplicationDbContext(Options))
            {
                var service = new GiftService(context);
                service.AddOrUpdateGift(myGift);
            }

            using (var context = new ApplicationDbContext(Options))
            {
                var service = new GiftService(context);
                var fetchedGift = service.Find(1);
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
            var giftsToAdd = CreateFiveGifts();
            using (var context = new ApplicationDbContext(Options))
            {
                var service = new GiftService(context);
                service.AddGifts(giftsToAdd);
            }

            using (var context = new ApplicationDbContext(Options))
            {
                var service = new GiftService(context);
                var fetchedGifts = service.FetchAll();
                for (var i = 0; i < fetchedGifts.Count; i++)
                {
                    var giftToAdd = giftsToAdd[i];
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
                User = new User
                {
                    FirstName = "Bob",
                    LastName = "Ross"
                }
            };
        }

        private static List<Gift> CreateFiveGifts()
        {
            return new List<Gift>
            {
                new Gift
                {
                    Title = "Item 1", Description = "Item 1 description", OrderOfImportance = 1,
                    URL = "https://www.item1.com"
                },
                new Gift
                {
                    Title = "Item 2", Description = "Item 2 description", OrderOfImportance = 2,
                    URL = "https://www.item2.com"
                },
                new Gift
                {
                    Title = "Item 3", Description = "Item 3 description", OrderOfImportance = 3,
                    URL = "https://www.item3.com"
                },
                new Gift
                {
                    Title = "Item 4", Description = "Item 4 description", OrderOfImportance = 4,
                    URL = "https://www.item4.com"
                },
                new Gift
                {
                    Title = "Item 5", Description = "Item 5 description", OrderOfImportance = 5,
                    URL = "https://www.item5.com"
                }
            };
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