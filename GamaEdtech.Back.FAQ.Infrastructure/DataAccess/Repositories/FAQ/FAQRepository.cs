using GamaEdtech.Back.FAQ.Domain.Common.InterfaceDependency;
using GamaEdtech.Back.FAQ.Domain.DataAccess.DomainModels;
using GamaEdtech.Back.FAQ.Domain.DataAccess.Repositories.FAQ;
using GamaEdtech.Back.FAQ.Domain.Entities.FAQ.Specifications;
using GamaEdtech.Back.FAQ.Infrastructure.DbContexts.Sql.SqlServer;

namespace GamaEdtech.Back.FAQ.Infrastructure.DataAccess.Repositories.FAQ
{
    public class FAQRepository(ApplicationDbContext dbContext) :
        BaseRepository<Domain.Entities.FAQ.FAQ>(dbContext), IFAQRepository, IScopedDependency
    {
    }
}