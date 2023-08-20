using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rugal.i18n.Core;
using Rugal.i18n.Model;

namespace Rugal.i18n.Extention
{
    public static class StartupExtention
    {
        public static IServiceCollection AddLangModel_Cookie(this IServiceCollection Services, IConfiguration Configuration, Action<LangSetting> OptionFunc = null)
        {
            BaseAddLangModel(Services);

            var Setting = CreateLangSetting(Configuration)
                .UseCookieLang();

            OptionFunc?.Invoke(Setting);
            Services.AddSingleton(Setting);

            return Services;
        }
        public static IServiceCollection AddLangModel_Header(this IServiceCollection Services, IConfiguration Configuration, Action<LangSetting> OptionFunc = null)
        {
            BaseAddLangModel(Services);

            var Setting = CreateLangSetting(Configuration)
                .UseHeaderLang();

            OptionFunc?.Invoke(Setting);
            Services.AddSingleton(Setting);

            return Services;
        }
        private static void BaseAddLangModel(IServiceCollection Services)
        {
            Services.AddHttpContextAccessor()
                .AddScoped<LangModel>();
        }
        private static LangSetting CreateLangSetting(IConfiguration Configuration)
        {
            var Setting = new LangSetting();

            var JsRootPath = Configuration["JsRootPath"];
            var JsViewPath = Configuration["JsViewPath"];
            var JsSharedPath = Configuration["JsSharedPath"];

            if (JsRootPath is not null)
                Setting.JsRootPath = JsRootPath;
            if (JsViewPath is not null)
                Setting.JsViewPath = JsViewPath;
            if (JsSharedPath is not null)
                Setting.JsSharedPath = JsSharedPath;

            var JsonRootPath = Configuration["JsonRootPath"];
            var JsonViewPath = Configuration["JsonViewPath"];
            var JsonSharedPath = Configuration["JsonSharedPath"];

            if (JsonRootPath is not null)
                Setting.JsonRootPath = JsonRootPath;
            if (JsonViewPath is not null)
                Setting.JsonViewPath = JsonViewPath;
            if (JsonSharedPath is not null)
                Setting.JsonSharedPath = JsonSharedPath;

            return Setting;
        }

    }
}