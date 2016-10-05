namespace HLyaa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEvents1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Payments", "DebtId", "dbo.DebtParts");
            DropIndex("dbo.Payments", new[] { "DebtId" });
            AddColumn("dbo.Payments", "CompleteFlag", c => c.Boolean(nullable: false));
            AddColumn("dbo.Payments", "UserId", c => c.Int());
            AlterColumn("dbo.Payments", "DebtId", c => c.Int(nullable: false));
            CreateIndex("dbo.Payments", "UserId");
            CreateIndex("dbo.Payments", "DebtId");
            AddForeignKey("dbo.Payments", "UserId", "dbo.UserInfoes", "Id");
            AddForeignKey("dbo.Payments", "DebtId", "dbo.DebtParts", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payments", "DebtId", "dbo.DebtParts");
            DropForeignKey("dbo.Payments", "UserId", "dbo.UserInfoes");
            DropIndex("dbo.Payments", new[] { "DebtId" });
            DropIndex("dbo.Payments", new[] { "UserId" });
            AlterColumn("dbo.Payments", "DebtId", c => c.Int());
            DropColumn("dbo.Payments", "UserId");
            DropColumn("dbo.Payments", "CompleteFlag");
            CreateIndex("dbo.Payments", "DebtId");
            AddForeignKey("dbo.Payments", "DebtId", "dbo.DebtParts", "Id");
        }
    }
}
