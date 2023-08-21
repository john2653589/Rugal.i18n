namespace Rugal.i18n.Core
{
    public partial class LangModel
    {
        #region Public Property
        public LangJson JsonLang { get; set; }
        #endregion

        #region Public Method
        public virtual string LangPath_JsonView(string FolderName = null)
        {
            var PathResult = Setting.CustomJsonPath(this);
            PathResult ??= GetLangPath();

            var JsFileName = LangFileName_Json();
            return CombineFullJsonFilePath(JsFileName, Setting.JsonViewPath, PathResult);
        }
        public virtual string LangPath_JsonShared(string FolderName = null)
        {
            var JsFileName = LangFileName_Json();
            return CombineFullJsonFilePath(JsFileName, Setting.JsonSharedPath);
        }
        public virtual string Get(string LangKey) => JsonLang.Get(LangKey);
        #endregion

        #region Public Process
        public virtual string CombineFullJsonFilePath(string JsonFileName, string FolderName, IEnumerable<string> Paths = null)
        {
            var Result = CombineFullFilePath(JsonFileName, FolderName, Setting.JsonRootPath, ".", Paths);
            return Result;
        }
        #endregion

        #region Private Process
        private string LangFileName_Json()
        {
            var SetLanguage = FileNameCase(LanguageType, Setting.JsonFileNameCase, Setting.JsonFileNameReplace);
            var JsFile = $"{SetLanguage}.json";

            return JsFile;
        }
        private void InitJsonSet()
        {
            var SharedPath = LangPath_JsonShared();
            var ViewPath = LangPath_JsonView();
            JsonLang = new LangJson()
                .WithSetting(Setting)
                .WithJson(SharedPath)
                .WithJson(ViewPath);
        }
        #endregion
    }
}
