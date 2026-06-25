using CorporateTrainingManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace CorporateTrainingManagementSystem.DataAccess.Configurations
{
    public class ExamAttemptConfiguration : IEntityTypeConfiguration<ExamAttempt>
    {
        public void Configure(EntityTypeBuilder<ExamAttempt> builder)
        {
            builder.HasKey(e => e.AttemptId);
            builder
                .HasOne(a => a.User)
                .WithMany(u => u.ExamAttempts)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
