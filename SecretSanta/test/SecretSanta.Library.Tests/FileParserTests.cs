using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SecretSanta.Library.Tests
{
    [TestClass]
    public class FileParserTests
    {
        private List<string> TempFiles { get; } = new List<string>();

        [TestInitialize, TestCleanup]
        public void CleanupTestFile()
        {
            TempFiles.ForEach(File.Delete);
            TempFiles.Clear();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        [DataRow("Name: Louis George Maurice Adolphe Roche Albert Abel Antonio")]
        [DataRow("")]
        public void ParseFile_InvalidHeaderDeclaration_ThrowsDataException(string header)
        {
            var fileHandle = CreateTempFile_WriteDataToFile(new List<string> {header});
            var user = FileParser.ParseWishlistFile(fileHandle);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ParseFile_InvalidHeaderTooManyNamesLineFive_ThrowsDataException()
        {
            var data = new List<string>{"","","","","Name: Louis George Maurice Adolphe Roche Albert Abel Antonio"};
            var fileHandle = CreateTempFile_WriteDataToFile(data);
            var user = FileParser.ParseWishlistFile(fileHandle);
        }


        [TestMethod]
        public void ParseWishlistFile_ValidHeaderLineOne_Success()
        {
            var fileHandle = CreateTempFile_WriteDataToFile(new List<string> {"Name: Issac Newton"});
            var user = FileParser.ParseWishlistFile(fileHandle);
            Assert.AreEqual<string>("Issac", user.FirstName);
            Assert.AreEqual<string>("Newton", user.LastName);
        }

        [TestMethod]
        public void ParseWishlistFile_ValidHeaderLineFive_Success()
        {
            var data = new List<string>{"","","","","Name: Tesla, Nikola"};
            var fileHandle = CreateTempFile_WriteDataToFile(data);
            var user = FileParser.ParseWishlistFile(fileHandle);
            Assert.AreEqual<string>("Nikola", user.FirstName);
            Assert.AreEqual<string>("Tesla", user.LastName);
        }

        [TestMethod]
        public void ParseWishlistFile_ValidHeaderLineOneReversedNames_Success()
        {
            var fileHandle = CreateTempFile_WriteDataToFile(new List<string> {"Name: Newton, Issac"});
            var user = FileParser.ParseWishlistFile(fileHandle);
            Assert.AreEqual<string>("Issac", user.FirstName);
            Assert.AreEqual<string>("Newton", user.LastName);
        }

        [TestMethod]
        public void ParseWishlistFile_ValidHeaderLineFiveReversedNames_Success()
        {
            var data = new List<string>{"","","","","Name: Tesla, Nikola"};
            var fileHandle = CreateTempFile_WriteDataToFile(data);
            var user = FileParser.ParseWishlistFile(fileHandle);
            Assert.AreEqual<string>("Nikola", user.FirstName);
            Assert.AreEqual<string>("Tesla", user.LastName);
        }

        [TestMethod]
        [DataRow("My Two Front Teeth")]
        public void ParseWishlistFile_ContainsOneGift_Success(string giftDescription)
        {
            var data = new List<string> {"Name: Tesla, Nikola" + Environment.NewLine, giftDescription};
            var fileHandle = CreateTempFile_WriteDataToFile(data);
            var user = FileParser.ParseWishlistFile(fileHandle);
            var gifts = user.Wishlist;
            Assert.AreEqual<string>(gifts[0].Description, giftDescription);
        }

        [TestMethod]
        [DataRow(new[] {"My Two Front Teeth", "Hot Wheels Cars", "Water Bottle"})]
        [DataRow(new[] {"C# Essentials", "The Pragmatic Programmer", "Mad Coding Skills"})]
        public void ParseWishlistFile_ContainsMultipleGifts_Success(string[] giftDescriptions)
        {
            var data = new List<string> {"Name: Tesla, Nikola" + Environment.NewLine};
            data.AddRange(giftDescriptions.Select(desc => desc + Environment.NewLine));

            var fileHandle = CreateTempFile_WriteDataToFile(data);
            var user = FileParser.ParseWishlistFile(fileHandle);
            var gifts = user.Wishlist;
            for (var index = 0; index < gifts.Count; index++)
            {
                Assert.AreEqual<string>(gifts[index].Description, giftDescriptions[index]);
            }
        }

        [TestMethod]
        [DataRow(new[] {"My Two Front Teeth", "", "Water Bottle"})]
        [DataRow(new[] {"C# Essentials", "             ", "Mad Coding Skills"})]
        public void ParseWishlistFile_ContainsBlankLines_BlankLinesIgnored(string[] giftDescriptions)
        {
            var data = new List<string> {"Name: Tesla, Nikola" + Environment.NewLine};
            data.AddRange(giftDescriptions.Select(desc => desc + Environment.NewLine));

            var fileHandle = CreateTempFile_WriteDataToFile(data);
            var user = FileParser.ParseWishlistFile(fileHandle);
            var gifts = user.Wishlist;

            foreach (var gift in gifts)
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(gift.Description)); //ensuring blank lines did not make it into wishlist
                Assert.IsTrue(giftDescriptions.Contains(gift.Description));
            }
        }


        private string CreateTempFile_WriteDataToFile(List<string> data)
        {
            string tempFile = Path.GetTempFileName();
            TempFiles.Add(tempFile);

            using (StreamWriter fout = new StreamWriter(tempFile))
            {
                data.ForEach(line => fout?.WriteLine(line));
            }

            return tempFile;
        }
    }
}