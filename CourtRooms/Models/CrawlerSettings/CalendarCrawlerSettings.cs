using System;
using System.Collections.Generic;

namespace CourtRooms.Models.CrawlerSettings
{
    public class CalendarCrawlerSettings : CrawlerSettings<CalendarSearchSettings>
    {
        public DateTime DateFrom { get; private set; }
        public DateTime DateTo { get; private set; }
        public string Courtroom { get; private set; }
        public string CourtroomName { get; private set; }

        public CalendarCrawlerSettings(DateTime from, DateTime to, string courtroom, string courtroomName)
        {
            DateFrom = from;
            DateTo = to;
            Courtroom = courtroom;
            CourtroomName = courtroomName;
        }

        public override IEnumerable<CalendarSearchSettings> SearchParameters
        {
            get
            {
                if (DateFrom == DateTo)
                {
                    yield return new CalendarSearchSettings(DateFrom, Courtroom, CourtroomName);
                    yield break;
                }

                for (var date = DateFrom; date <= DateTo; date = date.AddDays(1))
                    yield return new CalendarSearchSettings(date, Courtroom, CourtroomName);
            }
        }
    }
}
