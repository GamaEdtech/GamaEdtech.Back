using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace GamaEdtech.Back.Presentation.Swagger
{
    public class UnauthorizedResponsesOperationFilter : IOperationFilter
    {
        private readonly bool includeUnauthorizedAndForbiddenResponses;
        private readonly string schemeName;

        public UnauthorizedResponsesOperationFilter(bool includeUnauthorizedAndForbiddenResponses, string schemeName = "Bearer")
        {
            this.includeUnauthorizedAndForbiddenResponses = includeUnauthorizedAndForbiddenResponses;
            this.schemeName = schemeName;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var filters = context.ApiDescription.ActionDescriptor.FilterDescriptors;
            var metadta = context.ApiDescription.ActionDescriptor.EndpointMetadata;

            var hasAnonymous = filters.Any(p => p.Filter is AllowAnonymousFilter) || metadta.Any(p => p is AllowAnonymousAttribute);
            if (hasAnonymous) return;
            //var hasAuthorize = filters.Any(p => p.Filter is PermissionAuthorizeAttribute) || metadta.Any(p => p is PermissionAuthorizeAttribute);
            //if (!hasAuthorize) return;
#warning Commented For Swagger

            //Be Surat NormAl Swagger TanhA Dar Surati Ke Yek Action DArAie Attribute [Authorize] Bashad Header Auth Bearer Ersal Mikonad
            //Dar Customize Bala Swager Dar Surati Header Ersal Mikonad Ke Filter "hasAnonymous" True va "hasAuthorize" False Bashad(k ba Mohandesi Makus Neveshteh Shodeh ast)

            //Tamam CodeHaie Bala Ghabl Az Authentication Swagger Ejra Mishavand(Dar ZamAn bAz shodan Page baraie har controller/action codehaie bala ejra mishavad)
            //Bad Az Authentication(tavasot swagger) Page Refresh "N"mishavad Va Mohandesi Makus dochar moshkel mishavad (Agar Besurat Dasti Refresh konid=> Authentication Mipare)

            //B Hamin Dalil Header Ersal Nmishavad pas ==> khat 28 commend shod 

            if (includeUnauthorizedAndForbiddenResponses)
            {
                operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
                operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });
            }

            operation.Security.Add(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Scheme = schemeName,
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "OAuth2" }
                    },
                    Array.Empty<string>() //new[] { "readAccess", "writeAccess" }
                }
            });
        }
    }
}
