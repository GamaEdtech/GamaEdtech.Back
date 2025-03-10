using GamaEdtech.Back.FAQ.Domain.Common.Exceptions;
using GamaEdtech.Back.FAQ.Domain.Common.InterfaceDependency;
using GamaEdtech.Back.FAQ.Domain.Common.Utilities;
using GamaEdtech.Back.FAQ.Domain.DataAccess.DomainModels;
using GamaEdtech.Back.FAQ.Domain.DataAccess.Mapper.FAQ;
using GamaEdtech.Back.FAQ.Domain.DataAccess.Repositories.FAQ;
using GamaEdtech.Back.FAQ.Domain.DataAccess.Repositories.FAQCategory;
using GamaEdtech.Back.FAQ.Domain.Entities.FAQ.Aggregates;
using GamaEdtech.Back.FAQ.Domain.Entities.FAQ.Specifications;
using GamaEdtech.Back.FAQ.Domain.Entities.FAQCategory;
using GamaEdtech.Back.FAQ.Domain.Entities.FAQCategory.Specifications;

namespace GamaEdtech.Back.FAQ.Domain.Services.FAQCategory
{
    public class FAQDomainService
        (
            IFAQCategoryRepository fAQCategoryRepository, 
            IFAQRepository fAQRepository
        ) 
        : IFAQDomainService, IScopedDependency
    {
        public async Task<List<FAQResult>> GetFAQWithDynamicFilter(GetFAQWithDynamicFilterRequest dynamicFilter, CancellationToken cancellationToken)
        {
            var fAQs = await fAQRepository.ListAsync(new GetFAQWithDynamicFilterSpecification(dynamicFilter,
                FAQRelations.FAQCategory), cancellationToken);

            return fAQs.MapToResult(dynamicFilter.CustomDateFormat);
        }

        public async Task<List<FAQCategoryResult>> GetFAQCategoryHierarchy(CustomDateFormat customDateFormat, CancellationToken cancellationToken)
        {
            var categories = await fAQCategoryRepository.ListAsyncWithSecondaryLevelCache(cancellationToken);
            var tree = Entities.FAQCategory.FAQCategory.BuildHierarchyTree(categories);
            return tree.MapToResult(customDateFormat);
        }

        public async Task CreateFAQ(List<string> faqCategoryTitles, string summaryOfQuestion, string question,
            UploadFileResult uploadFileResult, CancellationToken cancellationToken)
        {
            var faqCategories = await fAQCategoryRepository.ListAsync
                (new GetFAQCategoryWithTitlesSpecification(faqCategoryTitles), cancellationToken);

            if (faqCategories.Count == 0) throw new NotFoundException();

            var faq = Entities.FAQ.FAQ.Create(summaryOfQuestion, question, faqCategories);

            if (uploadFileResult is not null && uploadFileResult.FileResults != null
                && uploadFileResult.FileResults.Count != 0)
            {
                faq.AddMedia(
                       uploadFileResult.FileResults.Select
                       (file => Media.Create(file.FileName, file.FileAddress, MediaEntity.FAQ, faq.Id,
                   file.ContentType))
                );
            }

            await fAQRepository.AddAsync(faq, cancellationToken);
        }
        public async Task CreateFAQCategory(string? parentCategoryTitle, string title, CategoryType categoryType, CancellationToken cancellationToken)
        {
            Entities.FAQCategory.FAQCategory newCategory;

            if (parentCategoryTitle != null && parentCategoryTitle.HasValue())
            {
                var parentCategory = await fAQCategoryRepository.FirstOrDefaultAsync(
                    new GetFAQCategoryWithTitleSpecification(parentCategoryTitle), cancellationToken)
                    ?? throw new NotFoundException();

                newCategory = Entities.FAQCategory.FAQCategory.Create(title, categoryType, parentCategory);
            }
            else
            {
                newCategory = Entities.FAQCategory.FAQCategory.Create(title, categoryType, null);
            }
            await fAQCategoryRepository.AddAsync(newCategory, cancellationToken);
        }
    }
}