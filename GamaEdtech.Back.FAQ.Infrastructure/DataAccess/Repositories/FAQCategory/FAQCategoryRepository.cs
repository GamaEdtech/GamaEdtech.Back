using GamaEdtech.Back.FAQ.Domain.Common.InterfaceDependency;
using GamaEdtech.Back.FAQ.Infrastructure.DbContexts.Sql.SqlServer;
using GamaEdtech.Back.FAQ.Domain.DataAccess.Repositories.FAQCategory;
using GamaEdtech.Back.FAQ.Domain.Entities.FAQCategory.Specifications;
using EFCoreSecondLevelCacheInterceptor;
using Microsoft.EntityFrameworkCore;

namespace GamaEdtech.Back.FAQ.Infrastructure.DataAccess.Repositories.FAQCategory
{
    public class FAQCategoryRepository(ApplicationDbContext dbContext) 
        : BaseRepository<Domain.Entities.FAQCategory.FAQCategory>(dbContext),
          IFAQCategoryRepository, IScopedDependency
    {
        public async Task<IReadOnlyCollection<Domain.Entities.FAQCategory.FAQCategory>> ListAsyncWithSecondaryLevelCache(CancellationToken cancellationToken)
        {
            return (await ApplySpecification(new GetAllFAQCategoriesSpecification())
                .Cacheable().ToListAsync(cancellationToken)).AsReadOnly();
               
        }
    }
}
