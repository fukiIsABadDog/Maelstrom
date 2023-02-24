using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EF_Models.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EF_Models
{
    public partial class MaelstromContext : DbContext
    {
        public MaelstromContext() { } // may not be needed
        public MaelstromContext(DbContextOptions<MaelstromContext> options) : base(options) { }
        public DbSet<User> Users => Set<User>();
        public DbSet<TestResult> TestResults => Set<TestResult>();
        public DbSet<FishType> FishTypes => Set<FishType>();
        public DbSet<Fish> Fishs => Set<Fish>();
        public DbSet<SiteUser> SiteUsers => Set<SiteUser>();
        public DbSet<SiteType> SiteType => Set<SiteType>();
        public DbSet<Site> Site => Set<Site>();

        public DbSet<Account> Accounts => Set<Account>();
       
        public DbSet<AccountStanding> AccountStandings => Set<AccountStanding>();
        public DbSet<AccountType> AccountTypes => Set<AccountType>();
        public DbSet<Payment> Payments => Set<Payment>();




        //Dont forget to add credentials here!
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseSqlServer("Server=DESKTOP-Q2BNFVU;DataBase=Maelstrom;Integrated Security=True;Connection Timeout=30;Encrypt=False;TrustServerCertificate=false;");

                optionsBuilder.UseSqlServer("Server=dotnet.reynolds.edu;DataBase=Maelstrom;User Id=Maelstrom;Password=$Camero7");

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           

            //composite key
            modelBuilder.Entity<SiteUser>()
            .HasKey(su => new { su.SiteID, su.UserID });

            //seed data for tests
            modelBuilder.ApplyConfiguration(new SeedAccountStandings());
            modelBuilder.ApplyConfiguration(new SeedAccountTypes());
        }


    }
}
