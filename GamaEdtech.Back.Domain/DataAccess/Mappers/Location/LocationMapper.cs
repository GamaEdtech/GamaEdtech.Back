using GamaEdtech.Back.Domain.DataAccess.Responses.Location;
namespace GamaEdtech.Back.Domain.DataAccess.Mappers.Location
{
    public static class LocationMapper
    {
        public static IEnumerable<LocationResponse> MapToResponse(this List<Entities.Location.Location> locations)
        {
            return locations.Select(location => 
            {
                return new LocationResponse
                {
                    LocationId = location.Id,
                    Code = location.Code,
                    LatinTitle = location.LatinTitle,
                    Latitude = location.Coordinates.Latitude,
                    Longitude = location.Coordinates.Longitude,
                    LocationType = location.LocationType,
                    Title = location.Title
                };
            });
        }

        public static LocationResponse MapToResponse(this Entities.Location.Location location)
        {
            return new LocationResponse
            {
                LocationId = location.Id,
                Code = location.Code,
                LatinTitle = location.LatinTitle,
                Latitude = location.Coordinates.Latitude,
                Longitude = location.Coordinates.Longitude,
                LocationType = location.LocationType,
                Title = location.Title
            };
        }
    }
}