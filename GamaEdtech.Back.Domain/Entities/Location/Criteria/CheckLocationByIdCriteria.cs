using GamaEdtech.Back.Domain.Common;
using GamaEdtech.Back.Domain.Common.Utilities;
using System.Linq.Expressions;

namespace GamaEdtech.Back.Domain.Entities.Location.Criteria
{
    public class CheckLocationByIdCriteria(Guid? Id) : CriteriaSpecification<Location>
    {

        public override Expression<Func<Location, bool>> ToExpression()
        {
            if (Id == null || Id.Value.GuidIsEmpty())
            {
                return current => true;
            }

            return current => current.Id == Id;
        }
    }
}
