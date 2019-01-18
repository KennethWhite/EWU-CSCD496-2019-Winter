using System;
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
    public class MessageServiceTests
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
        public void AddMessage_MessageIsAddedIntoDatabase()
        {
            using (var dbContext = new ApplicationDbContext(Options))
            {
                var service = new MessageService(dbContext);
                var messageFromDb = service.AddOrUpdateMessage(CreateMessage());
                Assert.AreNotEqual(0, messageFromDb.Id);
            }
        }

        [TestMethod]
        public void FindMessage_CreatedMessageIsRetrievedFromDatabase()
        {
            using (var context = new ApplicationDbContext(Options))
            {
                var service = new MessageService(context);
                var message = CreateMessage();
                service.AddOrUpdateMessage(message);
            }

            using (var context = new ApplicationDbContext(Options))
            {
                var service = new MessageService(context);
                var messageFromDb = service.Find(1);
                Assert.AreEqual(1, messageFromDb.Id);
            }
        }

        [TestMethod]
        public void FindMessageID2_CreatedMesssageIsRetrievedFromDatabase()
        {
            var messageList = CreateNMessages(3);
            using (var context = new ApplicationDbContext(Options))
            {
                new MessageService(context).AddMessages(messageList);
            }

            using (var context = new ApplicationDbContext(Options))
            {
                var messageFromDb = new MessageService(context).Find(2);
                Assert.AreEqual(2, messageFromDb.Id);
                Assert.AreEqual(messageList[1].Pairing.Santa.FirstName, messageFromDb.Pairing.Santa.FirstName);
                Assert.AreEqual(messageList[1].Pairing.Recipient.FirstName, messageFromDb.Pairing.Recipient.FirstName);
            }
        }



        [TestMethod]
        public void UpdateMessage_MessageIsUpdatedInTheDatabase()
        {
            var message = CreateMessage();
            using (var context = new ApplicationDbContext(Options))
            {
                new MessageService(context).AddOrUpdateMessage(message);
            }

            message.Pairing.Recipient.FirstName = "Inigo";
            message.Pairing.Recipient.LastName = "Montoya";

            using (var context = new ApplicationDbContext(Options))
            {
                new MessageService(context).AddOrUpdateMessage(message);
            }

            using (var context = new ApplicationDbContext(Options))
            {
                var messageFromDb = new MessageService(context).Find(1);
                Assert.AreEqual(1, messageFromDb.Id);
                Assert.AreEqual("Inigo", messageFromDb.Pairing.Recipient.FirstName);
                Assert.AreEqual("Montoya", messageFromDb.Pairing.Recipient.LastName);
            }
        }

        [TestMethod]
        public void RemoveMessage_MessageIsRemovedFromDatabase()
        {
            var messages = CreateNMessages(5);
            using (var context = new ApplicationDbContext(Options))
            {
                var messageFromDb = new MessageService(context).AddMessages(messages);
            }

            int numberRemoved = 0;
            foreach (Message m in messages)
            {
                using (var context = new ApplicationDbContext(Options))
                {
                    new MessageService(context).RemoveMessage(m);
                    numberRemoved++;
                    Assert.AreEqual<int>(messages.Count - numberRemoved, new MessageService(context).FetchAll().Count);
                }

            }

        }

        [TestMethod]
        public void FetchMessages_CreatedMessagesAreRetrievedFromDatabase()
        {
            using (var context = new ApplicationDbContext(Options))
            {
                new MessageService(context).AddMessages(CreateNMessages(5));
            }

            using (var context = new ApplicationDbContext(Options))
            {
                var initialMessages = CreateNMessages(5);
                var fetchedMessages = new MessageService(context).FetchAll();
                for (var i = 0; i < fetchedMessages.Count; i++)
                {
                    var messageToAdd = initialMessages[i];
                    var messageFetched = fetchedMessages[i];
                    Assert.AreEqual(messageToAdd.Pairing.Santa.FirstName, messageFetched.Pairing.Santa.FirstName);
                    Assert.AreEqual(messageToAdd.Pairing.Recipient.FirstName, messageFetched.Pairing.Recipient.FirstName);
                }
            }
        }

        private static Message CreateMessage()
        {
            return new Message
            {
                Pairing = new Pairing
                {
                    Santa = new User
                    {
                        FirstName = "Kris",
                        LastName = "Kringle"
                    },
                    Recipient = new User
                    {
                        FirstName = "Dennis",
                        LastName = "Menace"
                    }
                }
            };
        }

        private static List<Message> CreateNMessages(int n)
        {
            var messages = new List<Message>();
            for (var i = 1; i < n; i++)
            {
                messages.Add(new Message
                {
                    Pairing = new Pairing
                    {
                        Santa = new User
                        {
                            FirstName = $"Kris the {i}th",
                            LastName = "Kringle"
                        },
                        Recipient = new User
                        {
                            FirstName = $"Dennis the {i}th",
                            LastName = "Menace"
                        }
                    }
                });
            }
            return messages;
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