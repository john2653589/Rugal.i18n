using Microsoft.AspNetCore.Mvc;
using Rugal.i18n.Core;

namespace Rugal.i18n.js
{
    public class TestController : Controller
    {
        private readonly LangModel Lang;
        public TestController(LangModel _Lang)
        {
            Lang = _Lang;

            var c = Lang.Get("Domestic");
            var s = 1;
        }

        public ActionResult Index()
        {
            return View();
        }

    }
}
