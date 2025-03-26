using GamaEdtech.Back.Domain.Entities.FAQCategory.Valueobjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore;

namespace GamaEdtech.Back.Infrastructure.EntityConfigurations.Converters
{
    public class HierarchyPathConverter : ValueConverter<HierarchyPath, HierarchyId>
    {
        public HierarchyPathConverter() : base(
                v => HierarchyId.Parse(v.Value),
                v => new HierarchyPath(v.ToString())
           )
        { }
    }
}