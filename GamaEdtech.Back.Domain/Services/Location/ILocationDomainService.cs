using GamaEdtech.Back.Domain.DataAccess.Requests.Location;
using GamaEdtech.Back.Domain.DataAccess.Responses.Location;

namespace GamaEdtech.Back.Domain.Services.Location
{
    public interface ILocationDomainService
    {
        Task<LocationResponse> CreateRegionLocation(CreateLocationRequest createLocationRequest, CancellationToken cancellationToken);
    }
}