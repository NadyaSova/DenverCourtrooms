namespace CourtRoomsDataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCourtInformaion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblCaseDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DefendantId = c.Int(nullable: false),
                        BookingDate = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tblDefendant", t => t.DefendantId, cascadeDelete: true)
                .Index(t => t.DefendantId);
            
            CreateTable(
                "dbo.tblCourtInformation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DefendantId = c.Int(nullable: false),
                        CaseNumber = c.String(unicode: false),
                        CourtDate = c.DateTime(precision: 0),
                        CourtLocation = c.String(unicode: false),
                        CourtRoom = c.String(unicode: false),
                        CourtTime = c.String(unicode: false),
                        CourtStatus = c.String(unicode: false),
                        Bond = c.String(unicode: false),
                        HoldingAgency = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tblDefendant", t => t.DefendantId, cascadeDelete: true)
                .Index(t => t.DefendantId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblCourtInformation", "DefendantId", "dbo.tblDefendant");
            DropForeignKey("dbo.tblCaseDetail", "DefendantId", "dbo.tblDefendant");
            DropIndex("dbo.tblCourtInformation", new[] { "DefendantId" });
            DropIndex("dbo.tblCaseDetail", new[] { "DefendantId" });
            DropTable("dbo.tblCourtInformation");
            DropTable("dbo.tblCaseDetail");
        }
    }
}
