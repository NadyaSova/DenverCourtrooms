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
    }
}
