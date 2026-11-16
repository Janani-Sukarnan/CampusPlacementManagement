using CampusPlacementManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CampusPlacementManagement.Data.Configurations
{
    public class InterviewConfiguration :
        IEntityTypeConfiguration<Interview>
    {
        public void Configure(EntityTypeBuilder<Interview> builder)
        {
            builder.HasKey(i => i.InterviewId);

            builder.Property(i => i.InterviewRound)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(i => i.InterviewMode)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(i => i.Location)
                .HasMaxLength(250);

            builder.Property(i => i.MeetingLink)
                .HasMaxLength(500);

            builder.Property(i => i.Feedback)
                .HasMaxLength(2000);

            builder.Property(i => i.Status)
                .HasConversion<int>()
                .IsRequired();

            builder.HasOne(i => i.JobApplication)
                .WithMany()
                .HasForeignKey(i => i.JobApplicationId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(i => i.JobApplicationId);
        }
    }
}