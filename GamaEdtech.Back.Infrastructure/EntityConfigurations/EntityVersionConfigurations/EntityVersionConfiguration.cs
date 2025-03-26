using GamaEdtech.Back.Domain.Entities.EntityVersion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GamaEdtech.Back.Infrastructure.EntityConfigurations.EntityVersionConfigurations
{
    public class EntityVersionConfiguration : IEntityTypeConfiguration<EntityVersion>
    {
        public void Configure(EntityTypeBuilder<EntityVersion> builder)
        {
            builder.Property(prop => prop.EntityId)
                .IsRequired();

            builder.Property(prop => prop.UserId)
                .IsRequired();

            builder.Property(prop => prop.EntityType)
                .IsRequired();

            builder.Property(prop => prop.PropertyName)
                .HasMaxLength(200).IsRequired();

            builder.Property(prop => prop.OldValue)
                .IsRequired();

            builder.Property(prop => prop.NewValue)
            .IsRequired();
        }
    }
}