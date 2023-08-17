namespace Rugal.i18n.Model
{
    public partial class LangSetting
    {
        #region Const Property
        public const string DefaultCookieLangKey = ".AspNetCore.Culture";
        public const string DefaultHeaderLangKey = "Accept-Language";
        #endregion

        #region Setting Property
        public string DefaultLanguage { get; set; } = "en-us";
        public LangFromType LangFrom { get; set; } = LangFromType.Header;
        public PathFindSortType PathFindSort { get; set; } = PathFindSortType.Url_Route;
        #endregion

        #region Cookie Setting
        public string CookieLangKey { get; set; } = DefaultCookieLangKey;
        private Func<string, string> CookieFormatFunc { get; set; }
        #endregion

        #region Header Setting
        public string HeaderLangKey { get; set; } = DefaultHeaderLangKey;
        private Func<string, string> HeaderFormatFunc { get; set; }
        #endregion

        #region Url Path Setting
        public TakeFromType UrlTakeFrom { get; set; } = TakeFromType.FromLeft;
        public int UrlPathSkipCount { get; set; } = 0;
        public int UrlPathTakeCount { get; set; } = 5;
        private Func<IEnumerable<string>, IEnumerable<string>> UrlPathFormatFunc { get; set; }
        #endregion

        #region Route Path Setting
        public TakeFromType RouteTakeFrom { get; set; } = TakeFromType.FromLeft;
        public int RoutePathSkipCount { get; set; } = 0;
        public int RoutePathTakeCount { get; set; } = 5;
        private Func<IEnumerable<string>, IEnumerable<string>> RoutePathFormatFunc { get; set; }
        #endregion

        #region With Setting Method
        public LangSetting WithHeaderLangKey(string _HeaderLangKey)
        {
            HeaderLangKey = _HeaderLangKey;
            return this;
        }
        public LangSetting WithCookieLangKey(string _LangKey)
        {
            HeaderLangKey = _LangKey;
            return this;
        }
        public LangSetting WithDefaultLanguage(string _DefaultLanguage)
        {
            DefaultLanguage = _DefaultLanguage;
            return this;
        }
        public LangSetting WithCookieLangFormat(Func<string, string> _FormatFunc)
        {
            CookieFormatFunc = _FormatFunc;
            return this;
        }
        public LangSetting WithHeaderLangFormat(Func<string, string> _FormatFunc)
        {
            HeaderFormatFunc = _FormatFunc;
            return this;
        }
        public LangSetting WithRoutePathFormat(Func<IEnumerable<string>, IEnumerable<string>> _FormatFunc)
        {
            RoutePathFormatFunc = _FormatFunc;
            return this;
        }
        public LangSetting WithUrlPathFormat(Func<IEnumerable<string>, IEnumerable<string>> _FormatFunc)
        {
            UrlPathFormatFunc = _FormatFunc;
            return this;
        }
        #endregion

        #region Use Lang Set
        public LangSetting UseCookieLang()
        {
            LangFrom = LangFromType.Cookie;
            return this;
        }
        public LangSetting UseHeaderLang()
        {
            LangFrom = LangFromType.Header;
            return this;
        }
        #endregion

        #region Public Method
        public string FormatCookieLang(string _CookieLang)
        {
            if (CookieFormatFunc is null)
                return _CookieLang;

            var Result = CookieFormatFunc.Invoke(_CookieLang);
            return Result;
        }
        public string FormatHeaderLang(string _HeaderLang)
        {
            if (HeaderFormatFunc is null)
                return _HeaderLang;

            var Result = HeaderFormatFunc.Invoke(_HeaderLang);
            return Result;
        }
        public IEnumerable<string> FormatRoutePath(IEnumerable<string> Paths)
        {
            if (RoutePathFormatFunc is null)
                return Paths;

            var Reuslt = RoutePathFormatFunc?.Invoke(Paths);
            return Reuslt;
        }
        public IEnumerable<string> FormatUrlPath(IEnumerable<string> Paths)
        {
            if (UrlPathFormatFunc is null)
                return Paths;

            var Result = UrlPathFormatFunc?.Invoke(Paths);
            return Result;
        }
        #endregion
    }
    public enum LangFromType
    {
        Cookie,
        Header,
    }
    public enum TakeFromType
    {
        FromLeft,
        FromRight,
    }
    public enum PathFindSortType
    {
        Route_Url,
        Url_Route,
    }
    public enum LanguageType
    {
        None,
        zh_TW,
        zh_Hant,
        en_US,
    }
    public enum FileNameCaseType
    {
        Lower,
        Upper,
        HeadUpper,
    }
}