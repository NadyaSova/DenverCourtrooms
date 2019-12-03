using System;
using System.Threading;

namespace CourtRooms.Models
{
    public class CrawlerParameters
    {
        public Action<string> Log { get; set; }
        public Action<string> LogLastProcessed { get; set; }
        public bool SolveCaptchaManually { get; set; }
        public CancellationToken CancellationToken { get; set; }
    }
}
