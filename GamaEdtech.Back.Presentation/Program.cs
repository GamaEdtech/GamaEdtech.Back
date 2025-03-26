using Autofac.Extensions.DependencyInjection;
using Autofac;
using VoipService.Api.Registeration;
using GamaEdtech.Back.FAQ.Application.Registeration;
using static GamaEdtech.Back.Presentation.Configuration.AutofacConfigurationExtensions;
using GamaEdtech.Back.Presentation.Configuration;
using GamaEdtech.Back.Presentation.Swagger;
using GamaEdtech.Back.Application.Middlewares;
using GamaEdtech.Back.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

//set autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>
(builder => builder.RegisterModule(new ServiceModules()));
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);
builder.Configuration.AddEnvironmentVariables();

builder.Services.Configure<AppSetting>(builder.Configuration);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.RegisterDbContext(builder.Configuration);
builder.Services.RegisterApiVersioning();
builder.Services.RegisterMediatR(typeof(IEntity).Assembly);
builder.Services.AddCustomSwagger(builder.Configuration);


var app = builder.Build();

//Middlewares
app.UseCustomSwaggerUI();
app.UseStaticFiles();
app.UseCustomExceptionHandler();
app.UseAuthorization();

app.SeedDatabase(builder.Environment);
app.MapControllers();

app.Run();
