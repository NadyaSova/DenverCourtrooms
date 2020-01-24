using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourtRoomsDataLayer.Entities
{
    [Table("tblCalendarCase")]
    public class CalendarCase
    {
        [Key]
        [StringLength(10)]
        public string CaseNumber { get; set; }
        public DateTime Date { get; set; }
        public string RoomNumber { get; set; }
        public bool IsFound { get; set; }
        public string HearingName { get; set; }
        public string Defendant { get; set; }
        public string Disposition { get; set; }
        public string NextCourtroom { get; set; }
        public DateTime? NextCourtDate { get; set; }

        [NotMapped]
        public string Link { get; set; }
    }
}
