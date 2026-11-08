using CampusPlacementManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampusPlacementManagement.Data.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasKey(s => s.StudentId);

            builder.Property(s => s.RegistrationNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(s => s.RegistrationNumber)
                .IsUnique();

            builder.Property(s => s.Department)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(s => s.Degree)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.CGPA)
                .HasPrecision(4, 2);

            builder.Property(s => s.ResumeUrl)
                .HasMaxLength(500);

            builder.HasIndex(s => s.UserId)
                .IsUnique();

            builder.HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.College)
                .WithMany()
                .HasForeignKey(s => s.CollegeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}