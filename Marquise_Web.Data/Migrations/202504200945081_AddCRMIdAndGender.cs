namespace Marquise_Web.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCRMIdAndGender : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "Gender", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "Gender", c => c.Int(nullable: false));
        }
    }
}
