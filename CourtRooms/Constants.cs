using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourtRooms
{
    public static class Constants
    {
        public static string CaseNumberSearchUrl = "https://www.denvercountycourt.org/search/?searchtype=searchbycaseno";
        public static string CalendarSearchUrl = "https://www.denvercountycourt.org/search/?searchtype=searchdocket";
        public static string CalendarSearchRequestUrl = "https://www.denvercountycourt.org/search/?date={date}&room={room}&token={token}&searchtype=searchdocket";
        public static string InmateSearchUrl = "https://denvergov.org/inmatesearch";
    }
}
