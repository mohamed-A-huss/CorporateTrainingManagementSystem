using CorporateTrainingManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CorporateTrainingManagementSystem.DataAccess.Configurations
{
    public class UserBadgeConfiguration : IEntityTypeConfiguration<UserBadge>
    {
        public void Configure(EntityTypeBuilder<UserBadge> builder)
        {
            builder
                .HasOne(ub => ub.User)
                .WithMany(u => u.UserBadges)
                .HasForeignKey(ub => ub.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasIndex(ub => new { ub.UserId, ub.BadgeId })
                .IsUnique();
        }
    }
}
