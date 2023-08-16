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
        public string CookieLangKey { get; set; } = DefaultCookieLangKey;
        public string HeaderLangKey { get; set; } = DefaultHeaderLangKey;
        public bool IsRandomLoad { get; set; } = true;
        public LangFromType LangFrom { get; set; } = LangFromType.Header;
        public PathFindSortType PathFindSort { get; set; } = PathFindSortType.Url_Route;
        public RouteTakeFromType RouteTakeFrom { get; set; } = RouteTakeFromType.FromLeft;
        private Func<string, string> CookieFormatFunc { get; set; }
        private Func<string, string> HeaderFormatFunc { get; set; }
        #endregion

        #region Route Setting
        public int RouteSkipCount { get; set; } = 0;
        public int RouteTakeCount { get; set; } = 5;
        #endregion

        #region With Lang Set
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

        #region With Route Set
        public LangSetting WithRouteSkipCount(int _RouteSkipCount)
        {
            RouteSkipCount = _RouteSkipCount;
            return this;
        }
        public LangSetting WithRouteTakeCount(int _RouteTakeCount)
        {
            RouteTakeCount = _RouteTakeCount;
            return this;
        }
        public LangSetting WithPathFindSort(PathFindSortType _PathFindSort)
        {
            PathFindSort = _PathFindSort;
            return this;
        }
        public LangSetting WithRouteTakeFrom(RouteTakeFromType _RouteTakeFrom)
        {
            RouteTakeFrom = _RouteTakeFrom;
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
        #endregion
    }
    public enum LangFromType
    {
        Cookie,
        Header,
    }
    public enum RouteTakeFromType
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