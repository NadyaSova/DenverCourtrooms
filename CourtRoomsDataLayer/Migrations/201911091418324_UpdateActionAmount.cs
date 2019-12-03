namespace CourtRoomsDataLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateActionAmount : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tblAction", "Amount", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tblAction", "Amount", c => c.String(unicode: false));
        }
    }
}
