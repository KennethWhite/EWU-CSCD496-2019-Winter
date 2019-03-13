using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Chrome;

namespace SecretSanta.web.UITests
{
    [TestClass]
    public class ChromeTests : UserPageTests
    {
        [TestInitialize]
        public void Init()
        {
            Driver = new ChromeDriver(Path.GetFullPath("."));
        }
    }
}