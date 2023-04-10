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




        //Dont forget to add credentials here!
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseSqlServer("Server=dotnet.reynolds.edu;DataBase=Maelstrom;User a=Maelstrom;Password=$Camero7");
                //optionsBuilder.UseSqlServer("Server=(localdb)mssqllocaldb;Database=aspnet-Maelstrom-544b22ef-322c-4196-b14f-f34b2cf355ec;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<AppUser>(b =>
            //{
            //    
            //    b.HasMany(e => e.Claims)
            //        .WithOne()
            //        .HasForeignKey(uc => uc.UserId)
            //        .IsRequired();
            //});

            //composite key  --------------------------------   * currently broken -- fix in progress *
            //modelBuilder.Entity<SiteUser>()
            //.HasKey(su => new { su.SiteID, su.AppUser.Id});




            //seed data for tests
            //modelBuilder.ApplyConfiguration(new SeedAccountStandings());
            //modelBuilder.ApplyConfiguration(new SeedAccountTypes());
        }


    }
}
