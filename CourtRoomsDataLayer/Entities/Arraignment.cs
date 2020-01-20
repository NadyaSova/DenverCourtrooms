using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourtRoomsDataLayer.Entities
{
    [Table("tblArraignment")]
    public class Arraignment
    {
        public int Id { get; set; }

        [Index]
        [StringLength(10)]
        public string CaseNumber { get; set; }
        public string Offer { get; set; }
        public string Bond { get; set; }
        public string PoArNotes { get; set; }
    }
}
