using CampusPlacementManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampusPlacementManagement.Data.Configurations
{
    public class CompanyHRConfiguration : IEntityTypeConfiguration<CompanyHR>
    {
        public void Configure(EntityTypeBuilder<CompanyHR> builder)
        {
            builder.HasKey(x => x.CompanyHRId);

            builder.Property(x => x.UserId)
                .IsRequired();

            builder.Property(x => x.Designation)
                .HasMaxLength(100);

            // Each Identity user can have one CompanyHR profile.
            builder.HasIndex(x => x.UserId)
                .IsUnique();

            // A company can have multiple HR representatives.
            builder.HasOne(x => x.Company)
                .WithMany()
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            // The HR representative's Identity account.
            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // The SuperAdmin who approved the HR representative.
            builder.HasOne(x => x.Approver)
                .WithMany()
                .HasForeignKey(x => x.ApprovedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}