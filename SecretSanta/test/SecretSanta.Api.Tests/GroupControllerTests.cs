using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Controllers;
using SecretSanta.Api.DTO;
using SecretSanta.Domain.Models;
using Group = SecretSanta.Domain.Models.Group;
using User = SecretSanta.Domain.Models.User;

namespace SecretSanta.Api.Tests
{
    [TestClass]
    public class GroupControllerTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GroupController_RequiredGiftService()
        {
            new GroupController(null);
        }

        [TestMethod]
        public void AddGroup_RequiresGroup()
        {
            var testService = new TestableGroupService();
            var controller = new GroupController(testService);
            var result = controller.AddGroup(null);
            Assert.IsTrue(result.Result is BadRequestResult);
            //This check ensures that the service was not called
            Assert.IsNull(testService.LastGroupModified);
        }

        [TestMethod]
        public void AddGroup_GroupAdded()
        {
            var groupBeforeAdd = CreateGroupWithId(5);
            var testService = new TestableGroupService{GroupsToReturn = new List<Group>{groupBeforeAdd}};
            var controller = new GroupController(testService);
            var groupAfterAdd = controller.AddGroup(new DTO.Group(groupBeforeAdd)).Value;
            Assert.AreEqual(groupBeforeAdd.Id, groupAfterAdd.Id);
            Assert.AreEqual(groupBeforeAdd.Name, groupAfterAdd.Name);
            Assert.AreEqual(5, testService.LastGroupModified.Id);
        }
        
        [TestMethod]
        public void UpdateGroup_RequiresGroup()
        {
            var testService = new TestableGroupService();
            var controller = new GroupController(testService);
            var result = controller.UpdateGroup(null);
            Assert.IsTrue(result.Result is BadRequestResult);
            //This check ensures that the service was not called
            Assert.IsNull(testService.LastGroupModified);
        }

        [TestMethod]
        public void UpdateGroup_UpdatesGroup()
        {
            var groupBeforeUpdate = CreateGroupWithId(5);
            var testService = new TestableGroupService{GroupsToReturn = new List<Group>{groupBeforeUpdate}};
            var controller = new GroupController(testService);
            var groupAfterUpdate = controller.UpdateGroup(new DTO.Group(groupBeforeUpdate)).Value;
            Assert.AreEqual(groupBeforeUpdate.Id, groupAfterUpdate.Id);
            Assert.AreEqual(groupBeforeUpdate.Name, groupAfterUpdate.Name);
            Assert.AreEqual(5, testService.LastGroupModified.Id);
        }
        
        [TestMethod]
        public void RemoveGroup_RequiresGroup()
        {
            var testService = new TestableGroupService();
            var controller = new GroupController(testService);
            var result = controller.UpdateGroup(null);
            Assert.IsTrue(result.Result is BadRequestResult);
            //This check ensures that the service was not called
            Assert.IsNull(testService.LastGroupModified);
        }

        [TestMethod]
        public void RemoveGroup_GroupRemoved()
        {
            var groupBeforeRemove = CreateGroupWithId(5);
            var testService = new TestableGroupService{GroupsToReturn = new List<Group>{groupBeforeRemove}};
            var controller = new GroupController(testService);
            var groupAfterRemove = controller.RemoveGroup(new DTO.Group(groupBeforeRemove)).Value;
            Assert.AreEqual(groupBeforeRemove.Id, groupAfterRemove.Id);
            Assert.AreEqual(groupBeforeRemove.Name, groupAfterRemove.Name);
            Assert.AreEqual(5, testService.LastGroupModified.Id);
        }

        [TestMethod]
        public void AddUserToGroup_RequiresUser()
        {
            var testService = new TestableGroupService();
            var controller = new GroupController(testService);
            var result = controller.AddUserToGroup(4, null);
            Assert.IsTrue(result.Result is BadRequestResult);
            //This check ensures that the service was not called
            Assert.AreEqual(0, testService.LastGroupIDModified);
            Assert.IsNull(testService.LastUserModified);
        }
        
        [TestMethod]
        public void AddUserToGroup_GroupIdPositive()
        {
            var testService = new TestableGroupService();
            var controller = new GroupController(testService);
            var result = controller.AddUserToGroup(-1, new DTO.User());
            Assert.IsTrue(result.Result is NotFoundResult);
            //This check ensures that the service was not called
            Assert.AreEqual(0, testService.LastGroupIDModified);
            Assert.IsNull(testService.LastUserModified);
        }

        [TestMethod]
        public void AddUserToGroup_UserAddedToGroup()
        {
            var userBeforeAdd = CreateUserWithId(1);
            var testService = new TestableGroupService();
            var controller = new GroupController(testService);
            var userAfterAdd = controller.AddUserToGroup(5, new DTO.User(userBeforeAdd)).Value;
            Assert.AreEqual(userBeforeAdd.Id, userAfterAdd.Id);
            Assert.AreEqual(userBeforeAdd.FirstName, userAfterAdd.FirstName);
            Assert.AreEqual(userBeforeAdd.LastName, userAfterAdd.LastName);
            Assert.AreEqual(5, testService.LastGroupIDModified);
            Assert.AreEqual(1, testService.LastUserModified.Id);
        } 
        
        [TestMethod]
        public void RemoveUserFromGroup_RequiresUser()
        {
            var testService = new TestableGroupService();
            var controller = new GroupController(testService);
            var result = controller.RemoveUserFromGroup(4, null);
            Assert.IsTrue(result.Result is BadRequestResult);
            //This check ensures that the service was not called
            Assert.AreEqual(0, testService.LastGroupIDModified);
            Assert.IsNull(testService.LastUserModified);
        }
        
        [TestMethod]
        public void RemoveUserFromGroup_GroupIdPositive()
        {
            var testService = new TestableGroupService();
            var controller = new GroupController(testService);
            var result = controller.RemoveUserFromGroup(-1, new DTO.User());
            Assert.IsTrue(result.Result is NotFoundResult);
            //This check ensures that the service was not called
            Assert.AreEqual(0, testService.LastGroupIDModified);
            Assert.IsNull(testService.LastUserModified);
        }

        [TestMethod]
        public void RemoveUserFromGroup_UserRemovedFromGroup()
        {
            var userBeforeRemove = CreateUserWithId(1);
            var testService = new TestableGroupService();
            var controller = new GroupController(testService);
            var userAfterRemove = controller.RemoveUserFromGroup(5, new DTO.User(userBeforeRemove)).Value;
            Assert.AreEqual(userBeforeRemove.Id, userAfterRemove.Id);
            Assert.AreEqual(userBeforeRemove.FirstName, userAfterRemove.FirstName);
            Assert.AreEqual(userBeforeRemove.LastName, userAfterRemove.LastName);
            Assert.AreEqual(5, testService.LastGroupIDModified);
            Assert.AreEqual(1, testService.LastUserModified.Id);
        } 
        
        [TestMethod]
        public void FetchAllUsersFromGroup_GroupIdPositive()
        {
            var testService = new TestableGroupService();
            var controller = new GroupController(testService);
            var result = controller.FetchAllUsersInGroup(-1);
            Assert.IsTrue(result.Result is NotFoundResult);
            //This check ensures that the service was not called
            Assert.AreEqual(0, testService.LastGroupIDModified);
            Assert.IsNull(testService.LastUserModified);
        }

        [TestMethod]
        public void FetchAllUsersFromGroup_UsersAreFetched()
        {
            var testService = new TestableGroupService
            {
                UsersToReturn = new List<User> {new User {Id = 5, FirstName = "Bob", LastName = "Ross"}}
            };
            var controller = new GroupController(testService);
            var usersReturned = controller.FetchAllUsersInGroup(5).Value;
            Assert.AreEqual(1, usersReturned.Count);
            Assert.AreEqual(5, usersReturned[0].Id);
            Assert.AreEqual("Bob", usersReturned[0].FirstName);
            Assert.AreEqual("Ross", usersReturned[0].LastName);
            Assert.AreEqual(5, testService.LastGroupIDModified);

        }
       
        [TestMethod]
        public void FetchAllGroups_ReturnsAllGroups()
        {
            var testService = new TestableGroupService
            {
                GroupsToReturn = new List<Group>{CreateGroupWithId(1), CreateGroupWithId(2)}
            };
            var controller = new GroupController(testService);
            var result = controller.GetAllGroups();
            var groups = result.Value;
            Assert.AreEqual(2, groups.Count);
            Assert.AreEqual(1, groups[0].Id);
        }
        
        private static User CreateUserWithId(int id, List<GroupUser> groupUsers = null)
        {
            return new User
            {
                Id = id,
                FirstName = $"FirstName{id}",
                LastName = $"LastName{id}"
            };
        }

        private static Group CreateGroupWithId(int id, List<GroupUser> groupUsers = null)
        {
            return new Group
            {
                Id = id,
                Name = $"Group{id}"
            };
        }
    }
}