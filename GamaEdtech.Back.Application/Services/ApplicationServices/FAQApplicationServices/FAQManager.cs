using GamaEdtech.Back.Application.DTO.FAQManager;
using GamaEdtech.Back.Application.DTO.Mapping.FAQ;
using GamaEdtech.Back.Application.DTO.Mapping.Media;
using GamaEdtech.Back.Domain.Common.Exceptions;
using GamaEdtech.Back.Domain.Common.InterfaceDependency;
using GamaEdtech.Back.Domain.Common.Utilities;
using GamaEdtech.Back.Domain.Services.FAQ;
using GamaEdtech.Back.Application.Services.ApplicationServices.FileManagerService;
using GamaEdtech.Back.Domain.DataAccess.Responses.FAQ;

namespace GamaEdtech.Back.Application.Services.ApplicationServices.FAQApplicationServices
{
    public class FAQManager(IFAQDomainService fAQDomainService, IFileManager fileManager) : IFAQManager, IScopedDependency
    {
        public Task<List<FAQResponse>> GetFAQWithDynamicFilter(GetFAQWithDynamicFilterDTO dynamicFilterDto, CancellationToken cancellationToken)
        {
            return fAQDomainService.GetFAQWithDynamicFilter(dynamicFilterDto.MapToRequest(), cancellationToken);
        }
        public async Task AddForum(CreateForumDTO createForumDTO, CancellationToken cancellationToken)
        {
            var mediaResult = new Domain.DataAccess.Responses.Media.UploadFileResponse();
            if (createForumDTO.AttachFiles is not null && createForumDTO.AttachFiles.Count != 0)
            {
                var uploadResult = await fileManager.UploadFiles
                    (await createForumDTO.AttachFiles.MapToUploadFileRequest(), "Content", cancellationToken);

                mediaResult = uploadResult.GetUploaderFileResultsAllUploaded().UploadFileResult ??
                    throw new BadRequestException();
            }

            await fAQDomainService.CreateFAQ(createForumDTO.FaqCategoryTitles,
                createForumDTO.SummaryOfQuestion, createForumDTO.Question,
                mediaResult, cancellationToken);
        }

        public Task CreateFAQCategory(CreateFAQCategoryDTO createFAQCategoryDTO, CancellationToken cancellationToken)
        {
            return fAQDomainService.CreateFAQCategory(createFAQCategoryDTO.ParentCategoryTitle, createFAQCategoryDTO.Title,
                createFAQCategoryDTO.CategoryType, cancellationToken);
        }

        public Task<List<FAQCategoryResponse>> GetFAQCategoryHierarchy(CustomDateFormat customDateFormat, CancellationToken cancellationToken)
        {
            return fAQDomainService.GetFAQCategoryHierarchy(customDateFormat, cancellationToken);
        }
    }
}