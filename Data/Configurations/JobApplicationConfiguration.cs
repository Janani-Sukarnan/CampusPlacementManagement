
using CampusPlacementManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampusPlacementManagement.Data.Configurations
{
    public class JobApplicationConfiguration :
        IEntityTypeConfiguration<JobApplication>
    {
        public void Configure(EntityTypeBuilder<JobApplication> builder)
        {
            builder.HasKey(j => j.JobApplicationId);

            builder.Property(j => j.Status)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(j => j.ResumeUrl)
                .HasMaxLength(500);

            builder.Property(j => j.CoverLetter)
                .HasMaxLength(2000);

            builder.HasOne(j => j.Student)
                .WithMany()
                .HasForeignKey(j => j.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(j => j.JobOpening)
                .WithMany()
                .HasForeignKey(j => j.JobOpeningId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(j => new
            {
                j.StudentId,
                j.JobOpeningId
            }).IsUnique();
        }
    }
}
