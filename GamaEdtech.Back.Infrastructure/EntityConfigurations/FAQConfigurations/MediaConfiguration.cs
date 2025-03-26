using GamaEdtech.Back.Domain.Entities.FAQ.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GamaEdtech.Back.Infrastructure.EntityConfigurations.FAQConfigurations
{
    public class MediaConfiguration : IEntityTypeConfiguration<Media>
    {
        public void Configure(EntityTypeBuilder<Media> builder)
        {
            builder.Property(m => m.FileName)
                        .IsRequired()
                        .HasMaxLength(500);

            builder.Property(m => m.ContentType)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(m => m.FileAddress)
            .IsRequired()
            .HasMaxLength(500);

            builder.HasIndex(m => new { m.MediaEntity, m.MediaEntityId });
        }
    }
}