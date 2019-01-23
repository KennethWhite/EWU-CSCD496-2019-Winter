using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SecretSanta.Library.Tests
{
    [TestClass]
    public class FileParserTests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ParseFile_InvalidHeaderDeclaration_ThrowsDataException()
        {
            var user = FileParser.ParseWishlistFile("invalidHeader.txt");
        }
        
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ParseFile_InvalidHeaderTooManyNames_ThrowsDataException()
        {
            var user = FileParser.ParseWishlistFile("InvalidHeaderTooManyNames.txt");
        }
        
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ParseFile_InvalidHeaderTooManyNamesLineFive_ThrowsDataException()
        {
            var user = FileParser.ParseWishlistFile("InvalidHeaderTooManyNamesLine5.txt");
        }
        
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ParseFile_InvalidHeaderNoHeader_ThrowsDataException()
        {
            var user = FileParser.ParseWishlistFile("EmptyFile.txt");
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