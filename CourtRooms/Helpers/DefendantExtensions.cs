using CourtRoomsDataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourtRooms.Helpers
{
    public static class DefendantExtensions
    {
        public static void UpdateFrom(this Defendant d1, Defendant d2)
        {
            d1.Actions = d2.Actions;
            d1.Attorneys = d2.Attorneys;
            d1.Sentences = d2.Sentences;
            d1.Costs = d2.Costs;
        }
    }
}
