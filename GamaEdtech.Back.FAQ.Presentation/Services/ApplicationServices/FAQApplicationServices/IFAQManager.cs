using GamaEdtech.Back.FAQ.Application.DTO.FAQManager;
using GamaEdtech.Back.FAQ.Domain.Common.Utilities;
using GamaEdtech.Back.FAQ.Domain.DataAccess.Mapper.FAQ;

namespace GamaEdtech.Back.FAQ.Application.Services.ApplicationServices.FAQApplicationServices
{
    public interface IFAQManager
    {
        Task<List<FAQResult>> GetFAQWithDynamicFilter(GetFAQWithDynamicFilterDTO getFAQWithDynamicFilterDTO, CancellationToken cancellationToken);
        Task AddForum(CreateForumDTO createForumDTO, CancellationToken cancellationToken);
        Task<List<FAQCategoryResult>> GetFAQCategoryHierarchy(CustomDateFormat customDateFormat, CancellationToken cancellationToken);
        Task CreateFAQCategory(CreateFAQCategoryDTO createFAQCategoryDTO, CancellationToken cancellationToken);
    }
}
