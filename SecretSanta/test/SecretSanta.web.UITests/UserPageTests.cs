using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SecretSanta.web.UITests.WebPages;
using SecretSanta.web.UITests.WebPages.UserPages;

namespace SecretSanta.web.UITests
{
    [TestClass]
    public class UserPageTests
    {
        /* Because this URL is hardcoded, Changes to the port the web project runs on will break all of these tests unless this string is updated */
        private const string RootUrl = "https://localhost:5001/";
        
        private IWebDriver Driver { get; set; }

        [TestInitialize]
        public void Init()
        {
            Driver = new ChromeDriver(Path.GetFullPath("."));
        }

        [TestCleanup]
        public void Cleanup()
        {
            //Driver.Quit();
            //Driver.Dispose();
        }

        [TestMethod]
        public void CanNavigateToUsersPage()
        {
            Driver.Navigate().GoToUrl(RootUrl);

            var homePage = new HomePage(Driver);
            homePage.UsersLink.Click();
            
            Assert.IsTrue(Driver.Url.EndsWith(UsersPage.Slug));
        }

        [TestMethod]
        public void CanNavigateToAddUsersPage()
        {
            var rootUri = new Uri(RootUrl);
            Driver.Navigate().GoToUrl(new Uri(rootUri, UsersPage.Slug));
            var usersPage = new UsersPage(Driver);
            
            usersPage.AddUserLink.Click();
            
            Assert.IsTrue(Driver.Url.EndsWith(AddUserPage.Slug));
        }

        [TestMethod]
        public void CanAddNewUser()
        {
            Assert.Fail();
        }
    }
}