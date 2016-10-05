namespace HLyaa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class toPostgressMigration : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.EventTypeUserInfoes", newName: "UserType");
            RenameColumn(table: "dbo.UserType", name: "EventType_Id", newName: "TypeId");
            RenameColumn(table: "dbo.UserType", name: "UserInfo_Id", newName: "UserId");
            //RenameIndex(table: "dbo.UserType", name: "IX_EventType_Id", newName: "IX_TypeId");
            //RenameIndex(table: "dbo.UserType", name: "IX_UserInfo_Id", newName: "IX_UserId");
            AlterColumn("dbo.Events", "DateCreated", c => c.DateTime(nullable: false));
            AlterColumn("dbo.UserInfoes", "BirthdayDate", c => c.DateTime());
            AlterColumn("dbo.AspNetUsers", "LockoutEndDateUtc", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "LockoutEndDateUtc", c => c.DateTime());
            AlterColumn("dbo.UserInfoes", "BirthdayDate", c => c.DateTime());
            AlterColumn("dbo.Events", "DateCreated", c => c.DateTime(nullable: false));
            //RenameIndex(table: "dbo.UserType", name: "IX_UserId", newName: "IX_UserInfo_Id");
            //RenameIndex(table: "dbo.UserType", name: "IX_TypeId", newName: "IX_EventType_Id");
            RenameColumn(table: "dbo.UserType", name: "UserId", newName: "UserInfo_Id");
            RenameColumn(table: "dbo.UserType", name: "TypeId", newName: "EventType_Id");
            RenameTable(name: "dbo.UserType", newName: "EventTypeUserInfoes");
        }
    }
}
