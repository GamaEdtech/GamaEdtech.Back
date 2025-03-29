using GamaEdtech.Back.Domain.Common;
using GamaEdtech.Back.Domain.Entities.FAQCategory.Valueobjects;
using GamaEdtech.Back.Domain.Entities.Location.Valueobjects;
namespace GamaEdtech.Back.Domain.Entities.Location
{
    public class Location : AggregateRoot
    {
        #region Ctors
        private Location() {}
        private Location(string title, string latinTitle, LocationType locationType, GeographicCoordinates coordinates)
        {
            Title = title;
            LatinTitle = latinTitle;
            LocationType = locationType;
            Coordinates = coordinates;
        }
        private Location(string title, string latinTitle, string code, LocationType locationType, GeographicCoordinates coordinates)
        {
            Title = title;
            Code = code;
            LocationType = locationType;
            Coordinates = coordinates;
        }
        #endregion

        #region Propeties
        public string Title { get; private set; }
        public string? LatinTitle { get; private set; }
        public string? Code { get; private set; }
        public LocationType LocationType { get; private set; }
        public GeographicCoordinates? Coordinates { get; private set; }
        public HierarchyPath HierarchyPath { get; private set; }
        #endregion

        #region Relations
        #region ForeignKey
        #endregion

        #region ICollections
        private readonly List<School.School> _schools    ;
        public IReadOnlyCollection<School.School> Schools => _schools;
        #endregion
        #endregion

        #region Functionalities
        public static Location Create(string title, string latinTitle, string code, LocationType locationType, GeographicCoordinates coordinates)
        {
            return new Location(title, latinTitle, code, locationType, coordinates);
        }
        #endregion

        #region Domain Events

        #endregion
    }

    public enum LocationType
    {
        Country,
        State,
        City,
        Region
    }
}