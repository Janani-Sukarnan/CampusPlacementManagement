
using CampusPlacementManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampusPlacementManagement.Data.Configurations
{
    public class RecruitmentDriveConfiguration :
        IEntityTypeConfiguration<RecruitmentDrive>
    {
        public void Configure(EntityTypeBuilder<RecruitmentDrive> builder)
        {
            builder.HasKey(r => r.RecruitmentDriveId);

            builder.Property(r => r.DriveName)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(r => r.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(r => r.Venue)
                .HasMaxLength(200);

            builder.HasOne(r => r.Company)
                .WithMany()
                .HasForeignKey(r => r.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
