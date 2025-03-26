using GamaEdtech.Back.Domain.Entities.Location;

namespace GamaEdtech.Back.Domain.DataAccess.Repositories.Location
{
    public interface ILocationRepository : IBaseRepository<Entities.Location.Location>
    {
        Task<bool> IsExistsByTitleAsync(string title, LocationType locationType, CancellationToken cancellationToken = default);
        Task<Entities.Location.Location> GetByTitleAsync(string title, LocationType locationType, CancellationToken cancellationToken = default);
    }
}