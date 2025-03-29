using GamaEdtech.Back.Domain.Common.InterfaceDependency;
using GamaEdtech.Back.Domain.DataAccess.Mappers.Location;
using GamaEdtech.Back.Domain.DataAccess.Repositories.Location;
using GamaEdtech.Back.Domain.DataAccess.Requests.Location;
using GamaEdtech.Back.Domain.DataAccess.Responses.Location;
using GamaEdtech.Back.Domain.Entities.Location.Specifications;
using GamaEdtech.Back.Domain.Entities.Location.Valueobjects;
using static GamaEdtech.Back.Domain.Entities.Location.Location;

namespace GamaEdtech.Back.Domain.Services.Location
{
    public class LocationDomainService(ILocationRepository locationRepository)
        : ILocationDomainService, IScopedDependency
    {
        public async Task<IEnumerable<LocationResponse>> GetLocationByDynamicFilter(GetLocationByDynamicFilterRequest req, CancellationToken cancellationToken)
        {
            var locations = await locationRepository.ListAsync
                (new GetLocationByDynamicFilterSpecification(req), cancellationToken);
            return locations.MapToResponse();
        }
        public async Task<LocationResponse> CreateRegionLocation(CreateLocationRequest req, CancellationToken cancellationToken)
        {
            var parent = await BuildParentRegionLocation(req, cancellationToken);
            var coordinates = new GeographicCoordinates(req.Latitude, req.Longitude);

            var existsLocations = await GetLocationByDynamicFilter(new GetLocationByDynamicFilterRequest
            {
                //Coordinates = coordinates,
                //Radius = 2000
            }, cancellationToken);

            if (existsLocations.Any())
            {
                return existsLocations.First();
            }

            var newLocation = Create(req.Title, req.LatinTitle, req.Code, req.LocationType,
               coordinates, parent);

            newLocation.CaptureOriginalValues();
            try
            {
                var result = await locationRepository.AddAsync(newLocation, cancellationToken);
            }
            catch (Exception e)
            {

                throw;
            }
            return newLocation.MapToResponse();
        }

        private async Task<Entities.Location.Location?> BuildParentRegionLocation(CreateLocationRequest createLocationRequest, CancellationToken cancellationToken)
        {
            Entities.Location.Location? country = null;
            Entities.Location.Location? state = null;
            Entities.Location.Location? city = null;

            if (createLocationRequest.CountryLocationId.HasValue)
            {
                country = (await locationRepository.FirstOrDefaultAsync(new GetLocationByDynamicFilterSpecification
                (
                    new GetLocationByDynamicFilterRequest
                    {
                        LocationId = createLocationRequest.CountryLocationId
                    }
                ), cancellationToken));
            }

            if (createLocationRequest.StateLocationId.HasValue)
            {
                if (country == null)
                    throw new InvalidOperationException("برای ساخت State، Country باید موجود باشد.");

                state = (await locationRepository.FirstOrDefaultAsync(new GetLocationByDynamicFilterSpecification
                (
                    new GetLocationByDynamicFilterRequest
                    {
                        LocationId = createLocationRequest.StateLocationId
                    }
                ), cancellationToken));
            }

            if (createLocationRequest.CityLocationId.HasValue)
            {
                if (state == null)
                    throw new InvalidOperationException("برای ساخت City، State باید موجود باشد.");

                city = (await locationRepository.FirstOrDefaultAsync(new GetLocationByDynamicFilterSpecification
                (
                    new GetLocationByDynamicFilterRequest
                    {
                        LocationId = createLocationRequest.CityLocationId
                    }
                ), cancellationToken));
            }

            return city ?? state ?? country ?? null;
        }
    }
}