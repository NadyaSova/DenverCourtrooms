using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourtRoomsDataLayer.Entities
{
    [Table("tblAction")]
    public class Action
    {
        public int Id { get; set; }
        public int DefendantId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string JudicialOfficer { get; set; }
        public string Courtroom { get; set; }
        public string Dispo { get; set; }
        public double? Amount { get; set; }

        public Defendant Defendant { get; set; }
    }
}
