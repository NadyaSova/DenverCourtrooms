using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourtRoomsDataLayer.Entities
{
    [Table("tblNotFoundCase")]
    public class NotFoundCase
    {
        public int Id { get; set; }
        [Index(IsClustered = false, IsUnique = true)]
        [StringLength(10)]
        public string CaseNumber { get; set; }
        public DateTime Date { get; set; }
        public string RoomNumber { get; set; }
    }
}
