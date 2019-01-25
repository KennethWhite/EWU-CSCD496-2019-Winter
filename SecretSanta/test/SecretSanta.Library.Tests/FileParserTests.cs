using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Domain.Models;

namespace SecretSanta.Library.Tests
{
    [TestClass]
    public class FileParserTests
    {
        private List<string> TempFiles { get; } = new List<string>();

        [TestInitialize]
        [TestCleanup]
        public void CleanupTestFile()
        {
            foreach (string filePath in TempFiles)
            {
                File.Delete(filePath);
            }
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
            var fileHandle = CreateTempFile_WriteDataToFile(new List<string> { @"



Name: Louis George Maurice Adolphe Roche Albert Abel Antonio" });
            var user = FileParser.ParseWishlistFile(fileHandle);
        }
        

        [TestMethod]
        public void ParseWishlistFile_ValidHeaderLineOne_Success()
        {
            var fileHandle = CreateTempFile_WriteDataToFile(new List<string> { "Name: Issac Newton" });
            var user = FileParser.ParseWishlistFile(fileHandle);
            Assert.AreEqual("Issac", user.FirstName);
            Assert.AreEqual("Newton", user.LastName);
        }
        
        [TestMethod]
        public void ParseWishlistFile_ValidHeaderLineFive_Success()
        {
            var fileHandle = CreateTempFile_WriteDataToFile(new List<string> { @"



Name: Nikola Tesla
" });
            var user = FileParser.ParseWishlistFile(fileHandle);
            Assert.AreEqual("Nikola", user.FirstName);
            Assert.AreEqual("Tesla", user.LastName);
        }
        
        [TestMethod]
        public void ParseWishlistFile_ValidHeaderLineOneReversedNames_Success()
        {

            var fileHandle = CreateTempFile_WriteDataToFile(new List<string>() { "Name: Newton, Issac" });
            var user = FileParser.ParseWishlistFile(fileHandle);
            Assert.AreEqual("Issac", user.FirstName);
            Assert.AreEqual("Newton", user.LastName);
        }
        
        [TestMethod]
        public void ParseWishlistFile_ValidHeaderLineFiveReversedNames_Success()
        {
            var fileHandle = CreateTempFile_WriteDataToFile(new List<string> { @"



Name: Tesla, Nikola" });
            var user = FileParser.ParseWishlistFile(fileHandle);
            Assert.AreEqual("Nikola", user.FirstName);
            Assert.AreEqual("Tesla", user.LastName);
        }

        [TestMethod]
        [DataRow("My Two Front Teeth")]
        public void ParseWishlistFile_ContainsOneGift_Success(string giftDescription)
        {
            var fileHandle = CreateTempFile_WriteDataToFile(new List<string>() { $"Name: Tesla, Nikola"+ Environment.NewLine
            + $"{giftDescription}"});
            var user = FileParser.ParseWishlistFile(fileHandle);
            var gifts = user.Wishlist;
            Assert.AreEqual<string>(gifts[0].Description, giftDescription);
        }

        [TestMethod]
        [DataRow(new string[] { "My Two Front Teeth", "Hot Wheels Cars", "Water Bottle" })]
        [DataRow(new string[] { "C# Essentials", "The Pragmatic Programmer", "Mad Coding Skills" })]
        public void ParseWishlistFile_ContainsMultipleGifts_Success(string[] giftDescriptions)
        {
            var data = new List<string>() { $"Name: Tesla, Nikola" + Environment.NewLine };
            foreach (string desc in giftDescriptions)
            {
                data.Add(desc + Environment.NewLine);
            }
            var fileHandle = CreateTempFile_WriteDataToFile(data);
            var user = FileParser.ParseWishlistFile(fileHandle);
            var gifts = user.Wishlist;
            for (int index = 0; index < gifts.Count; index++)
            {
                Assert.AreEqual<string>(gifts[index].Description, giftDescriptions[index]);
            }
        }

        [TestMethod]
        [DataRow(new string[] { "My Two Front Teeth", "", "Water Bottle" })]
        [DataRow(new string[] { "C# Essentials", "             ", "Mad Coding Skills" })]
        public void ParseWishlistFile_ContainsBlankLines_BlankLinesIgnored(string[] giftDescriptions)
        {
            var data = new List<string>() { $"Name: Tesla, Nikola" + Environment.NewLine };
            foreach (string desc in giftDescriptions)
            {
                data.Add(desc + Environment.NewLine);
            }

            var fileHandle = CreateTempFile_WriteDataToFile(data);
            var user = FileParser.ParseWishlistFile(fileHandle);
            var gifts = user.Wishlist;

            foreach (Gift gift in gifts)
            {
                Assert.IsFalse(string.IsNullOrWhiteSpace(gift.Description)); //ensuring blank lines did not make it into wishlist
                Assert.IsTrue(giftDescriptions.Contains<string>(gift.Description));
            }
        }


        private string CreateTempFile_WriteDataToFile(List<string> data)
        {
            string tempFile = Path.GetTempFileName();
            TempFiles.Add(tempFile);

            StreamWriter fout = new StreamWriter(tempFile);
            foreach (string line in data){
                fout.WriteLine(line);
            }
            fout.Close();
            return tempFile;
        }

       

       
    }
}