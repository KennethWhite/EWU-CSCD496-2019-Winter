using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services;

namespace SecretSanta.Domain.Tests.Services
{
    [TestClass]
    public class UserServiceTests
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
        public void AddUser_UserIsAddedIntoDatabase()
        {
            using (var dbContext = new ApplicationDbContext(Options))
            {
                var service = new UserService(dbContext);
                var user = CreateUser();
                var userFromDb = service.AddOrUpdateUser(user);
                Assert.AreNotEqual(0, userFromDb.Id);
            }
        }

        [TestMethod]
        public void FindUser_CreatedUserIsRetrievedFromDatabase()
        {
            using (var context = new ApplicationDbContext(Options))
            {
                var service = new UserService(context);
                var myUser = CreateUser();
                var persistedUser = service.AddOrUpdateUser(myUser);
            }

            using (var context = new ApplicationDbContext(Options))
            {
                var service = new UserService(context);
                var fetchedUser = service.Find(1);
                Assert.AreEqual(1, fetchedUser.Id);
            }
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