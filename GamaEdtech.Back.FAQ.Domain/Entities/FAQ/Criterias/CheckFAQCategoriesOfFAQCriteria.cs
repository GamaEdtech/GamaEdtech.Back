using GamaEdtech.Back.FAQ.Domain.Common;
using System.Linq.Expressions;

namespace GamaEdtech.Back.FAQ.Domain.Entities.FAQ.Criterias
{
    public class CheckFAQCategoriesOfFAQCriteria(List<string>? titles) : CriteriaSpecification<FAQ>
    {
        public override Expression<Func<FAQ, bool>> ToExpression()
        {
            if (titles == null || titles.Count == 0)
            {
                return current => true;
            }

            return current => current.FAQAndFAQCategories.Any(a => titles.Any(t => t == a.FAQCategory.Title));
        }
    }
}
