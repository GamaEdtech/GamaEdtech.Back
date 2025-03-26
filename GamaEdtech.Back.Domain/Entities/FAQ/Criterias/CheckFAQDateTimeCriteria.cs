using GamaEdtech.Back.Domain.Common;
using System.Linq.Expressions;

namespace GamaEdtech.Back.Domain.Entities.FAQ.Criterias
{
    public class CheckFAQDateTimeCriteria(DateTime? fromDateTime, DateTime? toDateTime) : CriteriaSpecification<FAQ>
    {
        private readonly DateTime? _fromDateTime = fromDateTime == default ? DateTime.MinValue : fromDateTime;
        private readonly DateTime? _toDateTime = toDateTime == default ? DateTime.MaxValue : toDateTime;

        public override Expression<Func<FAQ, bool>> ToExpression()
        {
            if (_fromDateTime == default && _toDateTime == default)
            {
                return current => true;
            }

            return current => current.CreateDate >= _fromDateTime && current.CreateDate < _toDateTime;
        }
    }
}