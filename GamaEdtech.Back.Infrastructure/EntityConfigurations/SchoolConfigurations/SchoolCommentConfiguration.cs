using GamaEdtech.Back.Domain.Entities.School.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GamaEdtech.Back.Infrastructure.EntityConfigurations.SchoolConfigurations
{
    public class SchoolCommentConfiguration : IEntityTypeConfiguration<SchoolComment>
    {
        public void Configure(EntityTypeBuilder<SchoolComment> builder)
        {
        }
    }
}
