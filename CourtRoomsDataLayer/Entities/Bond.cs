using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourtRoomsDataLayer.Entities
{
    [Table("tblBond")]
    public class Bond
    {
        public int Id { get; set; }
        public int DefendantId { get; set; }
        public string Type { get; set; }
        public string SuretyName { get; set; }
        public string PowerNumber { get; set; }
        public int? BondNumber { get; set; }
        public string ArrestNumber { get; set; }
        public string Insurance { get; set; }

        public List<BondDetail> BondDetails { get; set; }
        public Defendant Defendant { get; set; }

    }
}
