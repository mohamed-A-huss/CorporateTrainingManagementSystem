using CorporateTrainingManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace CorporateTrainingManagementSystem.DataAccess.Configurations
{
    public class LessonProgressConfiguration : IEntityTypeConfiguration<LessonProgress>
    {
        public void Configure(EntityTypeBuilder<LessonProgress> builder)
        {
            builder
                .HasOne(lp => lp.User)
                .WithMany(u => u.LessonProgresses)
                .HasForeignKey(lp => lp.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasIndex(lp => new { lp.UserId, lp.LessonId }) 
                .IsUnique();
        }
    
    }
}
