using System.Collections.Generic;

namespace Tjs.Collections
{
	public class ListWithMarkupViewModel<TListItem>
	{
		public IEnumerable<TListItem> Items { get; set; }
		public string ItemsHtml { get; set; }
	}

	public class PagedListWithMarkupViewModel<TListItem> : ListWithMarkupViewModel<TListItem>
	{
		public IEnumerable<PagingItem> Paging { get; set; }
	}
}
