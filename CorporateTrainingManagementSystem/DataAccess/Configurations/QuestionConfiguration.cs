using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CorporateTrainingManagementSystem.DataAccess.Configurations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasMany(q => q.Choices)
               .WithOne(c => c.Question)
               .HasForeignKey(c => c.QuestionId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
