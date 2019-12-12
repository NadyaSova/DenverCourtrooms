using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CourtRooms.Extensions
{
    public static class StringExtensions
    {
        private static readonly string ShortDateFormat = "MM/dd/yyyy";
        private static readonly string InmatesDateFormat = "MMMM dd, yyyy";
        private static readonly CultureInfo UsCulture = CultureInfo.CreateSpecificCulture("en-US");

        public static DateTime? ToDateFromFormat(this string s, string format)
        {
            if (DateTime.TryParseExact(s, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                return result;

            return null;
        }

        public static DateTime? ToDate(this string s)
        {
            if (DateTime.TryParse(s, out DateTime result))
                return result;

            return null;
        }

        public static DateTime? ToDateFromShortFormat(this string s)
        {
            return s.ToDateFromFormat(ShortDateFormat);
        }

        public static DateTime? ToDateFromInmatesFormat(this string s)
        {
            return s.ToDateFromFormat(InmatesDateFormat);
        }

        public static DateTime? ToDateFromDefaultUsFormat(this string s)
        {
            if (DateTime.TryParse(s, UsCulture, DateTimeStyles.None, out DateTime result))
                return result;

            return null;
        }

        public static double? ToDoubleMoney(this string s)
        {
            if (double.TryParse(s, NumberStyles.Currency, UsCulture, out double result))
                return result;

            return null;
        }

        public static int? ToInt(this string s)
        {
            if (int.TryParse(s, out int result))
                return result;

            return null;
        }

        public static string TrimBeforeColon(this string s)
        {
            var i = s.IndexOf(':');
            if (i < 0)
                return s;

            return s.Substring(i + 1).Trim();
        }

        public static string Clear(this string s)
        {
            return s.Replace("\t", "").Replace("\r", "").Replace("\n", "").Trim();
        }

        public static string GetUrlParameter(this string url, string parameterName)
        {
            var uri = new Uri(url);
            return HttpUtility.ParseQueryString(uri.Query).Get(parameterName);
        }
    }
}
