using GamaEdtech.Back.Domain.Entities.Location;

namespace GamaEdtech.Back.Domain.DataAccess.Responses.Location
{
    public class LocationResponse
    {
        public Guid LocationId { get; init; }
        public required string Title { get; init; }
        public string? LatinTitle { get; init; }
        public string? Code { get; init; }
        public LocationType LocationType { get; init; }
        public double Latitude { get; init; } 
        public double Longitude { get; init; }
        public List<LocationResponse> Children { get; set; } = [];
    }
}