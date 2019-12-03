using CourtRoomsDataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourtRoomsDataLayer.Context
{
    [DbConfigurationType(typeof(MySql.Data.EntityFramework.MySqlEFConfiguration))]
    public class CourtRoomsContext: DbContext
    {
        public CourtRoomsContext() : base("name=CourtRoomContext") { }
        public DbSet<Defendant> Defendants { get; set; }
    }
}
