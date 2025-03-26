using GamaEdtech.Back.Domain.Common.InterfaceDependency;
using GamaEdtech.Back.Domain.DataAccess.Repositories.FAQ;
using GamaEdtech.Back.Infrastructure.DbContexts.Sql.SqlServer;

namespace GamaEdtech.Back.Infrastructure.DataAccess.Repositories.FAQ
{
    public class FAQRepository(ApplicationDbContext dbContext) :
        BaseRepository<Domain.Entities.FAQ.FAQ>(dbContext), IFAQRepository, IScopedDependency
    {
    }
}