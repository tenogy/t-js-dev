using Tjs.Collections;
using Tjs.Web;

namespace Mvc.Models
{
	public class ListItem
	{
		public string StringItem { get; set; }
		public DateTimeViewModel DateItem { get; set; }
	}

	public class ListRequest
	{
		public string Mode { get; set; }
		public SortRule<ListSortType> SortRule { get; set; }
		public int? PageIndex { get; set; }
		public int? PageSize { get; set; }
	}

	public enum ListSortType
	{
		ByString = 0,
		ByDate = 1
	}
}