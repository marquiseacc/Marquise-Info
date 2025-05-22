namespace Marquise_Web.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Security.Accounts",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 50),
                        Name = c.String(maxLength: 100),
                        CrmAccountId = c.String(maxLength: 50),
                        UserId = c.String(maxLength: 128),
                        CrmParentId = c.String(),
                        ParentId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Security.Accounts", t => t.ParentId)
                .ForeignKey("Security.User", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "Security.User",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FullName = c.String(),
                        OtpCode = c.String(),
                        OtpExpiration = c.DateTime(),
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
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "Security.UserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Security.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "Security.UserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("Security.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "Security.OtpRequestLogs",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 50),
                        PhoneNumber = c.String(maxLength: 50),
                        RequestTime = c.DateTime(nullable: false),
                        IPAddress = c.String(maxLength: 100),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Security.User", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "Security.OtpVerifyLogs",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 50),
                        PhoneNumber = c.String(maxLength: 50),
                        TryTime = c.DateTime(nullable: false),
                        IsSuccess = c.Boolean(nullable: false),
                        IPAddress = c.String(maxLength: 100),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Security.User", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "Security.UserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("Security.User", t => t.UserId, cascadeDelete: true)
                .ForeignKey("Security.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "Security.Roles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("Security.UserRoles", "RoleId", "Security.Roles");
            DropForeignKey("Security.Accounts", "UserId", "Security.User");
            DropForeignKey("Security.UserRoles", "UserId", "Security.User");
            DropForeignKey("Security.OtpVerifyLogs", "UserId", "Security.User");
            DropForeignKey("Security.OtpRequestLogs", "UserId", "Security.User");
            DropForeignKey("Security.UserLogins", "UserId", "Security.User");
            DropForeignKey("Security.UserClaims", "UserId", "Security.User");
            DropForeignKey("Security.Accounts", "ParentId", "Security.Accounts");
            DropIndex("Security.Roles", "RoleNameIndex");
            DropIndex("Security.UserRoles", new[] { "RoleId" });
            DropIndex("Security.UserRoles", new[] { "UserId" });
            DropIndex("Security.OtpVerifyLogs", new[] { "UserId" });
            DropIndex("Security.OtpRequestLogs", new[] { "UserId" });
            DropIndex("Security.UserLogins", new[] { "UserId" });
            DropIndex("Security.UserClaims", new[] { "UserId" });
            DropIndex("Security.User", "UserNameIndex");
            DropIndex("Security.Accounts", new[] { "ParentId" });
            DropIndex("Security.Accounts", new[] { "UserId" });
            DropTable("Security.Roles");
            DropTable("Security.UserRoles");
            DropTable("Security.OtpVerifyLogs");
            DropTable("Security.OtpRequestLogs");
            DropTable("Security.UserLogins");
            DropTable("Security.UserClaims");
            DropTable("Security.User");
            DropTable("Security.Accounts");
        }
    }
}
