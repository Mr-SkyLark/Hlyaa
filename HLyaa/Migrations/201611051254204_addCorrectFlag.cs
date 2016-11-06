namespace HLyaa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCorrectFlag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "IsCorrect", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "IsCorrect");
        }
    }
}
