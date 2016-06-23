using System.Web.Mvc;
using Newtonsoft.Json;

namespace Tjs.Web
{
	public static class HtmlHelperExtentions
	{
		public static MvcHtmlString ToJson(this HtmlHelper html, object obj)
		{
			return new MvcHtmlString(JsonConvert.SerializeObject(obj));
		}
	}
}