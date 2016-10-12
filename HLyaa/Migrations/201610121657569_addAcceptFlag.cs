namespace HLyaa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addAcceptFlag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DebtParts", "Accept", c => c.Boolean(nullable: false));
            AddColumn("dbo.Payments", "Accept", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Payments", "Accept");
            DropColumn("dbo.DebtParts", "Accept");
        }
    }
}
