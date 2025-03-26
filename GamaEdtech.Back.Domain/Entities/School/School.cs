using GamaEdtech.Back.Domain.Common;
using GamaEdtech.Back.Domain.Entities.School.Aggregates;
using GamaEdtech.Back.Domain.Entities.School.Valueobjects;

namespace GamaEdtech.Back.Domain.Entities.School
{
    public class School : AggregateRoot
    {
        #region Ctors
        private School() {}
        public School(SchoolType schoolType, long? osmId, string? name, Address? address, Guid locationId)
        {
            SchoolType = schoolType;
            OsmId = osmId;
            Name = name ?? "NoName";
            Address = address;
            LocationId = locationId;
        }
        #endregion

        #region Propeties
        public long? OsmId { get; private set; }

        public string Name { get; private set; }
        public string? LocalName { get; private set; }
        public SchoolType SchoolType { get; private set; }

        public Guid LocationId { get; private set; }
        public string? ZipCode { get; private set; }
        public Address? Address { get; private set; }
        public Address? LocalAddress { get; private set; }

        public string? Quarter { get; private set; }
        public string? FaxNumber { get; private set; }
        public string? PhoneNumber { get; private set; }
        public string? Email { get; private set; }
        public string? WebSite { get; private set; }
        public string? Facilities { get; private set; }
        public double? Score { get; private set; }
        #endregion

        #region Relations
        #region ForeignKey
        public virtual Location.Location Location { get; private set; }
        #endregion

        #region ICollection
        private readonly List<SchoolComment> _schoolComments;
        public IReadOnlyCollection<SchoolComment> SchoolComments => _schoolComments;

        private readonly List<SchoolImage> _schoolImages;
        public IReadOnlyCollection<SchoolImage> schoolImages => _schoolImages;
        #endregion
        #endregion

        #region Functionalities
        public static School Create(SchoolType schoolType, long? osmId, string? name, Address? address, Guid locationId)
        {
            return new School(schoolType, osmId, name, address, locationId);
        }
        #endregion

        #region Domain Events

        #endregion
    }

    public enum SchoolType
    {
        Public,
        Private,
        Religious,
        FirstNation,
        PrivateNonProfit,
        Government,
        Community
    }
}