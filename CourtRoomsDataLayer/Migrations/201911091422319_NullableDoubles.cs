namespace CourtRoomsDataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableDoubles : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tblDefendant", "PayAmount", c => c.Double());
            AlterColumn("dbo.tblBondDetail", "Amount", c => c.Double());
            AlterColumn("dbo.tblCost", "Imposed", c => c.Double());
            AlterColumn("dbo.tblCost", "Suspended", c => c.Double());
            AlterColumn("dbo.tblCost", "CcwpCts", c => c.Double());
            AlterColumn("dbo.tblCost", "Paid", c => c.Double());
            AlterColumn("dbo.tblCost", "Due", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tblCost", "Due", c => c.Double(nullable: false));
            AlterColumn("dbo.tblCost", "Paid", c => c.Double(nullable: false));
            AlterColumn("dbo.tblCost", "CcwpCts", c => c.Double(nullable: false));
            AlterColumn("dbo.tblCost", "Suspended", c => c.Double(nullable: false));
            AlterColumn("dbo.tblCost", "Imposed", c => c.Double(nullable: false));
            AlterColumn("dbo.tblBondDetail", "Amount", c => c.Double(nullable: false));
            AlterColumn("dbo.tblDefendant", "PayAmount", c => c.Double(nullable: false));
        }
    }
}
