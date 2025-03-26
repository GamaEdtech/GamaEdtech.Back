using GamaEdtech.Back.Application.DataInitializer;
using GamaEdtech.Back.Infrastructure.DbContexts.Sql.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GamaEdtech.Back.Application.Middlewares
{
    public static class SeedDatabaseMiddleware
    {
        public static async void SeedDatabase(this IApplicationBuilder app, IHostEnvironment env)
        {
            using var Scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var DbContext = Scope.ServiceProvider.GetService<ApplicationDbContext>();
            if (env.IsProduction().Equals(true))
                DbContext?.Database.EnsureCreated();
            else
                DbContext?.Database.Migrate();
            var DataInitializers = Scope.ServiceProvider
                .GetServices<IDataInitializer>().OrderBy(C => C.SortNumber);
            foreach (var DataInitializer in DataInitializers)
                await DataInitializer.InitializeData();
        }
    }
}