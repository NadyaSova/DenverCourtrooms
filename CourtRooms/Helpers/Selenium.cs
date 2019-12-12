using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CourtRooms.Helpers
{
    public class Selenium : IDisposable
    {
        public ChromeDriver Driver { get; private set; }

        public Selenium()
        {
            Driver = InitializeChromeNoCommandPrompt();
        }

        public ChromeDriver InitializeChrome()
        {
            return new ChromeDriver(".", GetChromeOptions(), TimeSpan.FromMinutes(3));
        }

        public ChromeDriver InitializeChromeNoCommandPrompt()
        {
            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;
            return new ChromeDriver(chromeDriverService, GetChromeOptions(), TimeSpan.FromMinutes(3));
        }

        private ChromeOptions GetChromeOptions()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--disable-notifications");
            chromeOptions.AddArgument("--headless");
            chromeOptions.AddArgument("-no-sandbox");

            return chromeOptions;
        }

        public void Dispose()
        {
            if (Driver != null)
            {
                Driver.Quit();
            }
        }
        public string CurrentPage
        {
            get
            {
                return Driver.PageSource;
            }
        }

        public void GoToUrl(string url)
        {
            Driver.Navigate().GoToUrl(url);
        }

        public async Task GoToUrlAsync(string url)
        {
            await Task.Run(() => { GoToUrl(url); });
        }

        public async Task GoToUrlAsync(string url, CancellationToken cancellationToken)
        {
            await Task.Run(() => { GoToUrl(url); }, cancellationToken);
        }

        public void ClickWithJsExecutor(IWebElement element)
        {
            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", element);
        }
    }
}
