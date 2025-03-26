using GamaEdtech.Back.Domain.Common;
using System.Linq.Expressions;

namespace GamaEdtech.Back.Domain.Entities.FAQCategory.Criteria
{
    public class CheckFAQCategoryTitleCriteria : CriteriaSpecification<FAQCategory>
    {
        private readonly string? _title;
        private readonly List<string>? _titles;

        public CheckFAQCategoryTitleCriteria(string title)
        {
            _title = title;
            _titles = null;
        }
        public CheckFAQCategoryTitleCriteria(List<string> titles)
        {
            _titles = titles;
            _title = null;
        }
        public override Expression<Func<FAQCategory, bool>> ToExpression()
        {
            if (_titles is null && _title is not null)
            {
                return current => current.Title == _title;
            }
            else if (_titles is not null && _title is null)
            {
                return current => _titles.Any(a => a == current.Title);
            }
            else return current => true;
        }
    }
}