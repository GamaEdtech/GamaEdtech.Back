using GamaEdtech.Back.Domain.Common;
using GamaEdtech.Back.Domain.Common.Utilities;
using System.Linq.Expressions;

namespace GamaEdtech.Back.Domain.Entities.Location.Criteria
{
    public class CheckLocationByCodeCriteria(string? Code) : CriteriaSpecification<Location>
    {
        public override Expression<Func<Location, bool>> ToExpression()
        {
            if (Code == null || !Code.HasValue())
            {
                return current => true;
            }
            return current => current.Code == Code;
        }
    }
}