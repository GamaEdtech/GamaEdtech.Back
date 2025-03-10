using GamaEdtech.Back.FAQ.Domain.Common;
using System.Linq.Expressions;

namespace GamaEdtech.Back.FAQ.Domain.Entities.FAQCategory.Criteria
{
    public class CheckFAQCategoryIdCriteria(Guid faqCategoryId) : CriteriaSpecification<FAQCategory>
    {
        private readonly Guid _faqCategoryId = faqCategoryId;

        public override Expression<Func<FAQCategory, bool>> ToExpression()
        {
            return current => current.Id == _faqCategoryId;
        }
    }
}