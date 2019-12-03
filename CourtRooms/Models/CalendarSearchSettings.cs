using System;

namespace CourtRooms.Models
{
    public class CalendarSearchSettings
    {
        public DateTime Date { get; private set; }
        public string Courtroom { get; private set; }
        public string CourtroomName { get; private set; }

        public CalendarSearchSettings(DateTime date, string courtroom, string courtroomName)
        {
            Date = date;
            Courtroom = courtroom;
            CourtroomName = courtroomName;
        }

        public string DateString => Date.ToString("MM/dd/yyyy");
    }
}
