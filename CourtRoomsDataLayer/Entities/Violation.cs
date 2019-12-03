using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourtRoomsDataLayer.Entities
{
    [Table("tblCharge")]
    public class Violation
    {
        public int Id { get; set; }
        public int DefendantId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int? Points { get; set; }
        public string Disposition { get; set; }
        public string ClassCode { get; set; }

        public Defendant Defendant { get; set; }
    }
}
