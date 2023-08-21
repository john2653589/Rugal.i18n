using Microsoft.AspNetCore.Http;
using Rugal.i18n.Model;

namespace Rugal.i18n.Core
{
    public partial class LangModel
    {
        #region DI Service
        private readonly IHttpContextAccessor HttpContextAccessor;
        public readonly LangSetting Setting;
        private HttpContext HttpContext => HttpContextAccessor.HttpContext;
        #endregion

        #region Get Property
        public string Language => GetLanguage();
        public Enum LanguageType => GetLanguageType();
        public IEnumerable<string> UrlPath => GetUrlPath();
        public IEnumerable<string> RoutePath => GetRoutePath();
        #endregion

        public LangModel(
            IHttpContextAccessor _HttpContextAccessor,
            LangSetting _Setting)
        {
            HttpContextAccessor = _HttpContextAccessor;
            Setting = _Setting;

            InitJsonSet();
        }

        #region Public Method
        public bool IsLanguage(Enum Type, bool IsIgnoreCase = true)
        {
            var LangName = Type.ToString();
            if (IsIgnoreCase)
                LangName = LangName.ToLower();

            var NowLang = Language;
            if (IsIgnoreCase)
                NowLang = NowLang.ToLower();

            var IsEquals = LangName == NowLang;
            return IsEquals;
        }
        public bool IsLanguage(LanguageType Type, bool IsIgnoreCase = true)
        {
            return IsLanguage(Type as Enum, IsIgnoreCase);
        }
        #endregion

        #region Public Process
        public virtual string CombineFullFilePath(string FileName, string FolderName, string FileBasePath, string Separator, IEnumerable<string> LangPaths = null)
        {
            LangPaths ??= new List<string>();
            var JoinPaths = new List<string> { FileBasePath, FolderName };

            if (LangPaths.Any())
                JoinPaths.AddRange(LangPaths);

            JoinPaths.Add(FileName);

            var ClearFullPath = JoinPaths.Select(Item => Item.TrimStart('/', '.').TrimEnd('/', '.'));
            var Result = string.Join(Separator, ClearFullPath);
            return Result;
        }

        #endregion

        #region Private Process
        private IEnumerable<string> GetUrlPath()
        {
            var ClearPath = HttpContext.Request.Path.Value
                .TrimStart('/')
                .TrimEnd('/');

            if (string.IsNullOrWhiteSpace(ClearPath))
                return null;

            var UrlPathValues = ClearPath.Split('/');

            var Result = Setting.UrlTakeFrom switch
            {
                TakeFromType.FromLeft => UrlPathValues
                    .Skip(Setting.UrlPathSkipCount)
                    .Take(Setting.UrlPathTakeCount),

                TakeFromType.FromRight => UrlPathValues
                    .TakeLast(Setting.UrlPathTakeCount)
                    .Skip(Setting.UrlPathSkipCount)
            };

            Result = Setting.FormatUrlPath(Result);
            return Result;
        }
        private IEnumerable<string> GetRoutePath()
        {
            if (!HttpContext.Request.RouteValues.Any())
                return null;

            var RoutePathValues = HttpContext.Request.RouteValues
                .Select(Item => Item.Value.ToString());

            var Result = Setting.RouteTakeFrom switch
            {
                TakeFromType.FromLeft => RoutePathValues
                    .Skip(Setting.RoutePathSkipCount)
                    .Take(Setting.RoutePathTakeCount),

                TakeFromType.FromRight => RoutePathValues
                    .TakeLast(Setting.RoutePathTakeCount)
                    .Skip(Setting.RoutePathSkipCount)
            };

            Result = Setting.FormatRoutePath(Result);
            return Result;
        }
        private string GetLanguage()
        {
            if (Setting.LangFrom == LangFromType.Cookie)
            {
                var Cookies = HttpContext.Request.Cookies;
                if (!Cookies.Any(Item => Item.Key == Setting.CookieLangKey))
                    return Setting.DefaultLanguageType.ToString();

                var GetCookie = Cookies
                    .FirstOrDefault(Item => Item.Key == Setting.CookieLangKey);

                var ClearValue = Setting.FormatCookieLang(GetCookie.Value);
                return ClearValue;
            }
            else if (Setting.LangFrom == LangFromType.Header)
            {
                var Headers = HttpContext.Request.Headers;
                var HeaderValue = Setting.HeaderLangKey == LangSetting.DefaultHeaderLangKey ?
                    Headers.AcceptLanguage.ToString() :
                    Headers[Setting.HeaderLangKey].ToString();

                var ClearValue = Setting.FormatHeaderLang(HeaderValue);
                return ClearValue;
            }

            return null;
        }
        private Enum GetLanguageType()
        {
            var ParseLang = Language.Replace('-', '_');

            var UseLangType = Setting.DefaultLanguageType.GetType();

            if (!Enum.TryParse(UseLangType, ParseLang, true, out var Result))
                return Setting.DefaultLanguageType;

            return Result as Enum;
        }
        private static string FileNameCase(Enum LangType, FileNameCaseType Case, FileNameReplaceType ReplaceType)
        {
            var FileName = LangType.ToString();
            FileName = ReplaceType switch
            {
                FileNameReplaceType.DashToUnderLine => FileName.Replace('-', '_'),
                FileNameReplaceType.UnderLineToDash => FileName.Replace('_', '-'),
                _ => FileName,
            };

            var CaseFileName = Case switch
            {
                FileNameCaseType.Upper => FileName.ToUpper(),
                FileNameCaseType.Lower => FileName.ToLower(),
                _ => FileName
            };

            if (Case == FileNameCaseType.HeadUpper)
            {
                var HeadChar = FileName.First().ToString().ToUpper();
                var OtherChar = string.Join("", FileName.Skip(1).Take(FileName.Length - 1)).ToLower();
                CaseFileName = $"{HeadChar}{OtherChar}";
            }
            return CaseFileName;
        }
        #endregion
    }
}