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
        public void GetGiftsForUser_ReturnsUsersFromService()
        {
            var gift = CreateGiftWithId(1);
            var testService = new TestableGiftService {AllGifts = new List<Gift> {gift}};
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
        public void AddGiftForUser_RequiresPositiveUserId()
        {
            var testService = new TestableGiftService();
            var controller = new GiftController(testService);

            var gift = new DTO.Gift(CreateGiftWithId(1));
            ActionResult<DTO.Gift> result = controller.AddGiftForUser(-1, gift);

            Assert.IsTrue(result.Result is NotFoundResult);
            //This check ensures that the service was not called
            Assert.AreEqual(0, testService.GetGiftsForUser_UserId);
        }

        [TestMethod]
        public void AddGiftForUser_RequiresGift()
        {
            var testService = new TestableGiftService();
            var controller = new GiftController(testService);
            ActionResult<DTO.Gift> result = controller.AddGiftForUser(1, null);

            Assert.IsTrue(result.Result is BadRequestResult);
            //This check ensures that the service was not called
            Assert.AreEqual(0, testService.GetGiftsForUser_UserId);
        }

        [TestMethod]
        public void AddGiftForUser_ReturnAddition()
        {
            var testService = new TestableGiftService {AllGifts = new List<Gift>()};
            var controller = new GiftController(testService);
            var gift = CreateGiftWithId(1);
            var result = controller.AddGiftForUser(1, new DTO.Gift(gift));
            var resultGift = result.Value;
            Assert.AreEqual(1, testService.AllGifts.Count);
            Assert.AreEqual(resultGift.Id, testService.AddGiftsForUser_UserId);
            Assert.AreEqual(gift.Id, testService.AddGiftsForUser_UserId);
            Assert.AreEqual(gift.Id, resultGift.Id);
            Assert.AreEqual(gift.Title, resultGift.Title);
        }
        
        
        
        
        [TestMethod]
        public void UpdateGiftForUser_RequiresPositiveUserId()
        {
            var testService = new TestableGiftService();
            var controller = new GiftController(testService);

            var gift = new DTO.Gift(CreateGiftWithId(1));
            ActionResult<DTO.Gift> result = controller.UpdateGiftForUser(-1, gift);

            Assert.IsTrue(result.Result is NotFoundResult);
            //This check ensures that the service was not called
            Assert.AreEqual(0, testService.UpdateGiftsForUser_GiftId);
        }

        [TestMethod]
        public void UpdateGiftForUser_RequiresGift()
        {
            var testService = new TestableGiftService();
            var controller = new GiftController(testService);
            ActionResult<DTO.Gift> result = controller.UpdateGiftForUser(1, null);

            Assert.IsTrue(result.Result is BadRequestResult);
            //This check ensures that the service was not called
            Assert.AreEqual(0, testService.UpdateGiftsForUser_GiftId);
        }

        [TestMethod]
        public void UpdateGiftForUser_ReturnUpdated()
        {
            var testService = new TestableGiftService
            {
                AllGifts = new List<Gift>
                {
                    CreateGiftWithId(1), CreateGiftWithId(2), CreateGiftWithId(3)
                }
            };
            var controller = new GiftController(testService);
            var oldGift = CreateGiftWithId(2);
            var updateGift = CreateGiftWithId(2);
            updateGift.Title = "New Title";
            updateGift.Description = "New Desc";
            var result = controller.UpdateGiftForUser(1, new DTO.Gift(updateGift));
            var resultGift = result.Value;
            Assert.AreEqual(3, testService.AllGifts.Count);
            Assert.AreEqual(oldGift.Id, testService.UpdateGiftsForUser_GiftId);
            Assert.AreEqual(oldGift.Id, resultGift.Id);
            Assert.AreNotEqual(oldGift.Title, resultGift.Title);
        }
        

        [TestMethod]
        public void RemoveGiftForUser_RequiresGift()
        {
            var testService = new TestableGiftService();
            var controller = new GiftController(testService);
            ActionResult<DTO.Gift> result = controller.RemoveGift(null);

            Assert.IsTrue(result.Result is BadRequestResult);
            //This check ensures that the service was not called
            Assert.AreEqual(0, testService.RemoveGifts_GiftId);
        }

        [TestMethod]
        public void RemoveGiftForUser_GiftIsRemoved()
        {
            var testService = new TestableGiftService
            {
                AllGifts = new List<Gift>
                {
                    CreateGiftWithId(1), CreateGiftWithId(2), CreateGiftWithId(3)
                }
            };
            var controller = new GiftController(testService);
            var giftToRemove = new DTO.Gift(testService.AllGifts[1]);
            Assert.AreEqual<int>(3, testService.AllGifts.Count);
            controller.RemoveGift(giftToRemove);
            Assert.AreEqual<int>(2, testService.AllGifts.Count);
            Assert.AreEqual<int>(1, testService.AllGifts[0].Id);
            Assert.AreEqual<int>(3, testService.AllGifts[1].Id);
            Assert.AreEqual(giftToRemove.Id, testService.RemoveGifts_GiftId);
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