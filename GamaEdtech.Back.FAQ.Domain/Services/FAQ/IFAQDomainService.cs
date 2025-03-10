using GamaEdtech.Back.FAQ.Domain.Common.Utilities;
using GamaEdtech.Back.FAQ.Domain.DataAccess.DomainModels;
using GamaEdtech.Back.FAQ.Domain.DataAccess.Mapper.FAQ;
using GamaEdtech.Back.FAQ.Domain.Entities.FAQCategory;

namespace GamaEdtech.Back.FAQ.Domain.Services.FAQCategory
{
    public interface IFAQDomainService  
    {
        Task<List<FAQResult>> GetFAQWithDynamicFilter(GetFAQWithDynamicFilterRequest domainModel, CancellationToken cancellationToken);
        Task CreateFAQ(List<string> faqCategoryTitles, string summaryOfQuestion, string question,
             UploadFileResult uploadFileResult, CancellationToken cancellationToken);
        Task<List<FAQCategoryResult>> GetFAQCategoryHierarchy(CustomDateFormat customDateFormat, CancellationToken cancellationToken);
        Task CreateFAQCategory(string? parentCategoryTitle, string title, CategoryType categoryType, CancellationToken cancellationToken);
    }
}
