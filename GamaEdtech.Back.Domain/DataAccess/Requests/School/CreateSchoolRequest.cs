using GamaEdtech.Back.Domain.Entities.School;
using GamaEdtech.Back.Domain.Entities.School.Valueobjects;

namespace GamaEdtech.Back.Domain.DataAccess.Requests.School
{
    public class CreateSchoolRequest
    {
        public long? OsmId { get; init; }
        public string? Name { get; init; }
        public string? LocalName { get; init; }
        public required SchoolType SchoolType { get; init; }

        public Address? Address { get; init; }
        public string? ZipCode { get; init; }
        public Address? LocalAddress { get; init; }
        public Guid LocationId { get; init; }

        public string? WebSite { get; init; }
        public string? Quarter { get; init; }
        public string? PhoneNumber { get; init; }
        public string? FaxNumber { get; init; }
        public string? Facilities { get; init; }
        public string? Email { get; init; }

        public Guid? CityId { get; init; }
        public Guid? CountryId { get; init; }
        public Guid? StateId { get; init; }
    }
}