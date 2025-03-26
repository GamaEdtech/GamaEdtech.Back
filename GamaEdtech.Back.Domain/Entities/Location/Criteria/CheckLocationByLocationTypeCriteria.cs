using GamaEdtech.Back.Domain.Common;
using System.Linq.Expressions;

namespace GamaEdtech.Back.Domain.Entities.Location.Criteria
{
    public class CheckLocationByLocationTypeCriteria(LocationType? locationType) : CriteriaSpecification<Location>
    {
        public override Expression<Func<Location, bool>> ToExpression()
        {
            if (locationType == null)
            {
                return current => true;
            }
            return current => current.LocationType == locationType;
        }
    }
}