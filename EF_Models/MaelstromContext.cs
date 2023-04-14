using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EF_Models.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EF_Models
{
    public partial class MaelstromContext : IdentityDbContext<AppUser>
    {
        /* public MaelstromContext() { }*/ // may not be needed
        public MaelstromContext(DbContextOptions<MaelstromContext> options) : base(options) { }
        public DbSet<AppUser> AppUsers => Set<AppUser>();
        public DbSet<TestResult> TestResults => Set<TestResult>();
        public DbSet<FishType> FishTypes => Set<FishType>();
        public DbSet<Fish> Fishs => Set<Fish>();
        public DbSet<SiteUser> SiteUsers => Set<SiteUser>();
        public DbSet<SiteType> SiteTypes => Set<SiteType>();
        public DbSet<Site> Sites => Set<Site>();

        //public DbSet<Account> Accounts => Set<Account>();
       
        //public DbSet<AccountStanding> AccountStandings => Set<AccountStanding>();
        //public DbSet<AccountType> AccountTypes => Set<AccountType>();
        //public DbSet<Payment> Payments => Set<Payment>();




       
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        

            //composite key  --------------------------------   * currently broken -- fix in progress *
            //modelBuilder.Entity<SiteUser>()
            //.HasKey(su => new { su.SiteID, su.AppUser.Id});




            //seed data for tests -- currently not in use
            //modelBuilder.ApplyConfiguration(new SeedAccountStandings());
            //modelBuilder.ApplyConfiguration(new SeedAccountTypes());
        }


    }
}
