using GamaEdtech.Back.Domain.DataAccess.Mappers.Location;
using GamaEdtech.Back.Domain.DataAccess.Responses.School;
using GamaEdtech.Back.Domain.Entities.School;

namespace GamaEdtech.Back.Domain.DataAccess.Mappers.SchoolMappers
{
    public static class SchoolMapperResponse
    {
        public static SchoolResponse MapToResponse(this School school)
        {
            return new SchoolResponse
            {
                Name = school.Name,
                PhoneNumber = school.PhoneNumber,
                Quarter = school.Quarter,
                SchoolType = school.SchoolType,
                WebSite = school.WebSite,
                ZipCode = school.ZipCode,
                Location = school.Location.MapToResponse(),
                Email = school.Email,
                Address = school.Address,
                LocalAddress = school.LocalAddress,
                FaxNumber = school.FaxNumber,
                Facilities = school.Facilities,
                LocalName = school.LocalName,
            };
        }
    }
}