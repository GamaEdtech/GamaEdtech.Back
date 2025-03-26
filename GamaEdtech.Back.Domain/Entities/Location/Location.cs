using GamaEdtech.Back.Domain.Common;
using GamaEdtech.Back.Domain.Entities.FAQCategory.Valueobjects;
using GamaEdtech.Back.Domain.Entities.Location.Valueobjects;
namespace GamaEdtech.Back.Domain.Entities.Location
{
    public class Location : AggregateRoot
    {
        #region Ctors
        private Location() {}
        private Location(string title, string? latinTitle, string? code, LocationType locationType, GeographicCoordinates coordinates, HierarchyPath hierarchyPath)
        {
            Title = title;
            LatinTitle = latinTitle;
            Code = code;
            LocationType = locationType;
            Coordinates = coordinates;
            HierarchyPath = hierarchyPath;
        }
        #endregion

        #region Propeties
        public string Title { get; private set; }
        public string? LatinTitle { get; private set; }
        public string? Code { get; private set; }
        public LocationType LocationType { get; private set; }
        public GeographicCoordinates Coordinates { get; private set; }
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
        public static Location Create(
        string title,
        string? latinTitle,
        string? code,
        LocationType locationType,
        GeographicCoordinates coordinates,
        Location? parent = null)
        {
            string defaultSegment = locationType switch
            {
                LocationType.Country => "1",
                LocationType.State => "2",
                LocationType.City => "3",
                LocationType.Region => "4",
                _ => throw new ArgumentException("location type is incorrect", nameof(locationType))
            };

            HierarchyPath newPath;

            if (parent == null)
            {
                if (locationType != LocationType.Country)
                    throw new InvalidOperationException("لوکیشن بدون والد باید از نوع Country باشد.");

                newPath = HierarchyPath.FromString($"/{defaultSegment}/");
            }
            else
            {
                newPath = parent.HierarchyPath.GetDescendant(defaultSegment, null);
            }

            return new Location(title, latinTitle, code, locationType, coordinates, newPath);
        }

        public static List<LocationTree> BuildHierarchyTree(IEnumerable<Location> locations)
        {
            var nodes = locations
                .Select(l => new LocationTree(l))
                .GroupBy(n => n.Location.HierarchyPath.Value)
                .ToDictionary(g => g.Key, g => g.ToList());

            var roots = new List<LocationTree>();

            foreach (var group in nodes.Values)
            {
                foreach (var node in group)
                {
                    var parentPath = GetParentPath(node.Location.HierarchyPath.Value);
                    if (!string.IsNullOrEmpty(parentPath) && nodes.TryGetValue(parentPath, out var parentGroup))
                    {
                        parentGroup.First().Children.Add(node);
                    }
                    else
                    {
                        roots.Add(node);
                    }
                }
            }

            return roots;
        }
        private static string? GetParentPath(string path)
        {
            var trimmed = path.TrimEnd('/');
            int lastIndex = trimmed.LastIndexOf('/');
            if (lastIndex <= 0)
                return null;
            return trimmed[..(lastIndex + 1)];
        }
        #endregion

        #region Domain Events

        #endregion
    }

    #region Enums
    public enum LocationType
    {
        Country,
        State,
        City,
        Region
    }
    #endregion
    public record LocationTree(Location Location)
    {
        public List<LocationTree> Children { get; init; } = [];
    }
}