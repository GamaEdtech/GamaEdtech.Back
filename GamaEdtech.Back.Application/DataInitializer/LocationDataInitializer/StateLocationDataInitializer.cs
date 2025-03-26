using GamaEdtech.Back.Domain.Common.InterfaceDependency;
using GamaEdtech.Back.Domain.DataAccess.Repositories.Location;
using GamaEdtech.Back.Domain.Entities.Location.Valueobjects;

namespace GamaEdtech.Back.Application.DataInitializer.LocationDataInitializer
{
    public class StateLocationDataInitializer(ILocationRepository locationRepository) : IDataInitializer, IScopedDependency
    {
        public int SortNumber { get; init; } = 2;

        public async Task InitializeData()
        {
            if (!await locationRepository.IsExistsByTitleAsync("Isfahan", Domain.Entities.Location.LocationType.State))
            {
                var iranCountryLocation = await locationRepository.GetByTitleAsync("Iran", Domain.Entities.Location.LocationType.Country);
                var location = Domain.Entities.Location.Location.Create
                    ("Isfahan", "Isfahan", "Isfahan", Domain.Entities.Location.LocationType.State,
                    new GeographicCoordinates(0, 0), iranCountryLocation);

                await locationRepository.AddAsync(location);
            }
        }
    }
}