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
        public void AddUser_UserIsAddedIntoDatabase()
        {
            using (var dbContext = new ApplicationDbContext(Options))
            {
                var service = new UserService(dbContext);
                var userFromDb = service.AddOrUpdateUser(CreateUser());
                Assert.AreNotEqual(0, userFromDb.Id);
            }
        }

        [TestMethod]
        public void FindUser_CreatedUserIsRetrievedFromDatabase()
        {
            using (var context = new ApplicationDbContext(Options))
            {
                var service = new UserService(context);
                service.AddOrUpdateUser(CreateUser());
            }

            using (var context = new ApplicationDbContext(Options))
            {
                var service = new UserService(context);
                var fetchedUser = service.Find(1);
                Assert.AreEqual(1, fetchedUser.Id);
            }
        }

        [TestMethod]
        public void UpdateUser_UserIsUpdatedInTheDatabase()
        {
            var myUser = CreateUser();
            
            using (var context = new ApplicationDbContext(Options))
            {
                var service = new UserService(context);
                service.AddOrUpdateUser(myUser);
            }

            myUser.FirstName = "Steve";
            myUser.LastName = "Irwin";
            
            using (var context = new ApplicationDbContext(Options))
            {
                var service = new UserService(context);
                service.AddOrUpdateUser(myUser);
            }
            
            using (var context = new ApplicationDbContext(Options))
            {
                var service = new UserService(context);
                var fetchedUser = service.Find(1);
                Assert.AreEqual(1, fetchedUser.Id);
                Assert.AreEqual("Steve", fetchedUser.FirstName);
                Assert.AreEqual("Irwin", fetchedUser.LastName);
            }
        }

        [TestMethod]
        public void FetchUsers_CreatedUsersAreRetrievedFromDatabase()
        {
            var usersToAdd = CreateFiveUsers();
            using (var context = new ApplicationDbContext(Options))
            {
                var service = new UserService(context);
                service.AddUsers(usersToAdd);
            }

            using (var context = new ApplicationDbContext(Options))
            {
                var service = new UserService(context);
                var fetchedUsers = service.FetchAll();
                for (var i = 0; i < fetchedUsers.Count; i++)
                {
                    var userToAdd = usersToAdd[i];
                    var userFetched = fetchedUsers[i];
                    Assert.AreEqual(userToAdd.Id, userFetched.Id);
                    Assert.AreEqual(userToAdd.FirstName, userFetched.FirstName);
                    Assert.AreEqual(userToAdd.LastName, userFetched.LastName);
                }
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
        
        
        private static List<User> CreateFiveUsers()
        {
            return new List<User>
            {
                new User {FirstName = "Casey", LastName = "White"},
                new User {FirstName = "Kenny", LastName = "White"},
                new User {FirstName = "Mark", LastName = "Michaelis"},
                new User {FirstName = "Kevin", LastName = "Bost"},
                new User {FirstName = "Michael", LastName = "Stokesbary"}
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