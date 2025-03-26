using GamaEdtech.Back.Domain.Common.InterfaceDependency;
using GamaEdtech.Back.Domain.DataAccess.Repositories.School;
using GamaEdtech.Back.Infrastructure.DbContexts.Sql.SqlServer;

namespace GamaEdtech.Back.Infrastructure.DataAccess.Repositories.School
{
    public class SchoolRepository(ApplicationDbContext dbContext) : BaseRepository<Domain.Entities.School.School>(dbContext),
        ISchoolRepository, IScopedDependency
    {
    }
}
