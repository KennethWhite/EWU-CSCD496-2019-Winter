using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SecretSanta.Api.Controllers;
using SecretSanta.Api.ViewModels;
using SecretSanta.Domain.Models;
using SecretSanta.Domain.Services.Interfaces;

namespace SecretSanta.Api.Tests.Controllers
{
    [TestClass]
    public class PairingControllerTests
    {
        private CustomWebApplicationFactory<Startup> Factory { get; set; }

        [TestInitialize]
        public void CreateWebFactory()
        {
            Factory = new CustomWebApplicationFactory<Startup>();
        }

        [TestMethod]
        public async Task GenerateUserPairings_RequiresPositiveId()
        {
            var service = new Mock<IPairingService>(MockBehavior.Strict);
            var controller = new PairingController(service.Object, Mapper.Instance);
            
            var result = await controller.GenerateUserPairings(-1);
            Assert.IsTrue(result is BadRequestObjectResult);
        }

        [TestMethod]
        public async Task GenerateUserPairings_ReturnsGeneratedPairings()
        {
            var pairings = new List<Pairing>
            {
                new Pairing {Id = 1, RecipientId = 1, SantaId = 2},
                new Pairing {Id = 2, RecipientId = 2, SantaId = 3},
                new Pairing {Id = 3, RecipientId = 3, SantaId = 4}
            };
            var service = new Mock<IPairingService>();
            service.Setup(x => x.GenerateUserPairings(It.IsAny<int>()))
                .ReturnsAsync(pairings)
                .Verifiable();

            var controller = new PairingController(service.Object, Mapper.Instance);
            var result = await controller.GenerateUserPairings(1) as CreatedResult;
            var resultValue = result?.Value as List<PairingViewModel>;
            Assert.IsNotNull(resultValue);
            Assert.AreEqual(3, resultValue.Count);
            Assert.AreEqual(1, resultValue[0].Id);
            Assert.AreEqual(1, resultValue[0].RecipientId);
            Assert.AreEqual(2, resultValue[0].SantaId);
            service.VerifyAll();
        }
    }
}