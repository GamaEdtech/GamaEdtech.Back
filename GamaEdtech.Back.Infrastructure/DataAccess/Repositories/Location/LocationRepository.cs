using GamaEdtech.Back.Domain.Common.InterfaceDependency;
using GamaEdtech.Back.Domain.DataAccess.Repositories.Location;
using GamaEdtech.Back.Domain.Entities.Location;
using GamaEdtech.Back.Domain.Entities.Location.Specifications;
using GamaEdtech.Back.Infrastructure.DbContexts.Sql.SqlServer;

namespace GamaEdtech.Back.Infrastructure.DataAccess.Repositories.Location
{
    public class LocationRepository(ApplicationDbContext dbContext) 
        : BaseRepository<Domain.Entities.Location.Location>(dbContext), ILocationRepository, IScopedDependency
    {
        public Task<bool> IsExistsByTitleAsync(string title, LocationType locationType, CancellationToken cancellationToken = default)
        {
            return AnyAsync(new GetLocationByDynamicFilterSpecification
                (new Domain.DataAccess.Requests.Location.GetLocationByDynamicFilterRequest
                {
                    Title = title,
                    LocationType = locationType,
                }), cancellationToken);
        }

        public Task<Domain.Entities.Location.Location> GetByTitleAsync(string title, LocationType locationType, CancellationToken cancellationToken = default)
        {
            return FirstOrDefaultAsync(new GetLocationByDynamicFilterSpecification
                (new Domain.DataAccess.Requests.Location.GetLocationByDynamicFilterRequest
                {
                    Title = title,
                    LocationType = locationType
                }), cancellationToken);
        }
    }
}