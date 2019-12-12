using System.Collections.Generic;

namespace CourtRooms.Models.CrawlerSettings
{
    public abstract class CrawlerSettings<SearchSettingsType>
    {
        public abstract IEnumerable<SearchSettingsType> SearchRange { get; }
    }
}
