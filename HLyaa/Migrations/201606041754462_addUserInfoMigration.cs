namespace HLyaa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addUserInfoMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserInfoes", "BirthdayDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserInfoes", "BirthdayDate");
        }
    }
}
