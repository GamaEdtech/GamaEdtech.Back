using GamaEdtech.Back.Domain.Common.InterfaceDependency;
using GamaEdtech.Back.Domain.DataAccess.Mappers.SchoolMappers;
using GamaEdtech.Back.Domain.DataAccess.Repositories.School;
using GamaEdtech.Back.Domain.DataAccess.Requests.School;
using GamaEdtech.Back.Domain.DataAccess.Responses.School;

namespace GamaEdtech.Back.Domain.Services.School
{
    public class SchoolDomainService(ISchoolRepository schoolRepository) : ISchoolDomainService, IScopedDependency
    {
        public async Task<SchoolResponse> CreateSchool(CreateSchoolRequest createSchoolRequest, CancellationToken cancellationToken)
        {
            var school = Entities.School.School.Create(createSchoolRequest.SchoolType,
                createSchoolRequest.OsmId, createSchoolRequest.Name,
                createSchoolRequest.Address, createSchoolRequest.LocationId);

            await schoolRepository.AddAsync(school, cancellationToken);
            return school.MapToResponse();
        }
    }   
}