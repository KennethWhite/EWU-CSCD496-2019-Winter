using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Api.Controllers;
using SecretSanta.Domain.Models;

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
        public void GetGroups_ReturnsAllGroups()
        {
            var testService = new TestableGroupService
            {
                AllGroups = new List<Group>
                {
                    CreateGroupWithId(1), CreateGroupWithId(2)
                }
            };
            var controller = new GroupController(testService);
            var result = controller.GetAllGroups();
            var groups = result.Value;
            Assert.AreEqual(2, groups.Count);
            Assert.AreEqual(1, groups[0].Id);
        }

        private static Group CreateGroupWithId(int id)
        {
            return new Group
            {
                Id = id,
                Name = $"Group{id}"
            };
        }
    }
}