using Autofac;
using GamaEdtech.Back.Application.Models;
using GamaEdtech.Back.Application.Services.ApplicationServices.FileManagerService;
using GamaEdtech.Back.Domain.Common.InterfaceDependency;
using GamaEdtech.Back.Domain.Entities;
using GamaEdtech.Back.FAQ.Application.Services.ApplicationServices.FileManagerService;
using GamaEdtech.Back.Infrastructure.DbContexts.Sql.SqlServer;
using GamaEdtech.Back.Infrastructure.Services.MediaServices;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace GamaEdtech.Back.Presentation.Configuration
{
    public static class AutofacConfigurationExtensions
    {
        public class ServiceModules : Autofac.Module
        {
            protected override void Load(ContainerBuilder builder)
            {
                base.Load(builder);

                Assembly PresentationAssembly = typeof(Program).Assembly;
                Assembly DomainAssembly = typeof(IEntity).Assembly;
                Assembly ApplicationAssembly = typeof(ApiResult).Assembly;
                Assembly DataAssembly = typeof(ApplicationDbContext).Assembly;

                builder.RegisterAssemblyTypes(PresentationAssembly, DomainAssembly, ApplicationAssembly, DataAssembly)
                    .AssignableTo<IScopedDependency>()
                    .AsImplementedInterfaces()
                    .InstancePerLifetimeScope();

                builder.RegisterAssemblyTypes(PresentationAssembly, DomainAssembly, ApplicationAssembly, DataAssembly)
                    .AssignableTo<ITransientDependency>()
                    .AsImplementedInterfaces()
                    .InstancePerDependency();

                builder.RegisterAssemblyTypes(PresentationAssembly, DomainAssembly, ApplicationAssembly, DataAssembly)
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
