using GamaEdtech.Back.Domain.Entities.School;
using GamaEdtech.Back.Infrastructure.EntityConfigurations.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GamaEdtech.Back.Infrastructure.EntityConfigurations.SchoolConfigurations
{
    public class SchoolConfiguration : IEntityTypeConfiguration<School>
    {
        public void Configure(EntityTypeBuilder<School> builder)
        {
            builder.Property(prop => prop.OsmId)
                .IsRequired();

            builder.Property(prop => prop.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(prop => prop.LocalName)
               .HasMaxLength(100)
               .IsRequired();

            builder.Property(prop => prop.SchoolType)
             .IsRequired();

            builder.Property(prop => prop.ZipCode)
            .HasMaxLength(50)
            .IsRequired(false);

            builder.Property(prop => prop.Quarter)
            .HasMaxLength(200)
            .IsRequired(false);

            builder.Property(prop => prop.FaxNumber)
            .HasMaxLength(100)
            .IsRequired(false);

            builder.Property(prop => prop.PhoneNumber)
             .HasMaxLength(20)
             .IsRequired(false);

            builder.Property(prop => prop.Email)
             .HasMaxLength(100)
             .IsRequired(false);

            builder.Property(prop => prop.WebSite)
             .HasMaxLength(100)
             .IsRequired(false);

            builder.Property(prop => prop.Facilities)
            .HasMaxLength(100)
            .IsRequired(false);


            builder.Property(prop => prop.Address)
                .IsRequired()
                .HasConversion(new AddressConverter());

            builder.Property(prop => prop.LocalAddress)
                .IsRequired(false).HasConversion(new AddressConverter());


            builder.HasOne(one => one.Location)
                .WithMany(many => many.Schools)
                .HasForeignKey(fk => fk.LocationId);

            builder.HasMany(many => many.schoolImages)
                .WithOne(one => one.School);
        }
    }
}
