using GamaEdtech.Back.Application.DTO.FAQManager;
using GamaEdtech.Back.Domain.DataAccess.Requests.FAQ;

namespace GamaEdtech.Back.Application.DTO.Mapping.FAQ
{
    public static class FAQMapping
    {
        public static GetFAQWithDynamicFilterRequest MapToRequest(this GetFAQWithDynamicFilterDTO getFAQWithDynamicFilterDTO)
        {
            return new GetFAQWithDynamicFilterRequest
            {
                FaqCategoriesTitle = getFAQWithDynamicFilterDTO.FaqCategoriesTitle,
                CustomDateFormat = getFAQWithDynamicFilterDTO.CustomDateFormat,
                CustomOrderBy = getFAQWithDynamicFilterDTO.CustomOrderBy,
                FromDate = getFAQWithDynamicFilterDTO.FromDate,
                ToDate = getFAQWithDynamicFilterDTO.ToDate,
                SummaryOfQuestion = getFAQWithDynamicFilterDTO.SummaryOfQuestion
            };
        }
    }
}