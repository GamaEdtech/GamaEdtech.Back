using GamaEdtech.Back.Domain.Entities.School;
namespace GamaEdtech.Back.Application.DTO.SchoolManager
{
    public class CreateSchoolDTO
    {
        public long? OsmId { get; init; }
        public string? Name { get; init; }
        public string? LocalName { get; init; }
        public required SchoolType SchoolType { get; init; }


        public string? Address { get; init; }
        public string? LocalAddress { get; init; }

        public double Latitude { get; init; }
        public double Longitude { get; init; }

        public string? WebSite { get; init; }
        public string? Quarter { get; init; }
        public string? PhoneNumber { get; init; }
        public string? FaxNumber { get; init; }
        public string? Facilities { get; init; }
        public string? Email { get; init; }

        public Guid? CityId { get; init; }
        public Guid? CountryId { get; init; }
        public Guid? StateId { get; init; }
        public string? ZipCode { get; init; }
    }
}