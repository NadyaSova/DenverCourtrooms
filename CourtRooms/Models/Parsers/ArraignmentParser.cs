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
    public class ArraignmentParser
    {
        HtmlDocument Document;
        public ArraignmentParser()
        {
            Document = new HtmlDocument();
        }

        public List<Arraignment> GetArraignments(string html)
        {
            Document.LoadHtml(html);

            var nodes = Document.DocumentNode.SelectNodes(@"//table//tr");
            if (nodes == null || nodes.Count == 0)
                return null;

            var arrs = new List<Arraignment>();

            Arraignment curArr = null;
            Arraignment newArr = null;
            foreach (var node in nodes)
            {
                newArr = GetArraignment(node, curArr);
                if (curArr != newArr && curArr != null)
                    arrs.Add(curArr);
                curArr = newArr;
            }

            if (newArr != null)
            {
                arrs.Add(newArr);
            }

            arrs.ForEach(SplitBondAndNotes);
            return arrs;
        }

        private Arraignment GetArraignment(HtmlNode node, Arraignment current)
        {
            var tds = node.Descendants()?.Where(x => x.Name == "td")?.ToArray();
            if (tds == null || tds.Length == 0)
                return null;

            var caseNumber = tds[0].InnerText?.Clear();
            if (caseNumber.StartsWith("CASE"))
                return null;

            var offer = tds[6].InnerText?.Clear();
            var bond = HttpUtility.HtmlDecode(tds[7].InnerText?.Clear());
            
            if (string.IsNullOrEmpty(caseNumber))
            {
                //Append data
                if (!string.IsNullOrEmpty(offer))
                    current.Offer += " " + offer;
                if (!string.IsNullOrEmpty(bond))
                    current.Bond += " " + bond;

                return current;
            }

            var newArraignment = new Arraignment
            {
                CaseNumber = caseNumber,
                Offer = offer,
                Bond = bond
            };

            return newArraignment;
        }

        private void SplitBondAndNotes(Arraignment a)
        {
            var bond = a.Bond;

            if (bond.StartsWith("pr", StringComparison.InvariantCultureIgnoreCase))
            {
                a.Bond = "pr";
                a.PoArNotes = bond.Substring(2).Trim();
                return;
            }

            var regex = new Regex(@"\$\d+");

            var matches = regex.Matches(bond);
            if (matches.Count > 0)
            {
                var match = matches[matches.Count - 1];
                var index = match.Index + match.Length;

                a.Bond = bond.Substring(0, index);
                a.PoArNotes = bond.Substring(index).Trim();
            }
            else
            {
                var index = bond.IndexOf(" ");
                if (index > 0)
                {
                    a.Bond = bond.Substring(0, index);
                    a.Offer = bond.Substring(index + 1);
                }
            }
        }
    }
}
