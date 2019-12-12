using CourtRooms.Extensions;
using CourtRoomsDataLayer.Entities;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourtRooms.Models.Parsers
{
    public class InmatesParser
    {
        HtmlDocument Document;
        public InmatesParser()
        {
            Document = new HtmlDocument();
        }

        public List<CourtInformation> GetCourtInformations(string html)
        {
            Document.LoadHtml(html);

            var nodes = Document.DocumentNode.SelectNodes(@"//table[@id=""CourtInformationTable""]/tbody/tr[@class=""row""]");
            if (nodes == null || nodes.Count == 0)
                return null;

            var infos = new List<CourtInformation>();

            foreach (var node in nodes)
            {
                infos.Add(GetCourtInformation(node));
            }

            return infos;
        }

        private CourtInformation GetCourtInformation(HtmlNode node)
        {
            var tds = node.Descendants()?.Where(x => x.Name == "td")?.ToArray();
            if (tds == null || tds.Length == 0)
                return null;

            var info = new CourtInformation
            {
                CaseNumber = tds[0].InnerText?.Clear(),
                CourtDate = tds[1].InnerText.ToDateFromInmatesFormat(),
                CourtLocation = tds[2].InnerText?.Clear(),
                CourtRoom = tds[3].InnerText?.Clear(),
                CourtTime = tds[4].InnerText?.Clear(),
                CourtStatus = tds[5].InnerText?.Clear(),
                Bond = tds[6].InnerText?.Clear(),
                HoldingAgency = tds[7].InnerText?.Clear()
            };

            return info;
        }

        public CaseDetail GetCaseDetail(string html)
        {
            Document.LoadHtml(html);
            var nodes = Document.DocumentNode.SelectNodes(@"//div[@id=""CaseDetailsTable""]/div[@class=""row""]/div");
            if (nodes == null || nodes.Count == 0)
                return null;

            return new CaseDetail
            {
                BookingDate = nodes[1].InnerText.ToDate()
            };
        }
    }
}
