using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Firefox;

namespace SecretSanta.web.UITests
{
    [TestClass]
    public class FireFoxTests : UserPageTests
    {
        [TestInitialize]
        public void Init()
        {
            Driver = new FirefoxDriver(Path.GetFullPath("."));
        }
    }
}