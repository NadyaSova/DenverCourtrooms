namespace CourtRoomsDataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldsToCalendarCase : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tblCalendarCase", "HearingName", c => c.String(unicode: false));
            AddColumn("dbo.tblCalendarCase", "Defendant", c => c.String(unicode: false));
            AddColumn("dbo.tblCalendarCase", "Disposition", c => c.String(unicode: false));
            AddColumn("dbo.tblCalendarCase", "NextCourtroom", c => c.String(unicode: false));
            AddColumn("dbo.tblCalendarCase", "NextCourtDate", c => c.DateTime(precision: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tblCalendarCase", "NextCourtDate");
            DropColumn("dbo.tblCalendarCase", "NextCourtroom");
            DropColumn("dbo.tblCalendarCase", "Disposition");
            DropColumn("dbo.tblCalendarCase", "Defendant");
            DropColumn("dbo.tblCalendarCase", "HearingName");
        }
    }
}
