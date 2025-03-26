using GamaEdtech.Back.Domain.Common.Utilities;
using GamaEdtech.Back.Domain.DataAccess.Requests.FAQ;
using GamaEdtech.Back.Domain.DataAccess.Responses.FAQ;
using GamaEdtech.Back.Domain.DataAccess.Responses.Media;
using GamaEdtech.Back.Domain.Entities.FAQCategory;

namespace GamaEdtech.Back.Domain.Services.FAQ
{
    public interface IFAQDomainService
    {
        Task<List<FAQResponse>> GetFAQWithDynamicFilter(GetFAQWithDynamicFilterRequest domainModel, CancellationToken cancellationToken);
        Task CreateFAQ(List<string> faqCategoryTitles, string summaryOfQuestion, string question,
             UploadFileResponse uploadFileResult, CancellationToken cancellationToken);
        Task<List<FAQCategoryResponse>> GetFAQCategoryHierarchy(CustomDateFormat customDateFormat, CancellationToken cancellationToken);
        Task CreateFAQCategory(string? parentCategoryTitle, string title, CategoryType categoryType, CancellationToken cancellationToken);
    }
}
