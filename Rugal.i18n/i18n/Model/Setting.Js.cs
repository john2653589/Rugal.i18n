using Rugal.i18n.Core;
namespace Rugal.i18n.Model
{
    public partial class LangSetting
    {
        #region Js Setting Property
        public string JsRootPath { get; set; } = "Lang";
        public string JsViewPath { get; set; } = "View";
        public string JsSharedPath { get; set; } = "Shared";
        public bool IsRandomLoadJs { get; set; } = true;
        public FileNameCaseType JsFileNameCase { get; set; } = FileNameCaseType.Lower;
        private Func<LangModel, IEnumerable<string>> CustomJsPathFunc { get; set; }
        #endregion

        #region Public Process
        public IEnumerable<string> CustomJsPath(LangModel Model)
        {
            var JsPath = CustomJsPathFunc?.Invoke(Model);
            return JsPath;
        }
        #endregion

        #region With Js Set
        public LangSetting WithCustomJsPath(Func<LangModel, IEnumerable<string>> _CustomJsPathFunc)
        {
            CustomJsPathFunc = _CustomJsPathFunc;
            return this;
        }
        #endregion
    }
}