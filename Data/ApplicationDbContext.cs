
using CampusPlacementManagement.Data.Configurations;
using CampusPlacementManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace CampusPlacementManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Application tables
        public DbSet<College> Colleges { get; set; }
        public DbSet<CompanyHR> CompanyHRs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new PlacementOfficerConfiguration());
            builder.ApplyConfiguration(new CompanyHRConfiguration());
            builder.ApplyConfiguration(new CompanyConfiguration());
            builder.ApplyConfiguration(new CollegeConfiguration());
        }
    }
}