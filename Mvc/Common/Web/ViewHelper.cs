using System.IO;
using System.Web.Mvc;

namespace Tjs.Web
{
	public static class ViewHelper
	{
		public static string WritePartialViewToString<TViewModel>(ControllerContext controllerContext, string viewName, TViewModel model)
		{
			var viewData = new ViewDataDictionary(controllerContext.Controller.ViewData) { Model = model };
			var tempData = new TempDataDictionary();

			using (var writer = new StringWriter())
			{
				ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controllerContext, viewName);
				var viewContext = new ViewContext(controllerContext, viewResult.View, viewData, tempData, writer);
				viewResult.View.Render(viewContext, writer);
				return writer.GetStringBuilder().ToString();
			}
		}
	}
}