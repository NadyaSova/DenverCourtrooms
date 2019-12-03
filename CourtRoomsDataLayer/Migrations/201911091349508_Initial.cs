namespace CourtRoomsDataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblDefendant",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LastName = c.String(unicode: false),
                        FirstName = c.String(unicode: false),
                        Mi = c.String(unicode: false),
                        Suffix = c.String(unicode: false),
                        DateOfBirth = c.DateTime(nullable: false, precision: 0),
                        PartyStatus = c.String(unicode: false),
                        Race = c.String(unicode: false),
                        Hair = c.String(unicode: false),
                        Weight = c.Int(),
                        Height = c.Int(),
                        EyeColor = c.String(unicode: false),
                        EyeGlasses = c.String(unicode: false),
                        CaseNumber = c.String(unicode: false),
                        CaseStatus = c.String(unicode: false),
                        CaseType = c.String(unicode: false),
                        ViolationDate = c.DateTime(nullable: false, precision: 0),
                        FiledDate = c.DateTime(nullable: false, precision: 0),
                        Courtroom = c.String(unicode: false),
                        PayAmount = c.Double(nullable: false),
                        Location = c.String(unicode: false),
                        AbNumber = c.Int(),
                        GoNumber = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.tblAction",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DefendantId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false, precision: 0),
                        Description = c.String(unicode: false),
                        JudicialOfficer = c.String(unicode: false),
                        Courtroom = c.String(unicode: false),
                        Dispo = c.String(unicode: false),
                        Amount = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tblDefendant", t => t.DefendantId, cascadeDelete: true)
                .Index(t => t.DefendantId);
            
            CreateTable(
                "dbo.tblAttorney",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DefendantId = c.Int(nullable: false),
                        Name = c.String(unicode: false),
                        Number = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tblDefendant", t => t.DefendantId, cascadeDelete: true)
                .Index(t => t.DefendantId);
            
            CreateTable(
                "dbo.tblBond",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DefendantId = c.Int(nullable: false),
                        Type = c.String(unicode: false),
                        SuretyName = c.String(unicode: false),
                        PowerNumber = c.String(unicode: false),
                        BondNumber = c.Int(),
                        ArrestNumber = c.String(unicode: false),
                        Insurance = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tblDefendant", t => t.DefendantId, cascadeDelete: true)
                .Index(t => t.DefendantId);
            
            CreateTable(
                "dbo.tblBondDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BondId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false, precision: 0),
                        ActionCode = c.String(unicode: false),
                        Amount = c.Double(nullable: false),
                        SoeDate = c.DateTime(precision: 0),
                        RelToParty = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tblBond", t => t.BondId, cascadeDelete: true)
                .Index(t => t.BondId);
            
            CreateTable(
                "dbo.tblCost",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DefendantId = c.Int(nullable: false),
                        Description = c.String(unicode: false),
                        Imposed = c.Double(nullable: false),
                        Suspended = c.Double(nullable: false),
                        CcwpCts = c.Double(nullable: false),
                        Paid = c.Double(nullable: false),
                        Due = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tblDefendant", t => t.DefendantId, cascadeDelete: true)
                .Index(t => t.DefendantId);
            
            CreateTable(
                "dbo.tblSentence",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DefendantId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false, precision: 0),
                        Description = c.String(unicode: false),
                        Value = c.Int(),
                        Units = c.String(unicode: false),
                        DueDate = c.DateTime(precision: 0),
                        Status = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tblDefendant", t => t.DefendantId, cascadeDelete: true)
                .Index(t => t.DefendantId);
            
            CreateTable(
                "dbo.tblSentenceDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SentenceId = c.Int(nullable: false),
                        Number = c.Int(),
                        Description = c.String(unicode: false),
                        Value = c.Int(),
                        Units = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tblSentence", t => t.SentenceId, cascadeDelete: true)
                .Index(t => t.SentenceId);
            
            CreateTable(
                "dbo.tblCharge",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DefendantId = c.Int(nullable: false),
                        Code = c.String(unicode: false),
                        Description = c.String(unicode: false),
                        Points = c.Int(),
                        Disposition = c.String(unicode: false),
                        ClassCode = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.tblDefendant", t => t.DefendantId, cascadeDelete: true)
                .Index(t => t.DefendantId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblCharge", "DefendantId", "dbo.tblDefendant");
            DropForeignKey("dbo.tblSentenceDetail", "SentenceId", "dbo.tblSentence");
            DropForeignKey("dbo.tblSentence", "DefendantId", "dbo.tblDefendant");
            DropForeignKey("dbo.tblCost", "DefendantId", "dbo.tblDefendant");
            DropForeignKey("dbo.tblBond", "DefendantId", "dbo.tblDefendant");
            DropForeignKey("dbo.tblBondDetail", "BondId", "dbo.tblBond");
            DropForeignKey("dbo.tblAttorney", "DefendantId", "dbo.tblDefendant");
            DropForeignKey("dbo.tblAction", "DefendantId", "dbo.tblDefendant");
            DropIndex("dbo.tblCharge", new[] { "DefendantId" });
            DropIndex("dbo.tblSentenceDetail", new[] { "SentenceId" });
            DropIndex("dbo.tblSentence", new[] { "DefendantId" });
            DropIndex("dbo.tblCost", new[] { "DefendantId" });
            DropIndex("dbo.tblBondDetail", new[] { "BondId" });
            DropIndex("dbo.tblBond", new[] { "DefendantId" });
            DropIndex("dbo.tblAttorney", new[] { "DefendantId" });
            DropIndex("dbo.tblAction", new[] { "DefendantId" });
            DropTable("dbo.tblCharge");
            DropTable("dbo.tblSentenceDetail");
            DropTable("dbo.tblSentence");
            DropTable("dbo.tblCost");
            DropTable("dbo.tblBondDetail");
            DropTable("dbo.tblBond");
            DropTable("dbo.tblAttorney");
            DropTable("dbo.tblAction");
            DropTable("dbo.tblDefendant");
        }
    }
}
