using GamaEdtech.Back.Domain.Entities.Location.Valueobjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTopologySuite.Geometries;

namespace GamaEdtech.Back.Infrastructure.EntityConfigurations.Converters
{
    public class CoordinatesConverter(ConverterMappingHints mappingHints = null) : ValueConverter<GeographicCoordinates, Point>(
              coord => coord == null ? null : coord.ToPoint(),
              point => point == null ? null : GeographicCoordinates.FromPoint(point),
              mappingHints)
    { }
}
