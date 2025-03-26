using GamaEdtech.Back.Domain.DataAccess.Responses.Location;
using GamaEdtech.Back.Domain.Entities.School;
using GamaEdtech.Back.Domain.Entities.School.Valueobjects;

namespace GamaEdtech.Back.Domain.DataAccess.Responses.School
{
    public class SchoolResponse
    {
        public string? Name { get; init; }
        public string? LocalName { get; init; }
        public required SchoolType SchoolType { get; init; }

        public Address? Address { get; init; }
        public Address? LocalAddress { get; init; }
        public required LocationResponse Location { get; init; }
        public string? ZipCode { get; init; }

        public string? WebSite { get; init; }
        public string? Quarter { get; init; }
        public string? PhoneNumber { get; init; }
        public string? FaxNumber { get; init; }
        public string? Facilities { get; init; }
        public string? Email { get; init; }

        public double? Distance { get; set; }
        public double? Score { get; set; }
    }
}
