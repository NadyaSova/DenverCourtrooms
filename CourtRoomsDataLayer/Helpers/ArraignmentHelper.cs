using CourtRoomsDataLayer.Context;
using CourtRoomsDataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourtRoomsDataLayer.Helpers
{
    public static class ArraignmentHelper
    {
        public static async Task<bool> AddArraignmentAsync(Arraignment arraignment)
        {
            using (var db = new CourtRoomsContext())
            {
                db.Arraignments.Add(arraignment);
                await db.SaveChangesAsync();
                return true;
            }
        }
        public static async Task<bool> AddArraignmentsAsync(List<Arraignment> arraignments)
        {
            using (var db = new CourtRoomsContext())
            {
                foreach (var arraignment in arraignments)
                    db.Arraignments.Add(arraignment);
                await db.SaveChangesAsync();
                return true;
            }
        }
    }
}
