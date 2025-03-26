using GamaEdtech.Back.Application.DTO.SchoolManager;
using GamaEdtech.Back.Domain.Common.InterfaceDependency;
using GamaEdtech.Back.Domain.Common.Utilities;
using GamaEdtech.Back.Domain.DataAccess.Responses.School;
using GamaEdtech.Back.Domain.Entities.School.Valueobjects;
using GamaEdtech.Back.Domain.Services.Location;
using GamaEdtech.Back.Domain.Services.School;

namespace GamaEdtech.Back.Application.Services.ApplicationServices.SchoolApplicationServices
{
    public class SchoolManager
        (
            ISchoolDomainService schoolDomainService,
            ILocationDomainService locationDomainService
        ) : ISchoolManager, IScopedDependency
    {
        public async Task<SchoolResponse> CreateSchool(CreateSchoolDTO createSchoolDTO, CancellationToken cancellationToken)
        {

            var regionLocation = await locationDomainService.CreateRegionLocation(new Domain.DataAccess.Requests.Location.CreateLocationRequest
            {
                Latitude = createSchoolDTO.Latitude,
                Longitude = createSchoolDTO.Longitude,
                LocationType = Domain.Entities.Location.LocationType.Region,
                Title = createSchoolDTO.Name!.HasValue() ? createSchoolDTO.Name! : createSchoolDTO.LocalName!.HasValue() ?
                createSchoolDTO.LocalName! : "NoName",

                CountryLocationId = createSchoolDTO.CountryId,
                StateLocationId = createSchoolDTO.StateId,
                CityLocationId = createSchoolDTO.CityId
            }, cancellationToken);

            return await schoolDomainService.CreateSchool(new Domain.DataAccess.Requests.School.CreateSchoolRequest
            {
                OsmId = createSchoolDTO.OsmId,
                Name = createSchoolDTO.Name,
                LocalName = createSchoolDTO.LocalName,
                SchoolType = createSchoolDTO.SchoolType,
                LocationId = regionLocation.LocationId,

                Email = createSchoolDTO.Email,
                WebSite = createSchoolDTO.WebSite,
                FaxNumber = createSchoolDTO.FaxNumber,

                CountryId = createSchoolDTO.CountryId,
                StateId = createSchoolDTO.StateId,
                CityId = createSchoolDTO.CityId,
                
                Address = createSchoolDTO.Address!.HasValue() ? Address.FromString(createSchoolDTO.Address!) : null,
                LocalAddress = createSchoolDTO.LocalAddress!.HasValue() ? Address.FromString(createSchoolDTO.LocalAddress!) : null,

                Facilities = createSchoolDTO.Facilities,
                PhoneNumber = createSchoolDTO.PhoneNumber,
                Quarter = createSchoolDTO.Quarter,
                ZipCode = createSchoolDTO.ZipCode
            }, cancellationToken);
        }
    }
}