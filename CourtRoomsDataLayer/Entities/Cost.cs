using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourtRoomsDataLayer.Entities
{
    [Table("tblCost")]
    public class Cost
    {
        public int Id { get; set; }
        public int DefendantId { get; set; }
        public string Description { get; set; }
        public double? Imposed { get; set; }
        public double? Suspended { get; set; }
        public double? CcwpCts { get; set; }
        public double? Paid { get; set; }
        public double? Due { get; set; }
        public Defendant Defendant { get; set; }
    }
}
