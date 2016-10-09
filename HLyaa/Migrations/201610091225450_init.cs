namespace HLyaa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DebtParts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Part = c.Double(),
                        Summ = c.Double(nullable: false),
                        GlobalFlag = c.Boolean(nullable: false),
                        UserId = c.Int(),
                        EventId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Events", t => t.EventId)
                .ForeignKey("dbo.UserInfoes", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.EventId);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        GodDebt = c.Boolean(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        ReporterId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserInfoes", t => t.ReporterId)
                .Index(t => t.ReporterId);
            
            CreateTable(
                "dbo.UserInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Age = c.Int(),
                        BirthdayDate = c.DateTime(),
                        Name = c.String(),
                        Nick = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Summ = c.Double(nullable: false),
                        CompleteFlag = c.Boolean(nullable: false),
                        UserId = c.Int(),
                        DebtId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DebtParts", t => t.DebtId, cascadeDelete: true)
                .ForeignKey("dbo.UserInfoes", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.DebtId);
            
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
            
            CreateTable(
                "dbo.EventTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        UserInfo_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserInfoes", t => t.UserInfo_Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.UserInfo_Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "UserInfo_Id", "dbo.UserInfoes");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.UserEventTypes", "UserId", "dbo.UserInfoes");
            DropForeignKey("dbo.UserEventTypes", "EventTypeId", "dbo.EventTypes");
            DropForeignKey("dbo.Payments", "UserId", "dbo.UserInfoes");
            DropForeignKey("dbo.Payments", "DebtId", "dbo.DebtParts");
            DropForeignKey("dbo.Events", "ReporterId", "dbo.UserInfoes");
            DropForeignKey("dbo.DebtParts", "UserId", "dbo.UserInfoes");
            DropForeignKey("dbo.DebtParts", "EventId", "dbo.Events");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "UserInfo_Id" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.UserEventTypes", new[] { "EventTypeId" });
            DropIndex("dbo.UserEventTypes", new[] { "UserId" });
            DropIndex("dbo.Payments", new[] { "DebtId" });
            DropIndex("dbo.Payments", new[] { "UserId" });
            DropIndex("dbo.Events", new[] { "ReporterId" });
            DropIndex("dbo.DebtParts", new[] { "EventId" });
            DropIndex("dbo.DebtParts", new[] { "UserId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.EventTypes");
            DropTable("dbo.UserEventTypes");
            DropTable("dbo.Payments");
            DropTable("dbo.UserInfoes");
            DropTable("dbo.Events");
            DropTable("dbo.DebtParts");
        }
    }
}
