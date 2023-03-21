using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using Database.Contexts.Models;

namespace Database.Context
{
    public class MyDbContext : DbContext , IDbContext
    {

        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StaffMobile>().HasMany(x => x.StaffMobileTokens).WithOne(x => x.StaffMobile);
            modelBuilder.Entity<StaffMobile>().HasMany(x => x.GPSPositions).WithOne(x => x.StaffMobile);
            modelBuilder.Entity<StaffMobile>().ToTable("STAFF_MOBILE");
            modelBuilder.Entity<StaffMobileToken>().ToTable("STAFF_MOBILE_TOKEN");

            
            modelBuilder.Entity<GPSPosition>().ToTable("GPS_POSITION");
        }

        public async Task SaveChangesAsync()
        {
            await base.SaveChangesAsync();
        }

        public virtual DbSet<StaffMobile> StaffMobiles { get; set; }
        public virtual DbSet<StaffMobileToken> StaffMobileTokens { get; set; }
        public virtual DbSet<GPSPosition> GPSPositions { get; set; }
    }
}
