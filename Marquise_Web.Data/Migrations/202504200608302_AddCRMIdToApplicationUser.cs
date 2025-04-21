namespace Marquise_Web.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCRMIdToApplicationUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "CRMId", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "CRMId");
        }
    }
}
