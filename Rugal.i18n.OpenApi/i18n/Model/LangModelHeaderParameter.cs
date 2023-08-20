using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Rugal.i18n.Model;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Rugal.i18n.OpenApi.i18n.Model
{
    public class LangModelHeaderParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter> { };

            var AllLang = Enum.GetValues<LanguageType>()
                .Where(Item => Item != LanguageType.None)
                .Select(Item => new OpenApiString(Item.ToString()))
                .Cast<IOpenApiAny>()
                .ToList();

            operation.Parameters.Add(new OpenApiParameter()
            {
                Name = "Accept-Language",
                In = ParameterLocation.Header,
                Description = "Language",
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Default = new OpenApiString(LanguageType.zh_Hant.ToString()),
                    Enum = AllLang,
                },
            });
        }
    }

}
