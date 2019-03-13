using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.IE;


namespace SecretSanta.web.UITests
{
    [TestClass]
    public class InternetExplorerTests : UserPageTests
    {
        [TestInitialize]
        public void Init()
        {
            Driver = new InternetExplorerDriver(Path.GetFullPath("."));
        }
    }
}