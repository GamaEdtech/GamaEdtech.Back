using Autofac;
using GamaEdtech.Back.FAQ.Application.Services.ApplicationServices.FileManagerService;
using GamaEdtech.Back.FAQ.Domain.Common.InterfaceDependency;
using GamaEdtech.Back.FAQ.Domain.Entities;
using GamaEdtech.Back.FAQ.Infrastructure.DbContexts.Sql.SqlServer;
using GamaEdtech.Back.FAQ.Infrastructure.Services.MediaServices;
using Microsoft.Extensions.Options;
using System.Reflection;
namespace GamaEdtech.Back.FAQ.Application.Configuration
{
    public static class AutofacConfigurationExtensions
    {
        public class ServiceModules : Autofac.Module
        {
            protected override void Load(ContainerBuilder builder)
            {
                base.Load(builder);

                Assembly ApiAssembly = typeof(Program).Assembly;
                Assembly EntitiesAssembly = typeof(IEntity).Assembly;
                Assembly DataAssembly = typeof(ApplicationDbContext).Assembly;

                builder.RegisterAssemblyTypes(ApiAssembly, EntitiesAssembly, DataAssembly)
                    .AssignableTo<IScopedDependency>()
                    .AsImplementedInterfaces()
                    .InstancePerLifetimeScope();

                builder.RegisterAssemblyTypes(ApiAssembly, EntitiesAssembly, DataAssembly)
                    .AssignableTo<ITransientDependency>()
                    .AsImplementedInterfaces()
                    .InstancePerDependency();

                builder.RegisterAssemblyTypes(ApiAssembly, EntitiesAssembly, DataAssembly)
                    .AssignableTo<ISingletonDependency>()
                    .AsImplementedInterfaces()
                    .SingleInstance();

                #region Accessors
                builder.RegisterFileManager();
                builder.RegisterAzureFileUploader();
                #endregion
            }
        }
        #region Accessors
        private static void RegisterFileManager(this ContainerBuilder builder)
        {
            builder.Register(c =>
            {
                var config = c.Resolve<IOptions<AppSetting>>().Value;
                var fileUploaders = c.Resolve<IEnumerable<IFileUploader>>();
                return new FileManager(fileUploaders, [.. config.FileUpload.Uploaders.Keys]);
            }).As<IFileManager>();
        }

        public static void RegisterAzureFileUploader(this ContainerBuilder builder)
        {
            builder.Register(c =>
            {
                var config = c.Resolve<IOptions<AppSetting>>().Value;
                config.FileUpload.Uploaders.TryGetValue("Azure", out var fileUploadConfig);
                var azureConnection = config.ConnectionStrings.Azure;
                return new AzureUploadFile(azureConnection, fileUploadConfig.ContainerNames);
            }).As<IFileUploader>();
        }
        #endregion
    }
}
