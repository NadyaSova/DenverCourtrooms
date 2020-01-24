using CourtRooms.Extensions;
using CourtRoomsDataLayer.Entities;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace CourtRooms.Models.Parsers
{
    public class CalendarCaseParser
    {
        private static Regex hearingRegex = new Regex(@"\d+:\d+:\d+\s+(?:AM|PM)\s+(.*)$");

        HtmlDocument Document;
        public CalendarCaseParser()
        {
            Document = new HtmlDocument();
        }

        public List<CalendarCase> GetCalendarCases(string html)
        {
            Document.LoadHtml(html);


            var tableRows = Document.DocumentNode.SelectNodes(@"//table[@class=""case_results""]//tr");
            if (tableRows == null || tableRows.Count == 0 )
                return null;

            var result = new List<CalendarCase>();
            string hearingName = null;

            foreach (var row in tableRows)
            {
                if (row.HasClass("title"))
                    continue;

                var header = row.Descendants("h3").ToList();
                if (header.Count > 0)
                {
                    hearingName = GetHearingName(header[0].InnerText?.Clear());
                    continue;
                }

                var tds = row.Descendants("td").ToArray();
                if (tds == null || tds.Length < 5)
                    continue;

                var nodeLink = tds[0].FirstChild;
                if (nodeLink == null)
                    continue;

                var calendarCase = new CalendarCase
                {
                    HearingName = hearingName,
                    Link = HttpUtility.HtmlDecode(nodeLink.Attributes["href"]?.Value),
                    CaseNumber = nodeLink.InnerText.Clear(),
                    Defendant = tds[1].InnerText?.Clear(),
                    Disposition = tds[2].InnerText?.Clear(),
                    NextCourtroom = tds[3].InnerText?.Clear(),
                    NextCourtDate = tds[4].InnerText?.ToDateFromDefaultUsFormat(),
                };

                result.Add(calendarCase);
            }

            return result;
        }

        private string GetHearingName(string fullName)
        {
            var match = hearingRegex.Match(fullName);
            if (match == null)
                return null;

            return match.Groups[1].Value;
        }
    }
}
