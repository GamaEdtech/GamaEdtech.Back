using GamaEdtech.Back.FAQ.Domain.Entities.FAQCategory;
using GamaEdtech.Back.FAQ.Domain.Entities.FAQCategory.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GamaEdtech.Back.FAQ.Infrastructure.EntityConfigurations.FAQCategoryConfigurations
{
    public class FAQCategoryConfiguration : IEntityTypeConfiguration<FAQCategory>
    {
        public void Configure(EntityTypeBuilder<FAQCategory> builder)
        {
            builder.Property(prop => prop.Title)
                .IsRequired().HasMaxLength(50);

            builder.HasMany(many => many.FAQAndFAQCategories)
                .WithOne(one => one.FAQCategory);


            var converter = new ValueConverter<HierarchyPath, HierarchyId>(
                v => HierarchyId.Parse(v.Value),
                v => new HierarchyPath(v.ToString())
              );

            builder.Property(e => e.HierarchyPath)
                .HasConversion(converter);
        }
    }
}