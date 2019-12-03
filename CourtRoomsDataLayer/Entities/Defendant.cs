using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourtRoomsDataLayer.Entities
{
    [Table("tblDefendant")]
    public class Defendant
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Mi { get; set; }
        public string Suffix { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string PartyStatus { get; set; }
        public string Race { get; set; }
        public string Hair { get; set; }
        public int? Weight { get; set; }
        public int? Height { get; set; }
        public string EyeColor { get; set; }
        public string EyeGlasses { get; set; }

        public string CaseNumber { get; set; }
        public string CaseStatus { get; set; }
        public string CaseType { get; set; }
        public DateTime? ViolationDate { get; set; }
        public DateTime? FiledDate { get; set; }
        public string Courtroom { get; set; }
        public double? PayAmount { get; set; }
        public string Location { get; set; }
        public int? AbNumber  { get; set; }
        public string GoNumber { get; set; }

        public List<Attorney> Attorneys { get; set; }
        public List<Violation> Violations { get; set; }
        public List<Bond> Bonds { get; set; }
        public List<Cost> Costs { get; set; }
        public List<Action> Actions { get; set; }
        public List<Sentence> Sentences { get; set; }


        public Defendant()
        {
            Attorneys = new List<Attorney>();
            Violations = new List<Violation>();
            Costs = new List<Cost>();
            Actions = new List<Action>();
            Sentences = new List<Sentence>();
            Bonds = new List<Bond>();
        }
    }
}
