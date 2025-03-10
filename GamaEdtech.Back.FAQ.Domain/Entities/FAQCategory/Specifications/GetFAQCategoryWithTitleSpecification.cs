using Ardalis.Specification;
using GamaEdtech.Back.FAQ.Domain.Common;
using GamaEdtech.Back.FAQ.Domain.Entities.FAQCategory.Criteria;

namespace GamaEdtech.Back.FAQ.Domain.Entities.FAQCategory.Specifications
{
    public class GetFAQCategoryWithTitleSpecification : BaseSpecification<FAQCategory>
    {
        private readonly string _title;

        public GetFAQCategoryWithTitleSpecification(string title)
        {
            _title = title;
            Query.Where(Criteria().ToExpression());
        }

        protected override CriteriaSpecification<FAQCategory> Criteria()
        {
            return new CheckFAQCategoryTitleCriteria(_title);
        }
    }
}