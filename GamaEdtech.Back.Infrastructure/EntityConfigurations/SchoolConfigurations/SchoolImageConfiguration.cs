using GamaEdtech.Back.Domain.Entities.School.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GamaEdtech.Back.Infrastructure.EntityConfigurations.SchoolConfigurations
{
    public class SchoolImageConfiguration : IEntityTypeConfiguration<SchoolImage>
    {
        public void Configure(EntityTypeBuilder<SchoolImage> builder)
        {
            builder.HasOne(one => one.School)
                .WithMany(many => many.schoolImages)
                .HasForeignKey(fk => fk.SchoolId);
        }
    }
}
