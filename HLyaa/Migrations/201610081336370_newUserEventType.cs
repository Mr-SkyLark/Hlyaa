namespace HLyaa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newUserEventType : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.UserType", newName: "EventTypeUserInfoes");
            DropForeignKey("dbo.DebtParts", "EventType_Id", "dbo.EventTypes");
            DropForeignKey("dbo.Events", "Type_Id", "dbo.EventTypes");
            DropIndex("dbo.DebtParts", new[] { "EventType_Id" });
            DropIndex("dbo.Events", new[] { "Type_Id" });
            RenameColumn(table: "dbo.EventTypeUserInfoes", name: "TypeId", newName: "EventType_Id");
            RenameColumn(table: "dbo.EventTypeUserInfoes", name: "UserId", newName: "UserInfo_Id");
            RenameIndex(table: "dbo.EventTypeUserInfoes", name: "IX_TypeId", newName: "IX_EventType_Id");
            RenameIndex(table: "dbo.EventTypeUserInfoes", name: "IX_UserId", newName: "IX_UserInfo_Id");
            CreateTable(
                "dbo.UserEventTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Part = c.Double(),
                        UserId = c.Int(),
                        EventTypeId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EventTypes", t => t.EventTypeId)
                .ForeignKey("dbo.UserInfoes", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.EventTypeId);
            
            AlterColumn("dbo.DebtParts", "Part", c => c.Double());
            DropColumn("dbo.DebtParts", "EventType_Id");
            DropColumn("dbo.Events", "Price");
            DropColumn("dbo.Events", "Type_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Events", "Type_Id", c => c.Int());
            AddColumn("dbo.Events", "Price", c => c.Double(nullable: false));
            AddColumn("dbo.DebtParts", "EventType_Id", c => c.Int());
            DropForeignKey("dbo.UserEventTypes", "UserId", "dbo.UserInfoes");
            DropForeignKey("dbo.UserEventTypes", "EventTypeId", "dbo.EventTypes");
            DropIndex("dbo.UserEventTypes", new[] { "EventTypeId" });
            DropIndex("dbo.UserEventTypes", new[] { "UserId" });
            AlterColumn("dbo.DebtParts", "Part", c => c.Double(nullable: false));
            DropTable("dbo.UserEventTypes");
            RenameIndex(table: "dbo.EventTypeUserInfoes", name: "IX_UserInfo_Id", newName: "IX_UserId");
            RenameIndex(table: "dbo.EventTypeUserInfoes", name: "IX_EventType_Id", newName: "IX_TypeId");
            RenameColumn(table: "dbo.EventTypeUserInfoes", name: "UserInfo_Id", newName: "UserId");
            RenameColumn(table: "dbo.EventTypeUserInfoes", name: "EventType_Id", newName: "TypeId");
            CreateIndex("dbo.Events", "Type_Id");
            CreateIndex("dbo.DebtParts", "EventType_Id");
            AddForeignKey("dbo.Events", "Type_Id", "dbo.EventTypes", "Id");
            AddForeignKey("dbo.DebtParts", "EventType_Id", "dbo.EventTypes", "Id");
            RenameTable(name: "dbo.EventTypeUserInfoes", newName: "UserType");
        }
    }
}
