
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
        public DbSet<Student> Students { get; set; }
        public DbSet<JobOpening> JobOpenings { get; set; }
        public DbSet<JobTargetCollege> JobTargetColleges { get; set; }
        public DbSet<RecruitmentDrive> RecruitmentDrives { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new PlacementOfficerConfiguration());
            builder.ApplyConfiguration(new CompanyHRConfiguration());
            builder.ApplyConfiguration(new CompanyConfiguration());
            builder.ApplyConfiguration(new CollegeConfiguration());
            builder.ApplyConfiguration(new StudentConfiguration());
            builder.ApplyConfiguration(new JobOpeningConfiguration());
            builder.ApplyConfiguration(new JobTargetCollegeConfiguration());
            builder.ApplyConfiguration(new RecruitmentDriveConfiguration());
            builder.ApplyConfiguration(new JobApplicationConfiguration());
        }
    }
}