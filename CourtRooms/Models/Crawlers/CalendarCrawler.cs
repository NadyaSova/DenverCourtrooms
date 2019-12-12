using CourtRooms.Extensions;
using CourtRooms.Models.CrawlerSettings;
using CourtRoomsDataLayer.Helpers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace CourtRooms.Models.Crawlers
{
    public class CalendarCrawler : Crawler<CalendarCrawlerSettings, CalendarSearchSettings>
    {
        public CalendarCrawler(CrawlerParameters parameters) : base(parameters) { }

        private string token = null;
        private CalendarSearchSettings testSearchParameters = null;
        private List<string> notProcessedDates = new List<string>();

        public override async Task Start(CalendarCrawlerSettings crawlerSettings)
        {
            foreach (var searchParameters in crawlerSettings.SearchRange)
            {
                try
                {
                    await ProcessDate(searchParameters);
                    if (cancellationToken.IsCancellationRequested)
                        break;
                }
                catch (Exception ex)
                {
                    LogException(ex);
                    notProcessedDates.Add(searchParameters.DateString);
                }
            }

            LogNotProcessed();
        }

        private async Task ProcessDate(CalendarSearchSettings searchParameters)
        {
            Log($"Processing room {searchParameters.CourtroomName}, date {searchParameters.DateString}...");

            if (token == null)
            {
                var updated = await UpdateToken(searchParameters);
                if (cancellationToken.IsCancellationRequested)
                    return;

                if (!updated)
                    return;
            }

            await SearchWithToken(searchParameters, token);
            if (cancellationToken.IsCancellationRequested)
                return;

            var caseLinks = courtroomsParser.GetCaseLinks(selenium.CurrentPage);
            if (caseLinks == null)
            {
                var isTokenValid = await IsTokenValid();
                if (cancellationToken.IsCancellationRequested)
                    return;

                if (isTokenValid)
                {
                    Log("No cases found");
                    return;
                }
                else
                {
                    token = null;
                    var updated = await UpdateToken(searchParameters);
                    if (cancellationToken.IsCancellationRequested)
                        return;

                    if (!updated)
                        return;
                }
            }


            Log($"{caseLinks.Count} cases found");

            foreach (var caseLink in caseLinks)
            {
                try
                {
                    await ProcessCase(caseLink, cancellationToken);
                    if (cancellationToken.IsCancellationRequested)
                        break;
                }
                catch (Exception ex)
                {
                    LogException(ex);
                    NotProcessedCases.Add(caseLink.GetUrlParameter("casenumber"));
                }
            }

            LogLastProcessed(searchParameters.DateString);
        }

        private async Task ProcessCase(string caseLink, CancellationToken cancellationToken)
        {
            var caseNumber = caseLink.GetUrlParameter("casenumber");

            Log(Environment.NewLine + $"Processing case number {caseNumber}...");

            if (await DefendantHelper.CaseExistAsync(caseNumber))
            {
                LogAlreadyExists(caseNumber);
                return;
            }
            if (cancellationToken.IsCancellationRequested)
                return;

            await selenium.GoToUrlAsync(caseLink, cancellationToken);
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
        }

        protected override async Task GoToSearchUrl()
        {
            await Task.Run(() =>
            {
                selenium.GoToUrl(Constants.CalendarSearchUrl);
                wait.Until(ExpectedConditions.ElementExists(By.Id("cimage")));
            }, cancellationToken);
        }

        protected override async Task Search(CalendarSearchSettings settings, string captcha)
        {
            await Task.Run(() =>
            {
                selenium.Driver.FindElementByName("date").Clear();
                selenium.Driver.FindElementByName("date").SendKeys(settings.DateString);

                var roomSelect = new SelectElement(selenium.Driver.FindElementByName("room"));
                roomSelect.SelectByValue(settings.Courtroom);

                selenium.Driver.FindElementByName("code").SendKeys(captcha + Keys.Enter);
            }, cancellationToken);
        }

        private async Task SearchWithToken(CalendarSearchSettings settings, string token)
        {
            await Task.Run(() =>
            {
                var url = Constants.CalendarSearchRequestUrl
                    .Replace("{date}", settings.DateString)
                    .Replace("{room}", settings.Courtroom)
                    .Replace("{token}", token);

                selenium.GoToUrl(url);
            }, cancellationToken);
        }


        private async Task<bool> IsTokenValid()
        {
            //Let's search with the token and settings which previously gave us some cases
            await SearchWithToken(testSearchParameters, token);
            if (cancellationToken.IsCancellationRequested)
                return false;

            //If we still see come cases, the token is valid.
            //If we don't see them, the token has expired.
            var caseLinks = courtroomsParser.GetCaseLinks(selenium.CurrentPage);
            return caseLinks != null;
        }

        private async Task<bool> UpdateToken(CalendarSearchSettings searchParameters)
        {
            var tokenResult = await GetToken(searchParameters);
            if (cancellationToken.IsCancellationRequested)
                return false;

            if (tokenResult.IsEmpty)
            {
                Log("No cases found");
                return false;
            }

            testSearchParameters = searchParameters;
            token = tokenResult.Token;
            return true;
        }

        private async Task<TokenResult> GetToken(CalendarSearchSettings settings)
        {
            await GoToSearchUrl();
            if (cancellationToken.IsCancellationRequested)
                return null;

            await PassCaptcha(settings);
            if (cancellationToken.IsCancellationRequested)
                return null;

            var caseLinks = courtroomsParser.GetCaseLinks(selenium.CurrentPage);
            if (caseLinks == null || caseLinks.Count == 0)
                return new TokenResult { IsEmpty = true };

            return new TokenResult
            {
                IsEmpty = false,
                Token = caseLinks[0].GetUrlParameter("token")
            };
        }
        protected override void LogNotProcessed()
        {
            base.LogNotProcessed();

            if (notProcessedDates.Any())
            {
                Log($"Following dates have not been processed:");
                foreach (var date in notProcessedDates)
                    Log(date);
            }
        }
    }
}
