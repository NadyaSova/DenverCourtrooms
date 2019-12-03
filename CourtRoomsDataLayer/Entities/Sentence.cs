using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourtRoomsDataLayer.Entities
{
    [Table("tblSentence")]
    public class Sentence
    {
        public int Id { get; set; }
        public int DefendantId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int? Value { get; set; }
        public string Units { get; set; }
        public DateTime? DueDate { get; set; }
        public string Status { get; set; }

        public Defendant Defendant { get; set; }
        public List<SentenceDetail> SentenceDetails { get; set; }

        public Sentence()
        {
            SentenceDetails = new List<SentenceDetail>();
        }
    }
}
