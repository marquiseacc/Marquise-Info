namespace Marquise_Web.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOtpRequestLogsTableToSecuritySchema : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Security.OtpRequestLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PhoneNumber = c.String(),
                        RequestTime = c.DateTime(nullable: false),
                        IPAddress = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("Security.OtpRequestLogs");
        }
    }
}
