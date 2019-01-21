using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecretSanta.Library;
using System;
using System.IO;

namespace SecretSanta.Library.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private string ValidFilePath { get; set; } =
            Environment.CurrentDirectory;
        [TestMethod]
        public void OpenFile_ReturnsTrue()
        {

            bool result = FileUtility.OpenFile(ValidFilePath,
                out FileStream fout);
            Assert.IsTrue(result);
            fout.Close();
        }

        [TestMethod]
        public void OpenFile_ValidFile_ReturnsTrue()
        {

        }
    }
}
