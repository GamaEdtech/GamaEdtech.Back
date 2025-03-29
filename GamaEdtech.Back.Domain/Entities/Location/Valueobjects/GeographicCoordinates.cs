using NetTopologySuite.Geometries;
using NetTopologySuite;

namespace GamaEdtech.Back.Domain.Entities.Location.Valueobjects
{
    public class GeographicCoordinates(double Latitude, double Longitude) : IEquatable<GeographicCoordinates>
    {
        public double Latitude { get; } = Latitude;
        public double Longitude { get; } = Longitude;

        public Point ToPoint()
        {
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            return geometryFactory.CreatePoint(new Coordinate(Longitude, Latitude));
        }

        public static GeographicCoordinates FromPoint(Point point)
        {
            if (point == null || point.IsEmpty) return null;
            return new GeographicCoordinates(point.Y, point.X);
        }


        public override bool Equals(object obj)
        {
            return Equals(obj as GeographicCoordinates);
        }

        public bool Equals(GeographicCoordinates other)
        {
            if (other is null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return Latitude.Equals(other.Latitude) && Longitude.Equals(other.Longitude);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Latitude, Longitude);
        }

        public static bool operator ==(GeographicCoordinates left, GeographicCoordinates right)
        {
            if (left is null)
                return right is null;

            return left.Equals(right);
        }

        public static bool operator !=(GeographicCoordinates left, GeographicCoordinates right)
        {
            return !(left == right);
        }
    }

}