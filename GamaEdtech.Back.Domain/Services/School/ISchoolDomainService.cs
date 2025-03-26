using GamaEdtech.Back.Domain.DataAccess.Requests.School;
using GamaEdtech.Back.Domain.DataAccess.Responses.School;

namespace GamaEdtech.Back.Domain.Services.School
{
    public interface ISchoolDomainService
    {
        Task<SchoolResponse> CreateSchool(CreateSchoolRequest createSchoolRequest, CancellationToken cancellationToken);
    }
}