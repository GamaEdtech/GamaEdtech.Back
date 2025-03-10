using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GamaEdtech.Back.FAQ.Infrastructure.EntityConfigurations.FAQConfigurations
{
    public class FAQConfiguration : IEntityTypeConfiguration<Domain.Entities.FAQ.FAQ>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.FAQ.FAQ> builder)
        {
            builder.HasMany(one => one.FAQAndFAQCategories)
                .WithOne(many => many.FAQ);
        }
    }
}
