namespace CourtRoomsDataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableDates : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tblDefendant", "DateOfBirth", c => c.DateTime(precision: 0));
            AlterColumn("dbo.tblDefendant", "ViolationDate", c => c.DateTime(precision: 0));
            AlterColumn("dbo.tblDefendant", "FiledDate", c => c.DateTime(precision: 0));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tblDefendant", "FiledDate", c => c.DateTime(nullable: false, precision: 0));
            AlterColumn("dbo.tblDefendant", "ViolationDate", c => c.DateTime(nullable: false, precision: 0));
            AlterColumn("dbo.tblDefendant", "DateOfBirth", c => c.DateTime(nullable: false, precision: 0));
        }
    }
}
