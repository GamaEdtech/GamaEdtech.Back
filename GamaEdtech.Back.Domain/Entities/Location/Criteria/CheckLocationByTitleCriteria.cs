using GamaEdtech.Back.Domain.Common;
using GamaEdtech.Back.Domain.Common.Utilities;
using System.Linq.Expressions;

namespace GamaEdtech.Back.Domain.Entities.Location.Criteria
{
    public class CheckLocationByTitleCriteria(string? title, string? latinTitle) : CriteriaSpecification<Location>
    {
        public override Expression<Func<Location, bool>> ToExpression()
        {
            if ((title is null || !title.HasValue()) &&
                (latinTitle is null || !latinTitle.HasValue()))
            {
                return current => true;
            }

            else if ((string.IsNullOrEmpty(title) || title.HasValue()) &&
                (latinTitle is null || !latinTitle.HasValue()))
            {
                return current => current.Title == title;
            }

            else if ((title is null || !title.HasValue()) &&
               (string.IsNullOrEmpty(latinTitle) || latinTitle.HasValue()))
            {
                return current => current.LatinTitle == latinTitle;
            }
            else return current => true;
        }
    }
}
