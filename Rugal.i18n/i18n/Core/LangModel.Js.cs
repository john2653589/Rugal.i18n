using Microsoft.AspNetCore.Mvc.RazorPages;
using Rugal.i18n.Model;

namespace Rugal.i18n.Core
{
    public partial class LangModel
    {
        #region Public Method
        public virtual string LangPath_JsView()
        {
            var PathResult = Setting.CustomJsPath(this);
            PathResult ??= GetLangPath();

            var JsFileName = LangFileName_Js();
            return CombineFullJsFilePath(JsFileName, Setting.JsViewPath, PathResult);
        }
        public virtual string LangPath_JsShared()
        {
            var JsFileName = LangFileName_Js();
            return CombineFullJsFilePath(JsFileName, Setting.JsonSharedPath);
        }
        #endregion

        #region Public Process
        public virtual string CombineFullJsFilePath(string JsFileName, string FolderName, IEnumerable<string> Paths = null)
        {
            var Result = CombineFullFilePath(JsFileName, FolderName, Setting.JsRootPath, "/", Paths);
            return Result;
        }
        #endregion

        #region Private Process
        private string LangFileName_Js()
        {
            var SetLanguage = Language ?? Setting.DefaultLanguage;
            SetLanguage = FileNameCase(SetLanguage, Setting.JsFileNameCase);
            
            var JsFile = $"{SetLanguage}.js";
            if (Setting.IsRandomLoadJs)
                JsFile += $"?id={Guid.NewGuid()}";

            return JsFile;
        }
        private IEnumerable<string> GetLangPath()
        {
            var GetUrlPath = UrlPath;
            var GetRoutePath = RoutePath;

            IEnumerable<string> PathResult = null;
            if (Setting.PathFindSort == PathFindSortType.Route_Url)
            {
                PathResult ??= GetRoutePath;
                PathResult ??= GetUrlPath;
            }
            else if (Setting.PathFindSort == PathFindSortType.Url_Route)
            {
                PathResult ??= GetUrlPath;
                PathResult ??= GetRoutePath;
            }
            return PathResult;
        }
        #endregion
    }
}