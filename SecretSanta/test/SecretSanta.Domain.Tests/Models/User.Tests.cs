using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Domain.Models;

namespace SecretSanta.Domain.Tests.Models
{
    [TestClass]
    public class UserTests
    {
        [TestMethod]
        public void CreateUser()
        {
            var user = new User {FirstName = "Inigo", LastName = "Montoya"};
            Assert.AreEqual<string>("Inigo", user.FirstName);
            Assert.AreEqual<string>("Montoya", user.LastName);
        }
    }
}