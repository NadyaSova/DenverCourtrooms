using CourtRooms.Extensions;
using CourtRooms.Models.CrawlerSettings;
using CourtRooms.Models.Parsers;
using CourtRoomsDataLayer.Entities;
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
        private CalendarCaseParser calendarCaseParser = new CalendarCaseParser();

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

            var cases = calendarCaseParser.GetCalendarCases(selenium.CurrentPage);
            if (cases == null)
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


            Log($"{cases.Count} cases found");

            foreach (var calendarCase in cases)
            {
                try
                {
                    calendarCase.Date = searchParameters.Date;
                    calendarCase.RoomNumber = searchParameters.CourtroomName;

                    await GoToCaseAndProcess(calendarCase, searchParameters, cancellationToken);
                    if (cancellationToken.IsCancellationRequested)
                        break;
                }
                catch (Exception ex)
                {
                    LogException(ex);
                    NotProcessedCases.Add(calendarCase.Link.GetUrlParameter("casenumber"));
                }
            }

            LogLastProcessed(searchParameters.DateString);
        }

        private async Task GoToCaseAndProcess(CalendarCase calendarCase, CalendarSearchSettings searchSettings, CancellationToken cancellationToken)
        {
            Log(Environment.NewLine + $"Processing case number {calendarCase.CaseNumber}...");

            if (cancellationToken.IsCancellationRequested) return;
            await selenium.GoToUrlAsync(calendarCase.Link, cancellationToken);

            calendarCase.IsFound = !courtroomsParser.IsNoRecords(selenium.CurrentPage);

            await CaseHelper.AddCalendarCaseAsync(calendarCase);
            await ProcessCase(calendarCase.CaseNumber);
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
            var caseLinks = calendarCaseParser.GetCalendarCases(selenium.CurrentPage);
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

            var cases = calendarCaseParser.GetCalendarCases(selenium.CurrentPage);
            if (cases == null || cases.Count == 0)
                return new TokenResult { IsEmpty = true };

            return new TokenResult
            {
                IsEmpty = false,
                Token = cases[0].Link.GetUrlParameter("token")
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
