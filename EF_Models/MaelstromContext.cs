using EF_Models.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EF_Models
{
    public partial class MaelstromContext : IdentityDbContext<AppUser>
    {
        public MaelstromContext(DbContextOptions<MaelstromContext> options) : base(options) { } // comment this out if you want to scaffold 
        public DbSet<AppUser> AppUsers => Set<AppUser>();
        public DbSet<TestResult> TestResults => Set<TestResult>();
        public DbSet<FishType> FishTypes => Set<FishType>();
        public DbSet<Fish> Fishs => Set<Fish>();
        public DbSet<SiteUser> SiteUsers => Set<SiteUser>();
        public DbSet<SiteType> SiteTypes => Set<SiteType>();
        public DbSet<Site> Sites => Set<Site>();

        // -------Not Implemented Yet-------
        //public DbSet<Account> Accounts => Set<Account>();
        //public DbSet<AccountStanding> AccountStandings => Set<AccountStanding>();
        //public DbSet<AccountType> AccountTypes => Set<AccountType>();
        //public DbSet<Payment> Payments => Set<Payment>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseSqlServer("Data Source=(localdb)\\\\MSSQLLocalDB;Initial Catalog=aspnet-Maelstrom-544b22ef-322c-4196-b14f-f34b2cf355ec;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);



            //composite key  --------------------------------   * Syntax Example *
            //modelBuilder.Entity<SiteUser>()
            //.HasKey(su => new { su.SiteID, su.AppUser.Id});

            //seed data for tests -- currently not in use
            //modelBuilder.ApplyConfiguration(new SeedAccountStandings());
            //modelBuilder.ApplyConfiguration(new SeedAccountTypes());
        }


    }
}
