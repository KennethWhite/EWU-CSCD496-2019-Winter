using System;
using OpenQA.Selenium;

namespace SecretSanta.web.UITests.WebPages
{
    public class HomePage
    {
        public IWebDriver Driver { get; }

        public IWebElement UsersLink => Driver.FindElement(By.LinkText("Users"));

        public HomePage(IWebDriver driver)
        {
            Driver = driver ?? throw new ArgumentNullException(nameof(driver));
        }
    }
}