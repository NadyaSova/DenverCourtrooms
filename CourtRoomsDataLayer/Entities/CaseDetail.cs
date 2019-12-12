using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourtRoomsDataLayer.Entities
{
    [Table("tblCaseDetail")]
    public class CaseDetail
    {
        public int Id { get; set; }
        public int DefendantId { get; set; }
        public DateTime? BookingDate { get; set; }

        public Defendant Defendant { get; set; }
    }
}
