using Rugal.i18n.Core;

namespace Rugal.i18n.Model
{
    public partial class LangSetting
    {
        #region Json Setting Property
        public string JsonRootPath { get; set; } = "Lang";
        public string JsonViewPath { get; set; } = "View";
        public string JsonSharedPath { get; set; } = "Shared";
        public FileNameCaseType JsonFileNameCase { get; set; } = FileNameCaseType.Lower;
        public FileNameReplaceType JsonFileNameReplace { get; set; } = FileNameReplaceType.UnderLineToDash;
        private Func<LangModel, IEnumerable<string>> CustomJsonPathFunc { get; set; }
        #endregion

        #region Public Process
        public IEnumerable<string> CustomJsonPath(LangModel Model)
        {
            var JsPath = CustomJsonPathFunc?.Invoke(Model);
            return JsPath;
        }
        #endregion

        #region With Json Set
        public LangSetting WithCustomJsonPath(Func<LangModel, IEnumerable<string>> _CustomJsPathFunc)
        {
            CustomJsPathFunc = _CustomJsPathFunc;
            return this;
        }
        #endregion
    }
}
