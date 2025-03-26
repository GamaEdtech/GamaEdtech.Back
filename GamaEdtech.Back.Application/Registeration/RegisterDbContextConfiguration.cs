using EFCoreSecondLevelCacheInterceptor;
using GamaEdtech.Back.Infrastructure.DbContexts.Sql.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
                options.UseSqlServer(config.GetConnectionString("SqlServer"), x => 
                {
                    x.UseNetTopologySuite();
                    x.UseHierarchyId();
                })
                .AddInterceptors(serviceProvider
                .GetRequiredService<SecondLevelCacheInterceptor>());

            }, ServiceLifetime.Scoped);
        }
    }
}