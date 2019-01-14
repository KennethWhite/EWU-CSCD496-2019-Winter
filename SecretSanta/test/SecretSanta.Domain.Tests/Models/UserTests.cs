using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Domain;


namespace SecretSanta.Domain.Tests
{
    [TestClass]
    public class UserTests
    {
        [TestMethod]
        public void CreateUser()
        {
            User user = new User{FirstName = "Inigo", LastName = "Montota"};
            Assert.AreEqual("Inigo", user.FirstName);
            Assert.AreEqual("Montota", user.LastName);

        }
    }
}
