using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SecretSanta.Library.Tests
{
    [TestClass]
    public class FileParserTests
    {
        private List<string> TempFiles { get; } = new List<string>();

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