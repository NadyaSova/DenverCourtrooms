using CourtRoomsDataLayer.Context;
using CourtRoomsDataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.EntityFramework.Plus;

namespace CourtRoomsDataLayer.Helpers
{
    public static class DefendantHelper
    {
        public static async Task<bool> AddOrUpdateDefendantAsync(Defendant defendant)
        {
            if (defendant.Id == 0)
                return await AddDefendantAsync(defendant);
            else
                return await UpdateDefendantAsync(defendant);
        }

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

        public static async Task<bool> UpdateDefendantAsync(Defendant defendant)
        {
            using (var db = new CourtRoomsContext())
            {
                if (defendant.CourtInformations != null)
                {
                    foreach (var courtInformation in defendant.CourtInformations)
                    {
                        courtInformation.DefendantId = defendant.Id;
                        db.CourtInformations.Add(courtInformation);
                    }
                }

                if (defendant.CaseDetails != null)
                {
                    foreach (var caseDetail in defendant.CaseDetails)
                    {
                        caseDetail.DefendantId = defendant.Id;
                        db.CaseDetails.Add(caseDetail);
                    }
                }

                if (defendant.Actions != null)
                {
                    foreach (var action in defendant.Actions)
                    {
                        action.DefendantId = defendant.Id;
                        db.Actions.Add(action);
                    }
                }

                if (defendant.Attorneys != null)
                {
                    foreach (var attorney in defendant.Attorneys)
                    {
                        attorney.DefendantId = defendant.Id;
                        db.Attorneys.Add(attorney);
                    }
                }

                if (defendant.Sentences != null)
                {
                    foreach (var sentence in defendant.Sentences)
                    {
                        sentence.DefendantId = defendant.Id;
                        db.Sentences.Add(sentence);
                    }
                }

                if (defendant.Costs != null)
                {
                    foreach (var cost in defendant.Costs)
                    {
                        cost.DefendantId = defendant.Id;
                        db.Costs.Add(cost);
                    }
                }

                await db.SaveChangesAsync();
                return true;
            }
        }

        public static async Task<bool> CaseExistAsync(string caseNumber)
        {
            using (var db = new CourtRoomsContext())
            {
                return await db.Defendants.AnyAsync(x => x.CaseNumber == caseNumber);
            }
        }

        public static async Task<Defendant> GetDefendantByCaseNumberAsync(string caseNumber)
        {
            using (var db = new CourtRoomsContext())
            {
                return await db.Defendants.FirstOrDefaultAsync(x => x.CaseNumber == caseNumber);
            }
        }


        public static async Task ClearDefendant (int id)
        {
            using (var db = new CourtRoomsContext())
            {
                await db.CourtInformations.Where(x => x.DefendantId == id).DeleteAsync();
                await db.CaseDetails.Where(x => x.DefendantId == id).DeleteAsync();

                await db.Actions.Where(x => x.DefendantId == id).DeleteAsync();
                await db.Attorneys.Where(x => x.DefendantId == id).DeleteAsync();
                await db.Sentences.Where(x => x.DefendantId == id).DeleteAsync();
                await db.Costs.Where(x => x.DefendantId == id).DeleteAsync();
            }
        }
    }
}
