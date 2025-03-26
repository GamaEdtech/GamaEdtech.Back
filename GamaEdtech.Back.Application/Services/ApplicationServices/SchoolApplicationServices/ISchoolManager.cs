using GamaEdtech.Back.Application.DTO.SchoolManager;
using GamaEdtech.Back.Domain.DataAccess.Responses.School;

namespace GamaEdtech.Back.Application.Services.ApplicationServices.SchoolApplicationServices
{
    public interface ISchoolManager
    {
        Task<SchoolResponse> CreateSchool(CreateSchoolDTO createSchoolDTO, CancellationToken cancellationToken);
    }
}