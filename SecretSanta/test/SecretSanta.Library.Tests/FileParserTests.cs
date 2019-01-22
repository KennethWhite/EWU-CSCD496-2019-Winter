using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SecretSanta.Library.Tests
{
    [TestClass]
    public class FileParserTests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ParseFile_InvalidHeader_Data()
        {
            var user = FileParser.ParseWishlistFile("invalidHeader.txt");
        }

        [TestMethod]
        public void ParseWishlistFile_ValidHeaderLineOne_Success()
        {
            var user = FileParser.ParseWishlistFile("ValidHeaderLine1.txt");
            Assert.AreEqual("Issac", user.FirstName);
            Assert.AreEqual("Newton", user.LastName);
        }
        
        [TestMethod]
        public void ParseWishlistFile_ValidHeaderLineFive_Success()
        {
            var user = FileParser.ParseWishlistFile("ValidHeaderLine5.txt");
            Assert.AreEqual("Nikola", user.FirstName);
            Assert.AreEqual("Tesla", user.LastName);
        }
        
        [TestMethod]
        public void ParseWishlistFile_ValidHeaderLineOneReversedNames_Success()
        {
            var user = FileParser.ParseWishlistFile("ValidHeaderLine1Rev.txt");
            Assert.AreEqual("Issac", user.FirstName);
            Assert.AreEqual("Newton", user.LastName);
        }
        
        [TestMethod]
        public void ParseWishlistFile_ValidHeaderLineFiveReversedNames_Success()
        {
            var user = FileParser.ParseWishlistFile("ValidHeaderLine5Rev.txt");
            Assert.AreEqual("Nikola", user.FirstName);
            Assert.AreEqual("Tesla", user.LastName);
        }
    }
}