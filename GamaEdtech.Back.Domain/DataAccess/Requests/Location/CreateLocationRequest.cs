using GamaEdtech.Back.Domain.Entities.Location;

namespace GamaEdtech.Back.Domain.DataAccess.Requests.Location
{
    public record CreateLocationRequest
    {
        public required string Title { get; init; }
        public string? LatinTitle { get; init; }
        public string? Code { get; init; }
        public double Latitude { get; init; }
        public double Longitude { get; init; }
        public required LocationType LocationType { get; init; }

        public Guid? CountryLocationId { get; init; }
        public Guid? StateLocationId { get; init; }
        public Guid? CityLocationId { get; init; }
    }
}