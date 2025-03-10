using GamaEdtech.Back.FAQ.Domain.Common.Utilities;

namespace GamaEdtech.Back.FAQ.Domain.DataAccess.Mapper.FAQ
{
    public record FAQResult
    {
        public Guid Id { get; init; }
        public string SummaryOfQuestion { get; init; }
        public string Question { get; init; }
        public List<FAQCategoryResult> FAQCategoryTree { get; init; }
        public CustomDateTimeFormat CreateDate { get; init; }
    }
    public static class FAQMapper
    {
        public static List<FAQResult> MapToResult(this List<Entities.FAQ.FAQ> fAQs, CustomDateFormat customDateFormat)
        {
            return fAQs.Select(s => new FAQResult
            {
                Id = s.Id,
                CreateDate = s.CreateDate.ConvertToCustomDate(customDateFormat),
                SummaryOfQuestion = s.SummaryOfQuestion,
                Question = s.Question,
                FAQCategoryTree = Entities.FAQCategory.FAQCategory.BuildHierarchyTree
                (s.FAQAndFAQCategories.Select(s => s.FAQCategory)).MapToResult(customDateFormat)
            }).ToList();
        }
    }
}
