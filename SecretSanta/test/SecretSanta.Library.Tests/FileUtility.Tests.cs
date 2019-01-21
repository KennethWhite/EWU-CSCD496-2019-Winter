using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Library;
using System;
using System.IO;

namespace SecretSanta.Library.Tests
{
    [TestClass]
    public class FileUtilityTests
    {
        private string ValidFilePath { get; set; } =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, 
                @"..\..\..\..\..\test\TestData\ValidFile.txt");

        [TestMethod]
        public void OpenFile_ValidFileName_NotNull()
        {
            FileStream fout = FileUtility.OpenFile(ValidFilePath);
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
