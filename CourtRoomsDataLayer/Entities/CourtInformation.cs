using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourtRoomsDataLayer.Entities
{
    [Table("tblCourtInformation")]
    public class CourtInformation
    {
        public int Id { get; set; }
        public int DefendantId { get; set; }
        public string CaseNumber { get; set; }
        public DateTime? CourtDate { get; set; }
        public string CourtLocation { get; set; }
        public string CourtRoom { get; set; }
        public string CourtTime { get; set; }
        public string CourtStatus { get; set; }
        public string Bond { get; set; }
        public string HoldingAgency { get; set; }


        public Defendant Defendant { get; set; }
    }
}
