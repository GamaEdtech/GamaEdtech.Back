using GamaEdtech.Back.Domain.Entities.Location;
using GamaEdtech.Back.Domain.Entities.Location.Valueobjects;

namespace GamaEdtech.Back.Domain.DataAccess.Requests.Location
{
    public class GetLocationByDynamicFilterRequest
    {
        public Guid? LocationId { get; init; }
        public GeographicCoordinates? Coordinates { get; init; }
        public double? Radius { get; init; }
        public string? Title { get; init; }
        public string? LatinTitle { get; init; }
        public string? Code { get; init; }
        public LocationType? LocationType { get; init; }
    }
}