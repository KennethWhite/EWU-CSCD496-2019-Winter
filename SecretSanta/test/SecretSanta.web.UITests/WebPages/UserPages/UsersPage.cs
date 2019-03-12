using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OpenQA.Selenium;

namespace SecretSanta.web.UITests.WebPages.UserPages
{
    public class UsersPage
    {
        public const string Slug = "Users";
        public IWebDriver Driver { get; }

        public IWebElement AddUserLink => Driver.FindElement(By.LinkText("Add User"));

        public List<string> DisplayedUsers
        {
            get
            {
                var elements = Driver.FindElements(By.CssSelector("h1+ul>li"));

                return elements
                    .Select(x =>
                    {
                        var text = x.Text;
                        if (text.EndsWith(" Edit Delete"))
                        {
                            text = text.Substring(0, text.Length - " Edit Delete".Length);
                        }

                        return text;
                    })
                    .ToList();
            }
        }
        
        public UsersPage(IWebDriver driver)
        {
            Driver = driver ?? throw new ArgumentNullException(nameof(driver));
        }

        public IWebElement GetDeleteLink(string firstName, string lastName)
        {
            ReadOnlyCollection<IWebElement> deleteLinks =
                Driver.FindElements(By.CssSelector("a.is-danger"));

           return deleteLinks.Single(x => x.GetAttribute("onclick").EndsWith($"{firstName} {lastName}?')"));
        }

        public IWebElement EditLink(string userName)
        {
            ReadOnlyCollection<IWebElement> userElements =
                Driver.FindElements(By.CssSelector("h1+ul>li"));

            var userElement = userElements.Single(x => x.Text.StartsWith(userName));

            return userElement.FindElement(By.CssSelector("a.button"));
        }

    }
}