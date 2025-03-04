using static System.Data.Entity.Infrastructure.Design.Executor;
using System.Data.Entity;

namespace Marquise_Web.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSiteDataEntities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "SiteData.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Phonenumber = c.String(),
                        Email = c.String(),
                        FilePath = c.String(),
                        Birthday = c.DateTime(),
                        Address = c.String(),
                        Section = c.String(),
                        RegisterDate = c.DateTime(nullable: false),
                        MessageText = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("SiteData.Messages");
        }
    }
}
