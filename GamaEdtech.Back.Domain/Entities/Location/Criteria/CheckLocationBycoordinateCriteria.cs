using GamaEdtech.Back.Domain.Common;
using GamaEdtech.Back.Domain.Entities.Location.Valueobjects;
using System.Linq.Expressions;

namespace GamaEdtech.Back.Domain.Entities.Location.Criteria
{
    public class CheckLocationByCoordinateCriteria(GeographicCoordinates? coordinates)
        : CriteriaSpecification<Location>
    {
        public override Expression<Func<Location, bool>> ToExpression()
        {
            if (coordinates! == null!)
            {
                return current => true;
            }

            return current => true;
        }
    }
}
    