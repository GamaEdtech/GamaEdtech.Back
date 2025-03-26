using GamaEdtech.Back.Domain.Common.Utilities;
using GamaEdtech.Back.Domain.DataAccess.Mapper.FAQ;
using GamaEdtech.Back.Domain.DataAccess.Responses.FAQ;

namespace GamaEdtech.Back.Domain.DataAccess.Mappers.FAQ
{
    public static class FAQMapperMapperExtension
    {
        public static List<FAQResponse> MapToResult(this List<Entities.FAQ.FAQ> fAQs, CustomDateFormat customDateFormat)
        {
            return fAQs.Select(s => new FAQResponse
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