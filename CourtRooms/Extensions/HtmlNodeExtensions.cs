using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourtRooms.Extensions
{
    public static class HtmlNodeExtensions
    {
        public static HtmlNode NthChild(this HtmlNode node, string tag, int number)
        {
            return node.ChildNodes.Where(x => x.Name == tag).ToArray()[number];
        }
    }
}
