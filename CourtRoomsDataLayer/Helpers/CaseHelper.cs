using CourtRoomsDataLayer.Context;
using CourtRoomsDataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourtRoomsDataLayer.Helpers
{
    public static class CaseHelper
    {
        public static async Task<bool> AddCalendarCaseAsync(CalendarCase ccase)
        {
            using (var db = new CourtRoomsContext())
            {
                var existingCase = db.CalendarCases.FirstOrDefault(x => x.CaseNumber == ccase.CaseNumber);
                if (existingCase != null)
                {
                    existingCase.Date = ccase.Date;
                    existingCase.RoomNumber = ccase.RoomNumber;
                    existingCase.IsFound = ccase.IsFound;             
                }
                else
                {
                    db.CalendarCases.Add(ccase);
                }

                await db.SaveChangesAsync();
                return true;
            }
        }

        public static async Task<bool> CaseExistAsync(string caseNumber)
        {
            using (var db = new CourtRoomsContext())
            {
                return await db.CalendarCases.AnyAsync(x => x.CaseNumber == caseNumber);
            }
        }
    }
}
