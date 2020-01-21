namespace CourtRoomsDataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateNotFoundCaseToCalendarCase : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.tblNotFoundCase", new[] { "CaseNumber" });
            CreateTable(
                "dbo.tblCalendarCase",
                c => new
                    {
                        CaseNumber = c.String(nullable: false, maxLength: 10, storeType: "nvarchar"),
                        Date = c.DateTime(nullable: false, precision: 0),
                        RoomNumber = c.String(unicode: false),
                        IsFound = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CaseNumber);
            
            DropTable("dbo.tblNotFoundCase");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.tblNotFoundCase",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CaseNumber = c.String(maxLength: 10, storeType: "nvarchar"),
                        Date = c.DateTime(nullable: false, precision: 0),
                        RoomNumber = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("dbo.tblCalendarCase");
            CreateIndex("dbo.tblNotFoundCase", "CaseNumber", unique: true);
        }
    }
}
