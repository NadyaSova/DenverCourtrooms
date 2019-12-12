using CourtRooms.Helpers;
using CourtRooms.Models.CrawlerSettings;
using CourtRooms.Models.Parsers;
using CourtRoomsDataLayer.Entities;
using CourtRoomsDataLayer.Helpers;
using DeathByCaptcha;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;


namespace CourtRooms.Models.Crawlers
{
    public abstract class Crawler<CrawlerSettingsType, SearchSettingsType> : IDisposable
        where CrawlerSettingsType: CrawlerSettings<SearchSettingsType> 
    {
        protected Selenium selenium;
        protected CourtroomsParser courtroomsParser;
        protected InmatesParser inmatesParser;
        protected WebDriverWait wait;

        protected bool solveCaptchaManually;
        protected CancellationToken cancellationToken;
        protected readonly Action<string> Log;
        protected readonly Action<string> LogLastProcessed;

        protected HashSet<string> NotProcessedCases;

        public Crawler(CrawlerParameters parameters)
        {
            selenium = new Selenium();
            courtroomsParser = new CourtroomsParser();
            inmatesParser = new InmatesParser();
            wait = new WebDriverWait(selenium.Driver, TimeSpan.FromSeconds(15));
            NotProcessedCases = new HashSet<string>();

            Log = parameters.Log;
            LogLastProcessed = parameters.LogLastProcessed;
            cancellationToken = parameters.CancellationToken;
            solveCaptchaManually = parameters.SolveCaptchaManually;
        }

        public void Dispose()
        {
            selenium.Dispose();
        }

        public abstract Task Start(CrawlerSettingsType crawlerSettings);
        protected abstract Task Search(SearchSettingsType settings, string captcha);
        protected abstract Task GoToSearchUrl();

        protected void LogCase(Defendant defendant)
        {
            Log($"Added case {defendant.CaseNumber} {defendant.LastName} {defendant.FirstName}");
        }

        protected void LogAlreadyExists(string caseNumber)
        {
            Log($"Case {caseNumber} already exists in the database");
        }

        protected void LogException(System.Exception ex)
        {
            Log($"Unhandled exception: {ex.Message}");

            var innerEx = ex.InnerException;
            while (innerEx != null)
            {
                Log($"Inner exception: {innerEx.Message}");
                innerEx = innerEx.InnerException;
            }
        }

        protected virtual void LogNotProcessed()
        {
            if (NotProcessedCases.Any())
            {
                Log($"Following cases have not been processed:");
                foreach (var caseNumber in NotProcessedCases)
                    Log(caseNumber);
            }
        }

        protected async Task PassCaptcha(SearchSettingsType settings)
        {
            if (solveCaptchaManually)
                await PassCaptchaManually(settings);
            else
                await PassCaptchaAutomatically(settings);
        }

        private async Task PassCaptchaManually(SearchSettingsType settings)
        {
            var isIncorrectCaptcha = true;
            while (isIncorrectCaptcha)
            {
                var solvedCaptcha = SolveCaptchaManually();
                if (cancellationToken.IsCancellationRequested)
                    return;

                await Search(settings, solvedCaptcha);
                if (cancellationToken.IsCancellationRequested)
                    return;

                isIncorrectCaptcha = courtroomsParser.IsIncorrectCaptcha(selenium.CurrentPage);
                if (isIncorrectCaptcha)
                    MessageBox.Show("Captcha was entered incorrectly. Please try again.");
            }
        }

        /// <summary>
        /// Pass captcha using DeathByCaptcha
        /// </summary>
        private async Task PassCaptchaAutomatically(SearchSettingsType settings)
        {
            var isIncorrectCaptcha = true;
            Captcha solvedCaptcha = null;

            while (isIncorrectCaptcha)
            {
                try
                {
                    solvedCaptcha = await SolveCaptcha();
                }
                catch(AccessDeniedException)
                {
                    Log("Cannot access DeathByCaptcha. Switching to solving captcha manually.");
                    solveCaptchaManually = true;

                    await PassCaptchaManually(settings);
                    return;
                }

                if (cancellationToken.IsCancellationRequested)
                    return;

                if (solvedCaptcha == null)
                {
                    Log("Captcha could not be solved. Making another attempt...");

                    await GoToSearchUrl();
                    if (cancellationToken.IsCancellationRequested)
                        return;

                    continue;
                }

                await Search(settings, solvedCaptcha.Text);
                if (cancellationToken.IsCancellationRequested)
                    return;

                isIncorrectCaptcha = courtroomsParser.IsIncorrectCaptcha(selenium.CurrentPage);
                if (isIncorrectCaptcha)
                {
                    Log("Captcha was solved incorrectly. Making another attempt...");
                    await CaptchaHelper.ReportAsync(solvedCaptcha, cancellationToken);
                    if (cancellationToken.IsCancellationRequested)
                        return;
                }
            }
        }

        protected async Task AddDefendant(Defendant defendant)
        {
            if (cancellationToken.IsCancellationRequested) return;
            await FillInmatesInfo(defendant);

            if (cancellationToken.IsCancellationRequested) return;
            if (await DefendantHelper.AddDefendantAsync(defendant))
                LogCase(defendant);
            else
                LogAlreadyExists(defendant.CaseNumber);
        }

        private void PerformCurrentInmateSearch()
        {
            selenium.Driver.FindElementById("SearchBox").SendKeys(OpenQA.Selenium.Keys.Enter);
            wait.Until(ExpectedConditions.ElementExists(By.ClassName("progress-bar")));
            wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("InmateSearchTable")));
        }

        public async Task FillInmatesInfo(Defendant defendant)
        {
            if (cancellationToken.IsCancellationRequested)
                return;
            await selenium.GoToUrlAsync(Constants.InmateSearchUrl, cancellationToken);
            selenium.Driver.FindElementById("SearchBox").SendKeys($"{defendant.LastName}, {defendant.FirstName}");
            PerformCurrentInmateSearch();

            for (var pageNumber = 1; GoToInmatesPage(pageNumber); pageNumber++)
            {
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
                            defendant.CaseDetails = new List<CaseDetail> { caseDetail };

                        Log($"Found {courtInfos.Count} court{(courtInfos.Count == 1 ? "" : "s")} for the defendant");
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
         
        
        protected async Task<Captcha> SolveCaptcha()
        {
            return await Task.Run(() =>
            {
                using (var captchaImage = GetCaptchaImage())
                {
                    return CaptchaHelper.Decode(captchaImage);
                }
            }, cancellationToken);
        }

        protected string SolveCaptchaManually()
        {
            var bitmapCaptcha = GetCaptchaImage();

            try
            {
                using (var frm = new CaptchaForm(bitmapCaptcha))
                {
                    var result = frm.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        return frm.Captcha;
                    }
                }
            }
            finally
            {
                bitmapCaptcha.Dispose();
            }

            return null;
        }

        protected Bitmap GetCaptchaImage()
        {
            var captcha = selenium.Driver.FindElementById("cimage");
            var location = captcha.Location;

            var screenshot = selenium.Driver.GetScreenshot();

            using (var stream = new MemoryStream(screenshot.AsByteArray))
            {
                using (var bitmap = new Bitmap(stream))
                {
                    var part = new RectangleF(location.X, location.Y, captcha.Size.Width, captcha.Size.Height);
                    return bitmap.Clone(part, bitmap.PixelFormat);
                }
            }
        }

    }
}
