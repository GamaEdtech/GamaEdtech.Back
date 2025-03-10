using GamaEdtech.Back.FAQ.Application.DTO.FAQManager;
using GamaEdtech.Back.FAQ.Application.DTO.Mapping;
using GamaEdtech.Back.FAQ.Application.Services.ApplicationServices.FileManagerService;
using GamaEdtech.Back.FAQ.Domain.Common.Exceptions;
using GamaEdtech.Back.FAQ.Domain.Common.InterfaceDependency;
using GamaEdtech.Back.FAQ.Domain.Common.Utilities;
using GamaEdtech.Back.FAQ.Domain.DataAccess.DomainModels;
using GamaEdtech.Back.FAQ.Domain.DataAccess.Mapper.FAQ;
using GamaEdtech.Back.FAQ.Domain.Services.FAQCategory;
using Mapster;

namespace GamaEdtech.Back.FAQ.Application.Services.ApplicationServices.FAQApplicationServices
{
    public class FAQManager(IFAQDomainService fAQDomainService, IFileManager fileManager) : IFAQManager, IScopedDependency
    {
        public Task<List<FAQResult>> GetFAQWithDynamicFilter(GetFAQWithDynamicFilterDTO getFAQWithDynamicFilterDTO, CancellationToken cancellationToken)
        {
            return fAQDomainService.GetFAQWithDynamicFilter(new GetFAQWithDynamicFilterRequest
            {
                FaqCategoriesTitle = getFAQWithDynamicFilterDTO.FaqCategoriesTitle,
                CustomDateFormat = getFAQWithDynamicFilterDTO.CustomDateFormat,
                CustomOrderBy = getFAQWithDynamicFilterDTO.CustomOrderBy,
                FromDate = getFAQWithDynamicFilterDTO.FromDate,
                ToDate = getFAQWithDynamicFilterDTO.ToDate,
                SummaryOfQuestion = getFAQWithDynamicFilterDTO.SummaryOfQuestion
            }, cancellationToken);
        }
        public async Task AddForum(CreateForumDTO createForumDTO, CancellationToken cancellationToken)
        {
            var mediaResult = new UploadFileResult();
            if (createForumDTO.AttachFiles is not null && createForumDTO.AttachFiles.Count != 0)
            {
                var uploadResult = await fileManager.UpladFiles
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

        public Task<List<FAQCategoryResult>> GetFAQCategoryHierarchy(CustomDateFormat customDateFormat, CancellationToken cancellationToken)
        {
            return fAQDomainService.GetFAQCategoryHierarchy(customDateFormat, cancellationToken);
        }
    }
}