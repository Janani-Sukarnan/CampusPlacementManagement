using CampusPlacementManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampusPlacementManagement.Data.Configurations
{
    public class JobTargetCollegeConfiguration :
        IEntityTypeConfiguration<JobTargetCollege>
    {
        public void Configure(EntityTypeBuilder<JobTargetCollege> builder)
        {
            builder.HasKey(j => j.JobTargetCollegeId);

            builder.HasOne(j => j.JobOpening)
                .WithMany()
                .HasForeignKey(j => j.JobOpeningId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(j => j.College)
                .WithMany()
                .HasForeignKey(j => j.CollegeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(j => new { j.JobOpeningId, j.CollegeId })
                .IsUnique();
        }
    }
}