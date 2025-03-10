using EFCoreSecondLevelCacheInterceptor;
using GamaEdtech.Back.FAQ.Infrastructure.DbContexts.Sql.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace VoipService.Api.Registeration
{
    public static class RegisterDbContextConfiguration
    {
        public static void RegisterDbContext(this IServiceCollection services, IConfiguration config)
        {
            services.AddEFSecondLevelCache(options =>
            {
                options.UseMemoryCacheProvider()
                        .DisableLogging(true);
            });
            
            services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            {
                options.UseSqlServer(config.GetConnectionString("SqlServer"), x => x.UseHierarchyId())
                    .AddInterceptors(serviceProvider
                    .GetRequiredService<SecondLevelCacheInterceptor>());

            }, ServiceLifetime.Scoped);
        }
    }
}