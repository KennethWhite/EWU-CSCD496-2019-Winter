using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Library;
using System;
using System.IO;

namespace SecretSanta.Library.Tests
{
    [TestClass]
    public class FileUtilityTests
    {


        [TestInitialize]
        public void InizializeTestFile()
        {
            ValidFile = Path.GetTempFileName();
        }

        [TestCleanup]
        public void CleanupTestFile()
        {
            File.Delete(Path.GetFullPath(ValidFile));
        }
        public string ValidFile { get; set; }

        [TestMethod]
        public void OpenFile_ValidFileName_NotNull()
        {
            FileStream fout = FileUtility.OpenFile(ValidFile);
            Assert.IsNotNull(fout);
            fout.Close();
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void OpenFile_FileNotFound_ThrowsFNFException()
        {
            FileStream fout = FileUtility.OpenFile("NoFile.txt");
            Assert.Fail();
        }

        [TestMethod]
        [DataRow("")]
        [DataRow("       ")]
        [DataRow(null)]
        [ExpectedException(typeof(ArgumentException))]
        public void OpenFile_EmptyString_ThrowsArgumentException(string path)
        {
            FileUtility.OpenFile(path);
        }

        
    }
}
