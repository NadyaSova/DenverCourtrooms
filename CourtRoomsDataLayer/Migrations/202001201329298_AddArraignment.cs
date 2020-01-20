namespace CourtRoomsDataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddArraignment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblArraignment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CaseNumber = c.String(maxLength: 10, storeType: "nvarchar"),
                        Offer = c.String(unicode: false),
                        Bond = c.String(unicode: false),
                        PoArNotes = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.CaseNumber);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.tblArraignment", new[] { "CaseNumber" });
            DropTable("dbo.tblArraignment");
        }
    }
}
