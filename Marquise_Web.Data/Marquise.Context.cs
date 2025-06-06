﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Marquise_Web.Data
{
    using Marquise_Web.Model.DTOs.CRM;
    using Marquise_Web.Model.Entities;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Reflection;

    public partial class Marquise_WebEntities : DbContext
    {
        public Marquise_WebEntities()
            : base("name=Marquise_WebEntities")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<Message> Messages { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("Marquise_WebEntities") // بدون 'name=' کافیه
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public DbSet<OtpRequestLog> OtpRequestLogs { get; set; }
        public DbSet<OtpVerifyLog> OtpVerifyLogs { get; set; }
        public DbSet<Account> Accounts { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>().ToTable("User", "Security");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles", "Security");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRoles", "Security");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins", "Security");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims", "Security");
            modelBuilder.Entity<OtpRequestLog>().ToTable("OtpRequestLogs", "Security");
            modelBuilder.Entity<OtpVerifyLog>().ToTable("OtpVerifyLogs", "Security");            
            modelBuilder.Entity<Account>().ToTable("Accounts", "Security");

            modelBuilder.Entity<OtpRequestLog>()
                .HasRequired(o => o.User)
                .WithMany(u => u.OtpRequestLogs)
                .HasForeignKey(o => o.UserId)
                .WillCascadeOnDelete(false);  

            modelBuilder.Entity<OtpVerifyLog>()
                .HasRequired(v => v.User)
                .WithMany(u => u.OtpVerifyLogs)
                .HasForeignKey(v => v.UserId)
                .WillCascadeOnDelete(false);
        }
    }

}
