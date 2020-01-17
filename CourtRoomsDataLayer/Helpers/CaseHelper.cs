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
        public static async Task<bool> AddNotFoundCaseAsync(NotFoundCase notFoundCase)
        {
            using (var db = new CourtRoomsContext())
            {
                if (await CaseExistAsync(notFoundCase.CaseNumber))
                    return false;

                db.NotFoundCases.Add(notFoundCase);
                await db.SaveChangesAsync();
                return true;
            }
        }

        public static async Task<bool> CaseExistAsync(string caseNumber)
        {
            using (var db = new CourtRoomsContext())
            {
                return await db.NotFoundCases.AnyAsync(x => x.CaseNumber == caseNumber);
            }
        }
    }
}
