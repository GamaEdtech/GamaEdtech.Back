using GamaEdtech.Back.Domain.Entities.FAQCategory;
using GamaEdtech.Back.Infrastructure.EntityConfigurations.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GamaEdtech.Back.Infrastructure.EntityConfigurations.FAQCategoryConfigurations
{
    public class FAQCategoryConfiguration : IEntityTypeConfiguration<FAQCategory>
    {
        public void Configure(EntityTypeBuilder<FAQCategory> builder)
        {
            builder.Property(prop => prop.Title)
                .IsRequired().HasMaxLength(50);

            builder.HasMany(many => many.FAQAndFAQCategories)
                .WithOne(one => one.FAQCategory);

            builder.Property(e => e.HierarchyPath)
                .HasConversion(new HierarchyPathConverter());
        }
    }
}