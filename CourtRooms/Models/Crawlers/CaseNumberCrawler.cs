using CourtRooms.Models.CrawlerSettings;
using CourtRoomsDataLayer.Helpers;
using OpenQA.Selenium;
using System;
using System.Threading.Tasks;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace CourtRooms.Models.Crawlers
{
    public class CaseNumberCrawler : Crawler<CaseNumberCrawlerSettings, string>
    {
        public CaseNumberCrawler(CrawlerParameters parameters) : base(parameters) { }

        public override async Task Start(CaseNumberCrawlerSettings crawlerSettings)
        {
            Log($"Processing cases {crawlerSettings.From} to {crawlerSettings.To}...");
            foreach (var caseNumber in crawlerSettings.SearchRange)
            {
                try
                {
                    await ProcessCase(caseNumber);
                    if (cancellationToken.IsCancellationRequested)
                        break;
                }
                catch(Exception ex)
                {
                    LogException(ex);
                    NotProcessedCases.Add(caseNumber);
                }
            }

            LogNotProcessed();
        }

        private async Task ProcessCase (string caseNumber)
        {
            Log(Environment.NewLine + $"Processing case number {caseNumber}...");

            if (await DefendantHelper.CaseExistAsync(caseNumber))
            {
                LogAlreadyExists(caseNumber);
                return;
            }

            await GoToSearchUrl();
            if (cancellationToken.IsCancellationRequested)
                return;

            await PassCaptcha(caseNumber);
            if (cancellationToken.IsCancellationRequested)
                return;

            if (courtroomsParser.IsNoRecords(selenium.CurrentPage))
            {
                Log("Case not found");
                return;
            }

            var defendant = courtroomsParser.GetDefendant(selenium.CurrentPage);
            if (defendant == null)
                return;

            await AddDefendant(defendant);
            LogLastProcessed(caseNumber);
        }

        protected override async Task GoToSearchUrl()
        {
            await Task.Run(() =>
            {
                selenium.GoToUrl(Constants.CaseNumberSearchUrl);
                wait.Until(ExpectedConditions.ElementExists(By.Id("cimage")));
            }, cancellationToken);
        }

        protected override async Task Search(string caseNumber, string captcha)
        {
            await Task.Run(() =>
            {
                selenium.Driver.FindElementByName("casenumber").Clear();
                selenium.Driver.FindElementByName("casenumber").SendKeys(caseNumber);
                selenium.Driver.FindElementByName("code").SendKeys(captcha + Keys.Enter);
            }, cancellationToken);
        }
    }
}
