using Ardalis.Specification;
using GamaEdtech.Back.FAQ.Domain.Common;
using GamaEdtech.Back.FAQ.Domain.Entities.FAQCategory.Criteria;

namespace GamaEdtech.Back.FAQ.Domain.Entities.FAQCategory.Specifications
{
    public class GetFAQCategoryWithTitlesSpecification : BaseSpecification<FAQCategory>
    {
        private readonly List<string> _titles;

        public GetFAQCategoryWithTitlesSpecification(List<string> titles)
        {
            _titles = titles;
            Query.Where(Criteria().ToExpression());
        }
        protected override CriteriaSpecification<FAQCategory> Criteria()
        {
            return new CheckFAQCategoryTitleCriteria(_titles);
        }
    }
}