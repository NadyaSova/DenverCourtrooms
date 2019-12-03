using CourtRoomsDataLayer.Context;
using CourtRoomsDataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourtRoomsDataLayer.Helpers
{
    public static class DefendantHelper
    {
        public static async Task<bool> AddDefendantAsync(Defendant defendant)
        {
            using (var db = new CourtRoomsContext())
            {
                if (await CaseExistAsync(defendant.CaseNumber))
                    return false;

                db.Defendants.Add(defendant);
                await db.SaveChangesAsync();
                return true;
            }
        }

        public static bool CaseExist(string caseNumber)
        {
            using (var db = new CourtRoomsContext())
            {
                return db.Defendants.Any(x => x.CaseNumber == caseNumber);
            }
        }

        public static async Task<bool> CaseExistAsync(string caseNumber)
        {
            return await Task.Run(() => { return CaseExist(caseNumber); });
        }
    }
}
