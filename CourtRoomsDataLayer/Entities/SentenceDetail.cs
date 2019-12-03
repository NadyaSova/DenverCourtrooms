using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourtRoomsDataLayer.Entities
{
    [Table("tblSentenceDetail")]
    public class SentenceDetail
    {
        public int Id { get; set; }
        public int SentenceId { get; set; }
        public int? Number { get; set; }
        public string Description { get; set; }
        public int? Value { get; set; }
        public string Units { get; set; }

        public Sentence Sentence { get; set; }
    }
}
