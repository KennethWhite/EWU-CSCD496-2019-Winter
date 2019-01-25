using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SecretSanta.Library.Tests
{
    [TestClass]
    public class FileUtilityTests
    {
        private List<string> TempFiles { get; } = new List<string>();

        [TestInitialize]
        public void InitializeTestFile()
        {
            TempFiles.ForEach(File.Delete);
            TempFiles.Clear();
            ValidFile = Path.GetTempFileName();
            TempFiles.Add(ValidFile);
        }

        [TestCleanup]
        public void CleanupTestFile()
        {
            TempFiles.ForEach(File.Delete);
            TempFiles.Clear();
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
