namespace CourtRoomsDataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNotFoundCase : DbMigration
    {
        public override void Up()
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
                .PrimaryKey(t => t.Id)
                .Index(t => t.CaseNumber, unique: true);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.tblNotFoundCase", new[] { "CaseNumber" });
            DropTable("dbo.tblNotFoundCase");
        }
    }
}
