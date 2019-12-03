using System;
using System.Collections.Generic;

namespace CourtRooms.Models.CrawlerSettings
{
    public class CaseNumberCrawlerSettings : CrawlerSettings<string>
    {
        public string From { get; private set; }
        public string To { get; private set; }

        public CaseNumberCrawlerSettings(string from, string to)
        {
            From = from;
            To = to;
        }

        public override IEnumerable<string> SearchParameters
        {
            get
            {
                if (From == To)
                {
                    yield return From;
                    yield break;
                }

                int i;
                for (i = 0; i < From.Length && i < To.Length && From[i] == To[i]; i++) ;

                var f = From.Substring(i);
                var t = To.Substring(i);

                if (!int.TryParse(f, out int intFrom) || !int.TryParse(t, out int intTo))
                    throw new Exception("Incorrect case number range");


                var endingLength = From.Length - i;
                var commonPart = From.Substring(0, i);

                for (var ending = intFrom; ending <= intTo; ending++)
                {
                    yield return commonPart + ending.ToString().PadLeft(endingLength, '0');
                }
            }
        }
    }
}
