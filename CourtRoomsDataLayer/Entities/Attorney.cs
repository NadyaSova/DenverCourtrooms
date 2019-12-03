using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourtRoomsDataLayer.Entities
{
    [Table("tblAttorney")]
    public class Attorney
    {
        public int Id { get; set; }
        public int DefendantId { get; set; }
        public string Name { get; set; }
        public int? Number { get; set; }

        public Defendant Defendant { get; set; }
    }
}
