using Microsoft.Extensions.DependencyInjection;
using Rugal.i18n.OpenApi.i18n.Model;

namespace Rugal.i18n.Extention
{
    public static class StartupExtention
    {
        public static IServiceCollection AddSwaggerLanguage(this IServiceCollection Services)
        {
            Services.AddSwaggerGen(options =>
            {
                options.OperationFilter<LangModelHeaderParameter>();
            });

            return Services;
        }
    }
}