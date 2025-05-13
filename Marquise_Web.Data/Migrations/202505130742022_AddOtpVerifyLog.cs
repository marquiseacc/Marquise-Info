namespace Marquise_Web.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOtpVerifyLog : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OtpVerifyLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PhoneNumber = c.String(),
                        TryTime = c.DateTime(nullable: false),
                        IsSuccess = c.Boolean(nullable: false),
                        IPAddress = c.String(),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Security.Users", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OtpVerifyLogs", "UserId", "Security.Users");
            DropIndex("dbo.OtpVerifyLogs", new[] { "UserId" });
            DropTable("dbo.OtpVerifyLogs");
        }
    }
}
