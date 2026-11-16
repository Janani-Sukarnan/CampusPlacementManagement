using CampusPlacementManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampusPlacementManagement.Data.Configurations
{
    public class ApplicationStatusHistoryConfiguration :
        IEntityTypeConfiguration<ApplicationStatusHistory>
    {
        public void Configure(
            EntityTypeBuilder<ApplicationStatusHistory> builder)
        {
            builder.HasKey(h => h.ApplicationStatusHistoryId);

            builder.Property(h => h.PreviousStatus)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(h => h.NewStatus)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(h => h.ChangedByUserId)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(h => h.Remarks)
                .HasMaxLength(1000);

            builder.HasOne(h => h.JobApplication)
                .WithMany()
                .HasForeignKey(h => h.JobApplicationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(h => h.ChangedByUser)
                .WithMany()
                .HasForeignKey(h => h.ChangedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(h => h.JobApplicationId);
        }
    }
}