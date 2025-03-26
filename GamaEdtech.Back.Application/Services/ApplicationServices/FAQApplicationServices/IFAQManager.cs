using GamaEdtech.Back.Application.DTO.FAQManager;
using GamaEdtech.Back.Domain.Common.Utilities;
using GamaEdtech.Back.Domain.DataAccess.Responses.FAQ;

namespace GamaEdtech.Back.Application.Services.ApplicationServices.FAQApplicationServices
{
    public interface IFAQManager
    {
        Task<List<FAQResponse>> GetFAQWithDynamicFilter(GetFAQWithDynamicFilterDTO getFAQWithDynamicFilterDTO, CancellationToken cancellationToken);
        Task AddForum(CreateForumDTO createForumDTO, CancellationToken cancellationToken);
        Task<List<FAQCategoryResponse>> GetFAQCategoryHierarchy(CustomDateFormat customDateFormat, CancellationToken cancellationToken);
        Task CreateFAQCategory(CreateFAQCategoryDTO createFAQCategoryDTO, CancellationToken cancellationToken);
    }
}