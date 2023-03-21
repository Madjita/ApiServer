using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Database.Contexts.Models
{
    public interface IDbContext
    {
        public Task SaveChangesAsync();
        public DbSet<StaffMobile> StaffMobiles { get; set; }
        public DbSet<StaffMobileToken> StaffMobileTokens { get; set; }
        public DbSet<GPSPosition> GPSPositions { get; set; }
    }
}