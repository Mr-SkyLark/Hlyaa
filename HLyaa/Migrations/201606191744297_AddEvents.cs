namespace HLyaa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEvents : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DebtParts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Part = c.Double(nullable: false),
                        Summ = c.Double(nullable: false),
                        UserId = c.Int(),
                        EventId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Events", t => t.EventId)
                .ForeignKey("dbo.UserInfoes", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.EventId);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Summ = c.Double(nullable: false),
                        DebtId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DebtParts", t => t.DebtId)
                .Index(t => t.DebtId);
            
            AddColumn("dbo.Events", "GodDebt", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payments", "DebtId", "dbo.DebtParts");
            DropForeignKey("dbo.DebtParts", "UserId", "dbo.UserInfoes");
            DropForeignKey("dbo.DebtParts", "EventId", "dbo.Events");
            DropIndex("dbo.Payments", new[] { "DebtId" });
            DropIndex("dbo.DebtParts", new[] { "EventId" });
            DropIndex("dbo.DebtParts", new[] { "UserId" });
            DropColumn("dbo.Events", "GodDebt");
            DropTable("dbo.Payments");
            DropTable("dbo.DebtParts");
        }
    }
}
