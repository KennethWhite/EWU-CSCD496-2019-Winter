using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Library;
using System;
using System.Collections.Generic;
using System.IO;

namespace SecretSanta.Library.Tests
{
    [TestClass]
    public class FileUtilityTests
    {
        private List<string> TempFiles { get; } = new List<string>();

        [TestInitialize]
        public void InizializeTestFile()
        {
            if (TempFiles.Count > 0)
            {
                foreach (string filePath in TempFiles)
                {
                    File.Delete(filePath);
                }
                TempFiles.Clear();
            }
            ValidFile = Path.GetTempFileName();
            TempFiles.Add(ValidFile);
        }

        [TestCleanup]
        public void CleanupTestFile()
        {
            foreach (string filePath in TempFiles)
            {
                File.Delete(filePath);
            }
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
