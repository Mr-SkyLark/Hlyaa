namespace HLyaa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EventTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EventTypeUserInfoes",
                c => new
                    {
                        EventType_Id = c.Int(nullable: false),
                        UserInfo_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.EventType_Id, t.UserInfo_Id })
                .ForeignKey("dbo.EventTypes", t => t.EventType_Id, cascadeDelete: true)
                .ForeignKey("dbo.UserInfoes", t => t.UserInfo_Id, cascadeDelete: true)
                .Index(t => t.EventType_Id)
                .Index(t => t.UserInfo_Id);
            
            AddColumn("dbo.DebtParts", "EventType_Id", c => c.Int());
            AddColumn("dbo.Events", "Type_Id", c => c.Int());
            CreateIndex("dbo.DebtParts", "EventType_Id");
            CreateIndex("dbo.Events", "Type_Id");
            AddForeignKey("dbo.DebtParts", "EventType_Id", "dbo.EventTypes", "Id");
            AddForeignKey("dbo.Events", "Type_Id", "dbo.EventTypes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Events", "Type_Id", "dbo.EventTypes");
            DropForeignKey("dbo.EventTypeUserInfoes", "UserInfo_Id", "dbo.UserInfoes");
            DropForeignKey("dbo.EventTypeUserInfoes", "EventType_Id", "dbo.EventTypes");
            DropForeignKey("dbo.DebtParts", "EventType_Id", "dbo.EventTypes");
            DropIndex("dbo.EventTypeUserInfoes", new[] { "UserInfo_Id" });
            DropIndex("dbo.EventTypeUserInfoes", new[] { "EventType_Id" });
            DropIndex("dbo.Events", new[] { "Type_Id" });
            DropIndex("dbo.DebtParts", new[] { "EventType_Id" });
            DropColumn("dbo.Events", "Type_Id");
            DropColumn("dbo.DebtParts", "EventType_Id");
            DropTable("dbo.EventTypeUserInfoes");
            DropTable("dbo.EventTypes");
        }
    }
}
