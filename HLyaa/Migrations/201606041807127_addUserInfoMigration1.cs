namespace HLyaa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addUserInfoMigration1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserInfoes", "Age", c => c.Int());
            AlterColumn("dbo.UserInfoes", "BirthdayDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserInfoes", "BirthdayDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.UserInfoes", "Age", c => c.Int(nullable: false));
        }
    }
}
