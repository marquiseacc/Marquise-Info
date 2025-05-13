namespace Marquise_Web.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOtpRequestLogsAndOtpVerifyLogs : DbMigration
    {
        public override void Up()
        {
            MoveTable(name: "dbo.OtpVerifyLogs", newSchema: "Security");
            DropForeignKey("Security.OtpRequestLogs", "UserId", "Security.Users");
            AddForeignKey("Security.OtpRequestLogs", "UserId", "Security.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("Security.OtpRequestLogs", "UserId", "Security.Users");
            AddForeignKey("Security.OtpRequestLogs", "UserId", "Security.Users", "Id", cascadeDelete: true);
            MoveTable(name: "Security.OtpVerifyLogs", newSchema: "dbo");
        }
    }
}
