using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourtRooms.Helpers
{
    public class CourtroomsInitializer : IDisposable
    {
        protected Selenium selenium;

        public CourtroomsInitializer()
        {
            selenium = new Selenium();
        }

        public void Dispose()
        {
            selenium.Dispose();
        }

        public async Task<List<Tuple<string, string>>> GetCourtrooms()
        {
            await selenium.GoToUrlAsync(Constants.CalendarSearchUrl);

            var ddlRooms = selenium.Driver.FindElementByName("room");
            return ddlRooms
                .FindElements(By.TagName("option"))
                .Select(x => new Tuple<string, string>(x.GetAttribute("value"), x.Text))
                .ToList();
        }
    }
}
