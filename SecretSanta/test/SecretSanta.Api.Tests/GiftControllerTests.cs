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
    public class GiftControllerTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GiftController_RequiresGiftService()
        {
            new GiftController(null);
        }

        [TestMethod]
        public void GetGiftForUser_ReturnsUsersFromService()
        {
            var gift = CreateGiftWithId(1);
            var testService = new TestableGiftService {UserGiftsToReturn =  new List<Gift> {gift}};
            var controller = new GiftController(testService);

            ActionResult<List<DTO.Gift>> result = controller.GetGiftForUser(4);

            Assert.AreEqual(4, testService.GetGiftsForUser_UserId);
            DTO.Gift resultGift = result.Value.Single();
            Assert.AreEqual(gift.Id, resultGift.Id);
            Assert.AreEqual(gift.Title, resultGift.Title);
            Assert.AreEqual(gift.Description, resultGift.Description);
            Assert.AreEqual(gift.Url, resultGift.Url);
            Assert.AreEqual(gift.OrderOfImportance, resultGift.OrderOfImportance);
        }

        [TestMethod]
        public void GetGiftForUser_RequiresPositiveUserId()
        {
            var testService = new TestableGiftService();
            var controller = new GiftController(testService);

            ActionResult<List<DTO.Gift>> result = controller.GetGiftForUser(-1);
            
            Assert.IsTrue(result.Result is NotFoundResult);
            //This check ensures that the service was not called
            Assert.AreEqual(0, testService.GetGiftsForUser_UserId);
        }

        [TestMethod]
        public void AddGiftForUser_RequiresGift()
        {
            var testService = new TestableGiftService();
            var controller = new GiftController(testService);
            var result = controller.AddGiftForUser(5, null);
            Assert.IsTrue(result is BadRequestResult);
            
        }

        [TestMethod]
        public void AddGiftForUser_ReturnAdditionIsTrue()
        {
            var testService = new TestableGiftService{UserGiftsToReturn =  new List<Gift>()};
            var controller = new GiftController(testService);
            var gift = CreateGiftWithId(1);
            var result = controller.AddGiftForUser(1, new DTO.Gift(gift));
            var resultGift = result.Value;
            Assert.AreEqual(1, testService.UserGiftsToReturn.Count);
            Assert.AreEqual(gift.Id, resultGift.Id);
            Assert.AreEqual(gift.Title, resultGift.Title);
        }

        [TestMethod]
        public void RemoveGiftForUser_GiftIsRemoved()
        {
            var testService = new TestableGiftService{UserGiftsToReturn =  new List<Gift>
            {
                CreateGiftWithId(1), CreateGiftWithId(2), CreateGiftWithId(3)
            }};
            var controller = new GiftController(testService);

            var giftToRemove = testService.UserGiftsToReturn[1];
            Assert.AreEqual<int>(3, testService.UserGiftsToReturn.Count);
            testService.RemoveGift(giftToRemove);
            Assert.AreEqual<int>(2, testService.UserGiftsToReturn.Count);
            Assert.AreEqual<int>(1, testService.UserGiftsToReturn[0].Id);
            Assert.AreEqual<int>(3, testService.UserGiftsToReturn[1].Id);
        }

        private Gift CreateGiftWithId(int id)
        {
            return new Gift
            {
                Id = id,
                Title = $"Gift Title {id}",
                Description = $"Gift Description {id}",
                Url = $"http://www.gift{id}.url",
                OrderOfImportance = id
            };
        }
    }
}
