using GamaEdtech.Back.Domain.Entities.FAQ.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GamaEdtech.Back.Infrastructure.EntityConfigurations.FAQConfigurations
{
    public class FAQAndFAQCategoryEntityConfiguration : IEntityTypeConfiguration<FAQAndFAQCategory>
    {
        public void Configure(EntityTypeBuilder<FAQAndFAQCategory> builder)
        {
            builder.HasOne(one => one.FAQ)
                .WithMany(many => many.FAQAndFAQCategories)
                .HasForeignKey(fk => fk.FAQId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(one => one.FAQCategory)
                .WithMany(many => many.FAQAndFAQCategories)
                .HasForeignKey(fk => fk.FAQCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
