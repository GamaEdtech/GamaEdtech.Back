using GamaEdtech.Back.Domain.Common;
using GamaEdtech.Back.Domain.Entities.Location.Valueobjects;
using System.Linq.Expressions;

namespace GamaEdtech.Back.Domain.Entities.Location.Criteria
{
    public class CheckLocationByRadiusCriteria(GeographicCoordinates? coordinates, double? radius) : CriteriaSpecification<Location>
    {
        public override Expression<Func<Location, bool>> ToExpression()
        {
            if (coordinates! == null! || radius == null || radius == 0.0)
            {
                return current => true;
            }

            return current => current.Coordinates.Distance(coordinates) < radius;
        }
    }
}