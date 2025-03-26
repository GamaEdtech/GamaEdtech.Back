
using GamaEdtech.Back.Domain.Common.InterfaceDependency;
using GamaEdtech.Back.Domain.DataAccess.Repositories.Location;
using GamaEdtech.Back.Domain.Entities.Location.Valueobjects;

namespace GamaEdtech.Back.Application.DataInitializer.LocationDataInitializer
{
    public class CountryLocationDataInitializer(ILocationRepository locationRepository) : IDataInitializer, IScopedDependency
    {
        public int SortNumber { get; init; } = 1;

        public async Task InitializeData()
        {
            if (!await locationRepository.IsExistsByTitleAsync("Iran", Domain.Entities.Location.LocationType.Country))
            {
                var location = Domain.Entities.Location.Location.Create
                    ("Iran", "Iran", "IRN", Domain.Entities.Location.LocationType.Country,
                    new GeographicCoordinates(0, 0), null);

                await locationRepository.AddAsync(location);
            }
        }
    }
}