using CourtRooms.Helpers;
using CourtRooms.Models.Parsers;
using CourtRoomsDataLayer.Entities;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace CourtRooms.Models.Crawlers
{
    public class InmateCrawler : IDisposable
    {
        protected Selenium selenium;
        protected WebDriverWait wait;

        protected InmatesParser inmatesParser;
        protected CancellationToken cancellationToken;
        protected readonly Action<string> Log;

        public InmateCrawler(CrawlerParameters parameters)
        {
            selenium = new Selenium();
            inmatesParser = new InmatesParser();
            wait = new WebDriverWait(selenium.Driver, TimeSpan.FromSeconds(15));

            Log = parameters.Log;
            cancellationToken = parameters.CancellationToken;
        }

        public void Dispose()
        {
            selenium.Dispose();
        }

        public async Task FillCourtInfo(Defendant defendant)
        {
            await GoToInmateSearchIfNeeded();

            if (cancellationToken.IsCancellationRequested)
                return;

            var searchBox = selenium.Driver.FindElementById("SearchBox");
            searchBox.Clear();
            searchBox.SendKeys($"{defendant.LastName}, {defendant.FirstName}");
            PerformCurrentInmateSearch();

            for (var pageNumber = 1; GoToInmatesPage(pageNumber); pageNumber++)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                var linkNumber = 0;
                while (ClickNextLinkWithName(ref linkNumber, defendant.LastName))
                {
                    linkNumber++;

                    if (cancellationToken.IsCancellationRequested)
                        return;

                    var courtInfos = inmatesParser.GetCourtInformations(selenium.CurrentPage);
                    if (courtInfos.Any(x => x.CaseNumber == defendant.CaseNumber))
                    {
                        defendant.CourtInformations = courtInfos;

                        var caseDetail = inmatesParser.GetCaseDetail(selenium.CurrentPage);
                        if (caseDetail != null)
                        {
                            if (defendant.CaseDetails.All(x => x.BookingDate != caseDetail.BookingDate))
                                defendant.CaseDetails.Add(caseDetail);
                        }


                        Log($"Found {courtInfos.Count} court {(courtInfos.Count == 1 ? "entry" : "entries")} for the defendant");
                        return;
                    }
                    else
                    {
                        PerformCurrentInmateSearch();
                        GoToInmatesPage(pageNumber);
                    }
                }
            }
            Log("Court information not found");
        }

        private void PerformCurrentInmateSearch()
        {
            selenium.Driver.FindElementById("SearchBox").SendKeys(Keys.Enter);
            wait.Until(ExpectedConditions.ElementExists(By.ClassName("progress-bar")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("InmateSearchTable")));
        }

        private async Task GoToInmateSearchIfNeeded()
        {
            var searchBox = selenium.Driver.FindElementsById("SearchBox");
            if (searchBox.Count == 0)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                await selenium.GoToUrlAsync(Constants.InmateSearchUrl, cancellationToken);
                wait.Until(ExpectedConditions.ElementIsVisible(By.Id("SearchBox")));
            }
        }

        private bool GoToInmatesPage(int pageNumber)
        {
            if (cancellationToken.IsCancellationRequested)
                return false;

            var pages = selenium.Driver
                .FindElementByClassName("panel-footer")
                ?.FindElements(By.TagName("span"))
                ?.Where(x => x.Text != "...")
                ?.ToList();

            if (pages == null || pages.Count < pageNumber + 1)
                return false;

            var page = pages[pageNumber];

            wait.Until(ExpectedConditions.ElementToBeClickable(page));
            page.Click();

            return true;
        }

        private bool ClickNextLinkWithName(ref int n, string lastName)
        {
            if (cancellationToken.IsCancellationRequested)
                return false;

            var trs = selenium.Driver.FindElementById("InmateSearchTable")
                ?.FindElement(By.TagName("tbody"))
                ?.FindElements(By.ClassName("row"))
                .Where(x => !string.IsNullOrEmpty(x.Text))
                .ToList();

            if (trs == null || trs.Count < n + 1)
                return false;

            IWebElement nextLink = null;
            lastName = lastName.ToLower();

            for (var i = n; i < trs.Count; i++)
            {
                var a = trs[i].FindElement(By.TagName("a"));
                if (a.Text.ToLower() == lastName)
                {
                    nextLink = a;
                    n = i;
                    break;
                }
            }
            if (nextLink == null)
                return false;

            selenium.ClickWithJsExecutor(nextLink);
            wait.Until(ExpectedConditions.ElementExists(By.Id("PersonalDetailsTable")));

            return true;
        }
    }
}
