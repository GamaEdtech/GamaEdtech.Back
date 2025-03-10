using GamaEdtech.Back.FAQ.Domain.Common.Utilities;

namespace GamaEdtech.Back.FAQ.Domain.DataAccess.DomainModels
{
    public class GetFAQWithDynamicFilterRequest
    {
        public List<string>? FaqCategoriesTitle { get; init; }
        public CustomDateFormat CustomDateFormat { get; init; }
        public CustomOrderBy CustomOrderBy { get; init; }
        public DateTime? FromDate { get; init; }
        public DateTime? ToDate { get; init; }

        public string? SummaryOfQuestion { get; set; }

        public bool HasValue()
        {
            if ((FaqCategoriesTitle is not null && FaqCategoriesTitle.Count != 0) || 
                 FromDate != null || ToDate != null || (SummaryOfQuestion != null && SummaryOfQuestion.HasValue()))
            {
                return true;
            }
            return false;
        }
    }
    public enum CustomOrderBy
    {
        OrderByCreateDateAscending,
        OrderByCreateDateDescending,
    }
}