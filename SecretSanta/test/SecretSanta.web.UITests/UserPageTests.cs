using System;
using System.Collections.Generic;
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
            //Arrange
            Driver.Navigate().GoToUrl(RootUrl);

            //ACt
            var homePage = new HomePage(Driver);
            homePage.UsersLink.Click();
            
            //Assert
            Assert.IsTrue(Driver.Url.EndsWith(UsersPage.Slug));
        }

        [TestMethod]
        public void CanNavigateToAddUsersPage()
        {
            //Arrange
            var rootUri = new Uri(RootUrl);
            Driver.Navigate().GoToUrl(new Uri(rootUri, UsersPage.Slug));
            var usersPage = new UsersPage(Driver);
            
            //Act
            usersPage.AddUserLink.Click();
            
            //Assert
            Assert.IsTrue(Driver.Url.EndsWith(AddUserPage.Slug));
        }

        [TestMethod]
        public void CanNavigateToEditUserPage()
        {
            //Arrange
            var rootUri = new Uri(RootUrl);
            Driver.Navigate().GoToUrl(new Uri(rootUri, UsersPage.Slug));
            var usersPage = new UsersPage(Driver);
            usersPage.AddUserLink.Click();
            string userFirstName = "First Name" + Guid.NewGuid().ToString("N");
            string userLastName = "Last Name" + Guid.NewGuid().ToString("N");

            //Act
            CreateUser(RootUrl , userFirstName, userLastName);

            IWebElement editLink = usersPage.EditLink($"{userFirstName} {userLastName}");
            string linkText = editLink.GetAttribute("href");
            string userID = (linkText.Substring(linkText.LastIndexOf("/") + 1));
            var editPage = new EditUserPage(Driver);

            editLink.Click();

            Assert.AreEqual<string>(userID, editPage.CurrentUserID);
            Assert.AreEqual<string>(userFirstName, editPage.FirstNameTextBox.GetAttribute("value"));
            Assert.AreEqual<string>(userFirstName, editPage.LastNameTextBox.GetAttribute("value"));
        }


        [TestMethod]
        public void CanAddNewUser()
        {
            //Arrange
            string userFirstName = "First Name" + Guid.NewGuid().ToString("N");
            string userLastName = "Last Name" + Guid.NewGuid().ToString("N");
            
            // Act
            UsersPage page = CreateUser(RootUrl, userFirstName, userLastName);

            //Assert
            Assert.IsTrue(Driver.Url.EndsWith(UsersPage.Slug));
            List<string> users = page.DisplayedUsers;
            Assert.IsTrue(users.Contains(userFirstName));
        }


        [TestMethod]
        public void CanDeleteUser()
        {
            //Arrange
            string userFirstName = "First Name" + Guid.NewGuid().ToString("N");
            string userLastName = "Last Name" + Guid.NewGuid().ToString("N");
            UsersPage page = CreateUser(RootUrl, userFirstName, userLastName);

            //Act
            IWebElement deleteLink = page.GetDeleteLink(userFirstName, userLastName);
            deleteLink.Click();

            //Assert
            List<string> groupNames = page.DisplayedUsers;
            Assert.IsFalse(groupNames.Contains($"{userFirstName} {userLastName}"));
        }


        [TestMethod]
        public void CanEditUser()
        {
            //Arrange
            string userFirstName = "First Name" + Guid.NewGuid().ToString("N");
            string userLastName = "Last Name" + Guid.NewGuid().ToString("N");
            var usersPage = CreateUser(RootUrl, userFirstName, userLastName);
            usersPage.EditLink($"{userFirstName} {userLastName}").Click();
            EditUserPage editPage = new EditUserPage(Driver);

            //Act
            editPage.FirstNameTextBox.Clear();
            editPage.LastNameTextBox.Clear();
            string newFirst = "First Name" + Guid.NewGuid().ToString("N");
            string newLast = "Last Name" + Guid.NewGuid().ToString("N");
            editPage.FirstNameTextBox.SendKeys(newFirst);
            editPage.LastNameTextBox.SendKeys(newLast);
            editPage.SubmitButton.Click();

            //Assert
            List<string> users = usersPage.DisplayedUsers as List<string>;
            Assert.IsTrue(users.Contains($"{newFirst} {newLast}"));
            Assert.IsFalse(users.Contains($"{userFirstName} {userLastName}"));
        }



        private UsersPage CreateUser(string rootUrl, string firstName, string lastName)
        {
            var rootUri = new Uri(rootUrl);
            Driver.Navigate().GoToUrl(new Uri(rootUri, UsersPage.Slug));
            var page = new UsersPage(Driver);
            page.AddUserLink.Click();

            var addUserPage = new AddUserPage(Driver);

            addUserPage.UserFirstNameTextBox.SendKeys(firstName);
            addUserPage.UserLastNameTextBox.SendKeys(lastName);
            addUserPage.SubmitButton.Click();
            return page;
        }

        

    }
}