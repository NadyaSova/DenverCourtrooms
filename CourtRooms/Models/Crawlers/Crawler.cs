using CourtRooms.Helpers;
using CourtRooms.Models.CrawlerSettings;
using CourtRooms.Models.Parsers;
using CourtRoomsDataLayer.Entities;
using CourtRoomsDataLayer.Helpers;
using DeathByCaptcha;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace CourtRooms.Models.Crawlers
{
    public abstract class Crawler<CrawlerSettingsType, SearchSettingsType> : IDisposable
        where CrawlerSettingsType: CrawlerSettings<SearchSettingsType> 
    {
        protected Selenium selenium;
        protected WebDriverWait wait;
        protected CourtroomsParser courtroomsParser;
        protected InmateCrawler inmateCrawler;

        protected bool solveCaptchaManually;
        protected CancellationToken cancellationToken;
        protected readonly Action<string> Log;
        protected readonly Action<string> LogLastProcessed;

        protected HashSet<string> NotProcessedCases;

        public Crawler(CrawlerParameters parameters)
        {
            selenium = new Selenium();
            courtroomsParser = new CourtroomsParser();
            inmateCrawler = new InmateCrawler(parameters);
            wait = new WebDriverWait(selenium.Driver, TimeSpan.FromSeconds(8));
            NotProcessedCases = new HashSet<string>();

            Log = parameters.Log;
            LogLastProcessed = parameters.LogLastProcessed;
            cancellationToken = parameters.CancellationToken;
            solveCaptchaManually = parameters.SolveCaptchaManually;
        }

        public void Dispose()
        {
            selenium.Dispose();
            inmateCrawler.Dispose();
        }

        public abstract Task Start(CrawlerSettingsType crawlerSettings);
        protected abstract Task Search(SearchSettingsType settings, string captcha);
        protected abstract Task GoToSearchUrl();

        protected void LogCase(Defendant defendant, bool isNew)
        {
            Log($"{(isNew ? "Added" : "Updated")} case {defendant.CaseNumber} {defendant.LastName} {defendant.FirstName}");
        }

        protected void LogAlreadyExists(string caseNumber)
        {
            Log($"Case already exists in the database");
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

        protected async Task ProcessCase(string caseNumber)
        {
            if (courtroomsParser.IsNoRecords(selenium.CurrentPage))
            {
                Log("Case not found");
                return;
            }

            if (cancellationToken.IsCancellationRequested) return;
            var newDefendant = courtroomsParser.GetDefendant(selenium.CurrentPage);
            if (newDefendant == null)
                return;

            var defendant = await DefendantHelper.GetDefendantByCaseNumberAsync(caseNumber);
            if (defendant == null)
            {
                defendant = newDefendant;
            }
            else
            {
                LogAlreadyExists(caseNumber);
                if (cancellationToken.IsCancellationRequested) return;

                await DefendantHelper.ClearDefendant(defendant.Id);
                defendant.UpdateFrom(newDefendant);
            }

            await AddOrUpdateDefendant(defendant);
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

        protected async Task AddOrUpdateDefendant(Defendant defendant)
        {
            if (cancellationToken.IsCancellationRequested) return;
            await inmateCrawler.FillCourtInfo(defendant);

            if (cancellationToken.IsCancellationRequested) return;

            bool isNew = defendant.Id == 0;
            if (await DefendantHelper.AddOrUpdateDefendantAsync(defendant))
                LogCase(defendant, isNew);
            else
                LogAlreadyExists(defendant.CaseNumber);
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
