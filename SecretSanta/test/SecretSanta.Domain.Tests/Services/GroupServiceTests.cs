using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SecretSanta.Domain.Tests.Services
{
    [TestClass]
    public class GroupServiceTests : DatabaseServiceTests    //todo Test additional GroupService methods
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GroupService_RequiresDbContext()
        {
            new GroupService(null);
        }

        [TestMethod]
        public void AddGroup_PersistsGroup()
        {
            var group = new Group
            {
                Name = "Test Group"
            };
            using (var context = new ApplicationDbContext(Options))
            {
                var service = new GroupService(context);

                Group addedGroup = service.AddGroup(group);
                Assert.AreEqual(addedGroup, group);
                Assert.AreNotEqual(0, addedGroup.Id);
            }

            using (var context = new ApplicationDbContext(Options))
            {
                Group retrievedGroup = context.Groups.Single();
                Assert.AreEqual(group.Name, retrievedGroup.Name);
            }
        }

        [TestMethod]
        public void UpdateGroup_UpdatesExistingGroup()
        {
            var group = new Group
            {
                Name = "Test Group"
            };
            using (var context = new ApplicationDbContext(Options))
            {
                context.Groups.Add(group);
                context.SaveChanges();
            }

            group.Name = "Updated Name";
            using (var context = new ApplicationDbContext(Options))
            {
                var service = new GroupService(context);
                Group updatedGroup = service.UpdateGroup(group);
                Assert.AreEqual(group, updatedGroup);
            }

            using (var context = new ApplicationDbContext(Options))
            {
                Group retrievedGroup = context.Groups.Single();
                Assert.AreEqual(group.Id, retrievedGroup.Id);
                Assert.AreEqual(group.Name, retrievedGroup.Name);
            }
        }

        [TestMethod]
        public void GetUsers_ReturnsUserInGroup()
        {
            var user = new User { Id = 42 };
            var group = new Group { Id = 43 };
            var groupUser = new GroupUser { GroupId = group.Id, UserId = user.Id };
            group.GroupUsers = new List<GroupUser> { groupUser };

            using (var context = new ApplicationDbContext(Options))
            {
                context.Users.Add(user);
                context.Groups.Add(group);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(Options))
            {
                var service = new GroupService(context);
                List<User> users = service.GetUsers(43);
                Assert.AreEqual(42, users.Single().Id);
            }
        }

        [TestMethod]
        public void GetUsers_ReturnsEmptySetWhenGroupIsNotFound()
        {
            using (var context = new ApplicationDbContext(Options))
            {
                var service = new GroupService(context);
                List<User> users = service.GetUsers(4);
                Assert.AreEqual(0, users.Count);
            }
        }

        [TestMethod]
        public void RemoveGroup_ValidGroup_GroupIsRemoved()
        {
            var group = new Group
            {
                Name = "Test Group"
            };
            using (var context = new ApplicationDbContext(Options))
            {
                context.Groups.Add(group);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(Options))
            {
                var service = new GroupService(context);
                Group removedGroup = service.RemoveGroup(group);
                Assert.AreEqual<Group>(group, removedGroup);
            }

            using (var context = new ApplicationDbContext(Options))
            {
                Assert.IsFalse(context.Groups.Contains(group));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveGroup_GroupIsNull_ThrowsException()
        {
            var group = new Group
            {
                Name = "Test Group"
            };
            using (var context = new ApplicationDbContext(Options))
            {
                context.Groups.Add(group);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(Options))
            {
                var service = new GroupService(context);
                Group removedGroup = service.RemoveGroup(null);
            }
        }

        [TestMethod]
        public void AddUserToGroup_ValidUser_UserIsAdded()
        {
            var group = new Group
            {
                Name = "Test Group"
            };
            var user = new User
            {
                FirstName = "Wyatt",
                LastName = "Earp"
            };
            using (var context = new ApplicationDbContext(Options))
            {
                context.Groups.Add(group);
                context.Users.Add(user);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(Options))
            {
                var service = new GroupService(context);
                User addedUser = service.AddUserToGroup(1, user);
                Assert.AreEqual<User>(addedUser, user);
            }

            using (var context = new ApplicationDbContext(Options))
            {
                var userFound = context.Users.Include(g => g.GroupUsers).SingleOrDefault(u => u.FirstName == user.FirstName);
                var groupUser = userFound.GroupUsers.SingleOrDefault();
                Assert.AreEqual<int>(group.Id, groupUser.GroupId);
                Assert.AreEqual<int>(user.Id, groupUser.UserId);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddUserToGroup_UserIsNull_ThrowsException()
        {
            var group = new Group
            {
                Name = "Test Group"
            };

            using (var context = new ApplicationDbContext(Options))
            {
                context.Groups.Add(group);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(Options))
            {
                var service = new GroupService(context);
                User addedUser = service.AddUserToGroup(1, null);
            }
        }

        [TestMethod]
        public void RemoveUserFromGroup_UserIsRemoved()
        {
            var group = new Group
            {
                Name = "Test Group"
            };
            var user = new User
            {
                FirstName = "Wyatt",
                LastName = "Earp"
            };
            using (var context = new ApplicationDbContext(Options))
            {
                context.Groups.Add(group);
                context.Users.Add(user);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(Options))
            {
                var service = new GroupService(context);
                User addedUser = service.AddUserToGroup(1, user);
                Assert.AreEqual<User>(addedUser, user);
            }

            using (var context = new ApplicationDbContext(Options))
            {
                var service = new GroupService(context);
                User removedUser = service.RemoveUserFromGroup(1, user);
                Assert.AreEqual<User>(removedUser, user);
            }
            using(var context = new ApplicationDbContext(Options))
            {
                var service = new GroupService(context);
                Assert.IsTrue(service.FetchAllUsersInGroup(1).Count == 0);
            }


        }


        [TestMethod]
        public void FetchAllUsersInGroup_ValidID_UsersAreReturned ()
        {
            var group = new Group
            {
                Name = "Test Group"
            };
            var cowboy = new User
            {
                FirstName = "Wyatt",
                LastName = "Earp"
            };
            var alien = new User
            {
                FirstName = "Extra",
                LastName = "Terrestrial"
            };

            using (var context = new ApplicationDbContext(Options))
            {
                context.Groups.Add(group);
                context.Users.Add(cowboy);
                context.Users.Add(alien);
                context.SaveChanges();
            }
            
            using (var context = new ApplicationDbContext(Options))
            {
                var service = new GroupService(context);
                service.AddUserToGroup(1, cowboy);
                service.AddUserToGroup(1, alien);
            }
            using (var context = new ApplicationDbContext(Options))
            {
                var service = new GroupService(context);
                var users = service.FetchAllUsersInGroup(1);
                Assert.AreEqual<string>(users[0].FirstName, cowboy.FirstName);
                Assert.AreEqual<string>(users[1].FirstName, alien.FirstName);
            }
        }

        [TestMethod]
        public void FetchAllGroups()
        {
            var group = new Group
            {
                Name = "Test Group"
            };

            var groupTwo = new Group
            {
                Name = "Avengers"
            };

            using (var context = new ApplicationDbContext(Options))
            {
                context.Groups.Add(group);
                context.Groups.Add(groupTwo);
                context.SaveChanges();
            }
            using (var context = new ApplicationDbContext(Options))
            {
                var service = new GroupService(context);
                var groups = service.FetchAllGroups();
                Assert.AreEqual<int>(2, groups.Count);
                Assert.AreEqual<string>(group.Name, groups[0].Name);
                Assert.AreEqual<string>(groupTwo.Name, groups[1].Name);
            }
        }

    }
}