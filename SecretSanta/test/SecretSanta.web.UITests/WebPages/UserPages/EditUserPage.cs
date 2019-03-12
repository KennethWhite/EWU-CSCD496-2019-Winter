using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SecretSanta.web.UITests.WebPages.UserPages
{
    class EditUserPage
    {
        IWebDriver Driver { get; }
        public IWebElement FirstNameTextBox => Driver.FindElement(By.Id("FirstName"));
        public IWebElement LastNameTextBox => Driver.FindElement(By.Id("LastName"));
        public string CurrentUserID =>
            Driver.Url.Substring(Driver.Url.LastIndexOf("/") + 1);
        public IWebElement SubmitButton =>
            Driver
                .FindElements(By.CssSelector("button.is-primary"))
                .Single(x => x.Text == "Submit");

        public EditUserPage(IWebDriver driver)
        {
            Driver = driver ?? throw new ArgumentNullException(nameof(driver));
        }
    }
}
