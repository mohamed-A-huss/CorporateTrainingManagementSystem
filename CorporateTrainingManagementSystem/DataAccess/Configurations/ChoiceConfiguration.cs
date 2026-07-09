using CorporateTrainingManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CorporateTrainingManagementSystem.Data.Configurations
{
    public class ChoiceConfiguration : IEntityTypeConfiguration<Choice>
    {
        public void Configure(EntityTypeBuilder<Choice> builder)
        {
            builder.HasKey(c => c.ChoiceId);

            builder.Property(c => c.ChoiceText)
                   .IsRequired()
                   .HasMaxLength(300);

            builder.Property(c => c.IsCorrect)
                   .IsRequired();
        }
    }
}