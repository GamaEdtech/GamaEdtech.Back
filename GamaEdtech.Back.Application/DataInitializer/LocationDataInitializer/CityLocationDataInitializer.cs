using GamaEdtech.Back.Domain.Common.InterfaceDependency;
using GamaEdtech.Back.Domain.DataAccess.Repositories.Location;
using GamaEdtech.Back.Domain.Entities.Location.Valueobjects;

namespace GamaEdtech.Back.Application.DataInitializer.LocationDataInitializer
{
    public class CityLocationDataInitializer(ILocationRepository locationRepository) : IDataInitializer, IScopedDependency
    {
        public int SortNumber { get; init; } = 3;

        public async Task InitializeData()
        {
            if (!await locationRepository.IsExistsByTitleAsync("Isfahan", Domain.Entities.Location.LocationType.City))
            {
                var isfahanLocation = await locationRepository.GetByTitleAsync("Isfahan", Domain.Entities.Location.LocationType.State);
                var location = Domain.Entities.Location.Location.Create
                    ("Isfahan", "Isfahan", "Isfahan City", Domain.Entities.Location.LocationType.City,
                    new GeographicCoordinates(0, 0), isfahanLocation);

                await locationRepository.AddAsync(location);
            }
        }
    }
}