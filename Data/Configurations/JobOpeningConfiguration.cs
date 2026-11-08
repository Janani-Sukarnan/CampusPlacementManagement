
using CampusPlacementManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampusPlacementManagement.Data.Configurations
{
    public class JobOpeningConfiguration :
        IEntityTypeConfiguration<JobOpening>
    {
        public void Configure(EntityTypeBuilder<JobOpening> builder)
        {
            builder.HasKey(j => j.JobOpeningId);

            builder.Property(j => j.JobTitle)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(j => j.JobDescription)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(j => j.Location)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(j => j.EmploymentType)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(j => j.MinimumCGPA)
                .HasPrecision(4, 2);

            builder.Property(j => j.SalaryRange)
                .HasMaxLength(100);

            builder.Property(j => j.Status)
                .HasConversion<int>();

            builder.HasOne(j => j.Company)
                .WithMany()
                .HasForeignKey(j => j.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
