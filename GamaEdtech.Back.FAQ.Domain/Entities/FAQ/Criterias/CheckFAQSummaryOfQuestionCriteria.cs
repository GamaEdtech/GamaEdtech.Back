using GamaEdtech.Back.FAQ.Domain.Common;
using System.Linq.Expressions;

namespace GamaEdtech.Back.FAQ.Domain.Entities.FAQ.Criterias
{
    public class CheckFAQSummaryOfQuestionCriteria(string summaryOfQuestion) : CriteriaSpecification<FAQ>
    {
        private readonly string _summaryOfQuestion = summaryOfQuestion;

        public override Expression<Func<FAQ, bool>> ToExpression()
        {
            return current => true;
        }
    }
}