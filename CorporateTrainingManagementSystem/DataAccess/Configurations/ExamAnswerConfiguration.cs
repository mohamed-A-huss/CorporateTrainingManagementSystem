using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CorporateTrainingManagementSystem.DataAccess.Configurations
{
    public class ExamAnswerConfiguration : IEntityTypeConfiguration<ExamAnswer>
    {
        public void Configure(EntityTypeBuilder<ExamAnswer> builder)
        {
            builder.HasKey(e => e.ExamAnswerId);

            builder
                .HasOne(e => e.Attempt)
                .WithMany(a => a.Answers)
                .HasForeignKey(e => e.AttemptId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(e => e.Question)
                .WithMany(q => q.ExamAnswers)
                .HasForeignKey(e => e.QuestionId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(e => e.Choice)
                .WithMany(c => c.ExamAnswers)
                .HasForeignKey(e => e.ChoiceId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
