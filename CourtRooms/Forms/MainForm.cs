using CourtRooms.Helpers;
using CourtRooms.Models;
using CourtRooms.Models.Crawlers;
using CourtRooms.Models.CrawlerSettings;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CourtRooms
{
    public partial class MainForm : Form
    {
        CancellationTokenSource cancellation;

        public MainForm()
        {
            InitializeComponent();

            dtTo.Value = DateTime.Now.Date;
            dtFrom.Value = dtTo.Value.AddMonths(-1);
            EnableSearch(true);
        }

        private bool ValidateInput()
        {
            if (tabMain.SelectedTab == tabCaseNumber)
                return ValidateCaseNumberInput();
            else if (tabMain.SelectedTab == tabCalendar)
                return ValidateCalendarInput();

            return false;
        }

        private bool ValidateCaseNumberInput()
        {
            var from = txtFrom.Text;
            var to = txtTo.Text;

            if (string.IsNullOrEmpty(from))
            {
                MessageBox.Show("Please enter 'from' case number");
                return false;
            }

            if (string.IsNullOrEmpty(to))
            {
                MessageBox.Show("Please enter 'to' case number");
                return false;
            }

            if (from.Length != to.Length)
            {
                MessageBox.Show("'from' and 'to' case numbers should be of same length");
                return false;
            }

            return true;
        }

        private bool ValidateCalendarInput()
        {
            var dateFrom = dtFrom.Value;
            var dateTo = dtTo.Value;

            if (dateFrom > dateTo)
            {
                MessageBox.Show("'From' date should be less than 'end' date");
                return false;
            }

            if (ddlRoom.SelectedIndex <0 )
            {
                MessageBox.Show("Please select a courtroom");
                return false;
            }

            return true;
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            await CollectData();
        }

        private async void txtTo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                await CollectData();
        }

        private async void dtTo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                await CollectData();
        }

        private async Task CollectData()
        {
            if (!ValidateInput())
                return;

            ClearLog();
            EnableSearch(false);

            cancellation = new CancellationTokenSource();

            try
            {
                await PerformCrawling();
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
            finally
            {
                EnableSearch(true);
                Log("The process has finished");
            }
        }

        private async Task PerformCrawling()
        {
            var crawlerParameters = new CrawlerParameters
            {
                Log = Log,
                LogLastProcessed = LogLastProcessed,
                SolveCaptchaManually = chkManualCaptcha.Checked,
                CancellationToken = cancellation.Token
            };

            if (tabMain.SelectedTab == tabCaseNumber)
            {
                using (var crawler = new CaseNumberCrawler(crawlerParameters))
                {
                    var settings = new CaseNumberCrawlerSettings(txtFrom.Text.Trim(), txtTo.Text.Trim());
                    await crawler.Start(settings);
                }
            }
            else
            {
                using (var crawler = new CalendarCrawler(crawlerParameters))
                {
                    var settings = new CalendarCrawlerSettings(dtFrom.Value, dtTo.Value, ddlRoom.SelectedValue.ToString(), ddlRoom.Text);
                    await crawler.Start(settings);
                }
            }
        }

        private void EnableSearch(bool enable)
        {
            tabMain.Enabled = enable;
            btnStart.Enabled = enable;
            chkManualCaptcha.Enabled = enable;

            btnCancel.Enabled = !enable;
        }

        private void LogLastProcessed(string message)
        {
            this.lblLastProcessedCase.Invoke((MethodInvoker)delegate
            {
                this.lblLastProcessedCase.Text = message;
            });
        }

        private void Log(string log)
        {
            this.txtLog.Invoke((MethodInvoker)delegate
            {
                this.txtLog.AppendText(log + Environment.NewLine);
            });
        }

        private void LogException(Exception ex)
        {
            Log($"Unhandled exception: {ex.Message}");

            var innerEx = ex.InnerException;
            while (innerEx != null)
            {
                Log($"Inner exception: {innerEx.Message}");
                innerEx = innerEx.InnerException;
            }
        }

        private void ClearLog()
        {
            lblLastProcessedCase.Text = string.Empty;
            txtLog.Text = string.Empty;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Log("Cancelling...");
            if (cancellation != null)
                cancellation.Cancel();
        }

        private async Task InitCourtRooms()
        {
            if (ddlRoom.Items.Count > 0)
                return;

            EnableSearch(false);
            Log("Loading courtrooms...");

            try
            {
                using (var courtroomsInitializer = new CourtroomsInitializer())
                {
                    ddlRoom.DataSource = await courtroomsInitializer.GetCourtrooms();
                    ddlRoom.ValueMember = "Item1";
                    ddlRoom.DisplayMember = "Item2";
                }

                Log("Courtrooms loaded.");
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
            finally
            {
                EnableSearch(true);
            }
        }

        private async void tabMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblLastProcessedCase.Text = string.Empty;

            if (tabMain.SelectedTab == tabCalendar)
            {
                lblLastProcessedTitle.Text = "Last Processed Date:";
                await InitCourtRooms();
            }
            else if (tabMain.SelectedTab == tabCaseNumber)
            {
                lblLastProcessedTitle.Text = "Last Processed Case:";
            }
        }
    }
}
