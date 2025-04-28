namespace Marquise_Web.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameIdentityTables : DbMigration
    {
        
            public override void Up()
            {
                // تغییر اسکیمای جداول موجود
                Sql("ALTER SCHEMA Security TRANSFER dbo.AspNetUsers;");
                Sql("EXEC sp_rename 'Security.AspNetUsers', 'Users';");

                Sql("ALTER SCHEMA Security TRANSFER dbo.AspNetRoles;");
                Sql("EXEC sp_rename 'Security.AspNetRoles', 'Roles';");

                Sql("ALTER SCHEMA Security TRANSFER dbo.AspNetUserRoles;");
                Sql("EXEC sp_rename 'Security.AspNetUserRoles', 'UserRoles';");

                Sql("ALTER SCHEMA Security TRANSFER dbo.AspNetUserLogins;");
                Sql("EXEC sp_rename 'Security.AspNetUserLogins', 'UserLogins';");

                Sql("ALTER SCHEMA Security TRANSFER dbo.AspNetUserClaims;");
                Sql("EXEC sp_rename 'Security.AspNetUserClaims', 'UserClaims';");
            }

            public override void Down()
            {
                // اگر خواستی Rollback بکنی، می‌تونی برعکس همین تغییرات رو بنویسی
                Sql("ALTER SCHEMA dbo TRANSFER Security.Users;");
                Sql("EXEC sp_rename 'Security.Users', 'AspNetUsers';");

                Sql("ALTER SCHEMA dbo TRANSFER Security.Roles;");
                Sql("EXEC sp_rename 'Security.Roles', 'AspNetRoles';");

                Sql("ALTER SCHEMA dbo TRANSFER Security.UserRoles;");
                Sql("EXEC sp_rename 'Security.UserRoles', 'AspNetUserRoles';");

                Sql("ALTER SCHEMA dbo TRANSFER Security.UserLogins;");
                Sql("EXEC sp_rename 'Security.UserLogins', 'AspNetUserLogins';");

                Sql("ALTER SCHEMA dbo TRANSFER Security.UserClaims;");
                Sql("EXEC sp_rename 'Security.UserClaims', 'AspNetUserClaims';");
            }
        }

    }
