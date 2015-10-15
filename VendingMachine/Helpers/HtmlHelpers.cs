using System.IO;
using System.Web;
using System.Web.Mvc;

namespace VendingMachine.Helpers
{
    public static class HtmlHelpers
    {
        public static MvcHtmlString IncludeJs(this HtmlHelper helper)
        {
            string result = string.Empty;
            string controllerName = helper.ViewContext.RouteData.Values["controller"].ToString();
            string actionName = helper.ViewContext.RouteData.Values["action"].ToString();
            const string pattern = "<script type='text/javascript' src='{0}'></script>";
            string commonControllerJs = GetScriptPath(controllerName, controllerName, "js");
            if (File.Exists(HttpContext.Current.Server.MapPath(commonControllerJs)))
            {
                result += string.Format(pattern, commonControllerJs);
            }

            string actionJs = GetScriptPath(controllerName, actionName, "js");
            if (File.Exists(HttpContext.Current.Server.MapPath(actionJs)))
            {
                result += string.Format(pattern, actionJs);
            }

            return new MvcHtmlString(result);
        }


        private static string GetScriptPath(string controller, string file, string extention)
        {
            return string.Format("/Scripts/Pages/{0}/{1}.{2}", controller, file, extention);
        }
    }
}