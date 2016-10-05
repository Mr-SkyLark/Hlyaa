namespace HLyaa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEvents2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DebtParts", "GlobalFlag", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DebtParts", "GlobalFlag");
        }
    }
}
