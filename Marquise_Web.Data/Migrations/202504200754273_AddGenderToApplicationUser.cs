namespace Marquise_Web.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddGenderToApplicationUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Gender", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Gender");
        }
    }
}
