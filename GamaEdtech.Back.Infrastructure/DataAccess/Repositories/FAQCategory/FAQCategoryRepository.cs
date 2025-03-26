using EFCoreSecondLevelCacheInterceptor;
using Microsoft.EntityFrameworkCore;
using GamaEdtech.Back.Domain.Common.InterfaceDependency;
using GamaEdtech.Back.Domain.DataAccess.Repositories.FAQCategory;
using GamaEdtech.Back.Domain.Entities.FAQCategory.Specifications;
using GamaEdtech.Back.Infrastructure.DbContexts.Sql.SqlServer;

namespace GamaEdtech.Back.Infrastructure.DataAccess.Repositories.FAQCategory
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