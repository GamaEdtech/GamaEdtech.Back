using GamaEdtech.Back.FAQ.Domain.Common.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;

namespace GamaEdtech.Back.FAQ.Application.Swagger
{
    public static class SwaggerConfigurationExtensions
    {
        public static void AddCustomSwagger(this IServiceCollection services, IConfiguration config)
        {
            var siteUrl = config.GetValue<string>("servicesHostDomainSetting:SiteDomin") ?? "";

            Assert.NotNull(services, nameof(services));

            //Add Service to use Example Filters in swagger
            #region AddSwaggerExamples
            //Add services to use Example Filters in swagger
            //If you want to use the Request and Response example filters (and have called options.ExampleFilters() above), then you MUST also call
            //This method to register all ExamplesProvider classes form the assembly
            //services.AddSwaggerExamplesFromAssemblyOf<PersonRequestExample>();

            //We call this method for by reflection with the Startup type of entry assmebly (MyApi assembly)
            var mainAssembly = Assembly.GetEntryAssembly(); // => MyApi project assembly
            var mainType = mainAssembly.GetExportedTypes()[0];

            var methodName = nameof(Swashbuckle.AspNetCore.Filters.ServiceCollectionExtensions.AddSwaggerExamplesFromAssemblyOf);
            MethodInfo method = typeof(Swashbuckle.AspNetCore.Filters.ServiceCollectionExtensions)
               .GetMethods()
               .Where(C => C.Name.Equals(methodName))
               .FirstOrDefault(x => x.IsGenericMethod);

            MethodInfo generic = method.MakeGenericMethod(mainType);
            generic.Invoke(null, new[] { services });
            #endregion

            services.AddSwaggerGen(option =>
            {
                var XmlDocumentPath = Path.Combine(AppContext.BaseDirectory, "Gama.xml");
                // Xml Documentation
                option.IncludeXmlComments(XmlDocumentPath, true);
                option.EnableAnnotations();
                option.DescribeAllParametersInCamelCase();


                //option.DescribeStringEnumsInCamelCase();
                //option.UseReferencedDefinitionsForEnums();
                //option.IgnoreObsoleteActions();
                //option.IgnoreObsoleteProperties();

                option.SwaggerDoc("v1", new OpenApiInfo { Version = "v1", Title = "Gama API V1", Description = "Gama v1", Contact = new OpenApiContact() { Email = "samiyariceboy@gmail.com", Name = "samiyar" } });
                option.SwaggerDoc("v2", new OpenApiInfo { Version = "v2", Title = "Gama API V2", Description = "Gama v2", Contact = new OpenApiContact() { Email = "samiyariceboy@gmail.com", Name = "samiyar" } });

                #region 1). Set Swagger Versioning with Reflection
                //Remove Version parameter...
                option.OperationFilter<RemoveVersionParameters>();

                //IFormFile
                //option.OperationFilter<ApplyUploadFileOperationFilter>();

                // Set version "api/v{version}/[controller]"
                option.DocumentFilter<SetVersionInPaths>();

                option.DocInclusionPredicate((docName, apiDesc) =>
                {
                    if (!apiDesc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;

                    var Version = methodInfo.DeclaringType
                        .GetCustomAttributes<ApiVersionAttribute>(true)
                        .SelectMany(attri => attri.Versions);

                    return Version.Any(v => $"v{v}" == docName);
                });

                #endregion

                #region 4).Filters summery of action that is not already set (dynamic) pashm rizooon
                option.OperationFilter<ApplySummariesOperationFilter>();
                // Set summery of action that is not already set (dynamic) pashm rizooon 
                // Enable to use [SwaggerRequestExample] [SwaggerResponseExample]
                option.ExampleFilters();
                // Filter2 for swagger (Adds an upload button to endpoint which have [AddSwaggerFileUploadButton])

                #endregion

                #region Add UnAuthorized to Response
                //Add 401 response and security requirements (Lock icon) to actions that need authorization
                option.OperationFilter<UnauthorizedResponsesOperationFilter>(true, "OAuth2");
                #endregion

                #region 0). Add unAuthorize to Response (importent part)
                //option.OperationFilter<UnauthorizedResponsesOperationFilter>(true, "Bearer");
                //option.AddSecurityDefinition("OAuth2", new OpenApiSecurityScheme
                //{
                //    In = ParameterLocation.Header,
                //    Scheme = "Bearer",
                //    BearerFormat = "JWT",
                //    Type = SecuritySchemeType.OAuth2,
                //    Flows = new OpenApiOAuthFlows
                //    {
                //        Password = new OpenApiOAuthFlow
                //        {
                //            TokenUrl = new Uri($"{siteUrl}/v1/Auth/GetTokenJustForSwagger") //,
                //            //Scopes = new Dictionary<string, string>
                //            //{
                //            //    { "readAccess", "Access read operations" },
                //            //    { "writeAccess", "Access write operations" }
                //            //}
                //            //RefreshUrl = new Uri($"{siteSettings.servicesHostDomainSetting.ProjectManagerService.SiteDomin}/v1/Auth/Token"),
                //        }//,
                //    }
                //});
                #endregion
            });
        }

        public static void UseCustomSwaggerUI(this IApplicationBuilder app)
        {
            Assert.NotNull(app, nameof(app));

            app.UseStaticFiles();

            app.UseSwagger();

            app.UseSwaggerUI(option =>
            {
                option.DocExpansion(DocExpansion.None);

                option.SwaggerEndpoint("/swagger/v1/swagger.json", "Gama-v1");
                option.SwaggerEndpoint("/swagger/v2/swagger.json", "Gama-v2");
                option.InjectStylesheet("/swagger-ui/SwaggerDark.css"); //Comment=to Light Mode
            });

            app.UseReDoc(options =>
            {
                options.SpecUrl("/swagger/v1/swagger.json");
                options.SpecUrl("/swagger/v2/swagger.json");

                #region Customizing
                //By default, the ReDoc UI will be exposed at "/api-docs"
                //options.RoutePrefix = "docs";
                //options.DocumentTitle = "My API Docs";

                options.EnableUntrustedSpec();
                options.ScrollYOffset(10);
                options.HideHostname();
                options.HideDownloadButton();
                options.ExpandResponses("200,201");
                options.RequiredPropsFirst();
                options.NoAutoAuth();
                options.PathInMiddlePanel();
                options.HideLoading();
                options.NativeScrollbars();
                options.DisableSearch();
                options.OnlyRequiredInSamples();
                options.SortPropsAlphabetically();
                #endregion
            });
        }
    }
}
