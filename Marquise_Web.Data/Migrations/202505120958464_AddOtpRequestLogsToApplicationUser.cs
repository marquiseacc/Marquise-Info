namespace Marquise_Web.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOtpRequestLogsToApplicationUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("Security.OtpRequestLogs", "UserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("Security.OtpRequestLogs", "UserId");
            AddForeignKey("Security.OtpRequestLogs", "UserId", "Security.Users", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("Security.OtpRequestLogs", "UserId", "Security.Users");
            DropIndex("Security.OtpRequestLogs", new[] { "UserId" });
            DropColumn("Security.OtpRequestLogs", "UserId");
        }
    }
}
