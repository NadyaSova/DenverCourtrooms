using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourtRoomsDataLayer.Entities
{
    [Table("tblBondDetail")]
    public class BondDetail
    {
        public int Id { get; set; }
        public int BondId { get; set; }
        public DateTime Date { get; set; }
        public string ActionCode { get; set; }
        public double? Amount { get; set; }
        public DateTime? SoeDate { get; set; }
        public string RelToParty { get; set; }

        public Bond Bond { get; set; }
    }
}
