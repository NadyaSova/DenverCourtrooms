using CourtRoomsDataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Action = CourtRoomsDataLayer.Entities.Action;

namespace CourtRoomsDataLayer.Context
{
    [DbConfigurationType(typeof(MySql.Data.EntityFramework.MySqlEFConfiguration))]
    public class CourtRoomsContext: DbContext
    {
        public CourtRoomsContext() : base("name=CourtRoomContext") { }
        public DbSet<Defendant> Defendants { get; set; }
        public DbSet<CourtInformation> CourtInformations { get; set; }
        public DbSet<CaseDetail> CaseDetails { get; set; }
        public DbSet<Action> Actions { get; set; }
        public DbSet<Attorney> Attorneys { get; set; }
        public DbSet<Sentence> Sentences { get; set; }
        public DbSet<SentenceDetail> SentenceDetails { get; set; }
        public DbSet<Cost> Costs { get; set; }
        public DbSet<NotFoundCase> NotFoundCases { get; set; }
    }
}
