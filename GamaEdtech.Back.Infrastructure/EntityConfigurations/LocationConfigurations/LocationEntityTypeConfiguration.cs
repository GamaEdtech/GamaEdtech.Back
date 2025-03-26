using GamaEdtech.Back.Domain.Entities.Location;
using GamaEdtech.Back.Infrastructure.EntityConfigurations.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GamaEdtech.Back.Infrastructure.EntityConfigurations.LocationConfigurations
{
    public class LocationEntityTypeConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.Property(prop => prop.Title)
                .HasMaxLength(100).IsRequired();

            builder.Property(prop => prop.LatinTitle)
                .HasMaxLength(100).IsRequired(false);

            builder.Property(prop => prop.Code)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(prop => prop.LocationType)
                .IsRequired();

            builder.Property(prop => prop.LatinTitle)
                .IsRequired(false);

            builder.Property(prop => prop.Coordinates)
                .HasConversion(new CoordinatesConverter())
                .HasColumnType("geometry")
                .IsRequired();

            builder.HasIndex(prop => prop.HierarchyPath);
            builder.HasIndex(prop => prop.LocationType);
            builder.HasIndex(prop => prop.Code).IsUnique(true);

            builder.Property(e => e.HierarchyPath)
                .HasConversion(new HierarchyPathConverter());

            builder.HasMany(many => many.Schools)
                .WithOne(one => one.Location);
        }
    }
}