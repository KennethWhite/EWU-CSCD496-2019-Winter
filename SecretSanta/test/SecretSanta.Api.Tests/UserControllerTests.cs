using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Controllers;
using SecretSanta.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace SecretSanta.Api.Tests
{
    [TestClass]
    public class UserControllerTests
    {
        private TestableUserService TestService { get; set; }
        private UserController UserController { get; set; }

        [TestInitialize]
        public void InitializeController()
        {
            TestService = new TestableUserService { UsersToReturn = new List<User> {} };
            UserController = new UserController(TestService);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UserController_RequiresUserService()
        {
            new UserController(null);
        }

        [TestMethod]
        public void CreateUser_ReturnsUserFromService_ResultCreated()
        {
            var user = CreateUser(1, "Bob", "Ross");
            ActionResult<DTO.User> result = UserController.CreateUser(new DTO.User(user));
            Assert.AreEqual(1, TestService.LastModifiedUser.Id);

            DTO.User resultUser = result.Value;
            Assert.AreEqual<int>(user.Id, TestService.LastModifiedUser.Id);
            Assert.AreEqual<string>(user.FirstName, TestService.LastModifiedUser.FirstName);
            Assert.AreEqual<string>(user.LastName, TestService.LastModifiedUser.LastName);
        }

        [TestMethod]
        public void CreateUser_UserCannotBeNull_BadRequestReturned()
        {
            ActionResult<DTO.User> result = UserController.CreateUser(null);
            Assert.IsTrue(result.Result is BadRequestResult);
        }

        [TestMethod]
        public void UpdateUser_UserIsModified()
        {
            var user = CreateUser(4, "Bill", "Nye");
            ActionResult<DTO.User> result = UserController.UpdateUser(new DTO.User(user));
            DTO.User resultUser = result.Value; 
            Assert.AreEqual<int>(user.Id, resultUser.Id);
            Assert.AreEqual<string>(user.FirstName, resultUser.FirstName);
            Assert.AreEqual<string>(user.LastName, resultUser.LastName);
        }

        [TestMethod]
        public void UpdateUser_UserCannotBeNull_BadRequestReturned()
        {
            ActionResult<DTO.User> result = UserController.UpdateUser(null);
            Assert.IsTrue(result.Result is BadRequestResult);
        }

        [TestMethod]
        public void DeleteUser_UserIsRemoved ()
        {
            var user = CreateUser(42, "Levar", "Burton");
            ActionResult<DTO.User> result = UserController.DeleteUser(new DTO.User(user));
            DTO.User resultUser = result.Value;
            Assert.AreEqual<int>(user.Id, resultUser.Id);
            Assert.AreEqual<string>(user.FirstName, resultUser.FirstName);
            Assert.AreEqual<string>(user.LastName, resultUser.LastName);
        }

        [TestMethod]
        public void DeleteUser_UserCannotBeNull_BadRequestReturned()
        {
            ActionResult<DTO.User> result = UserController.DeleteUser(null);
            Assert.IsTrue(result.Result is BadRequestResult);
        }



        private User CreateUser(int id, string fName, string lName)
        {
            return new User
            {
                FirstName = fName,
                LastName = lName,
                Id = id,
                Gifts = new List<Gift>(),
                GroupUsers = new List<GroupUser>()
            };
        }
    }
}
