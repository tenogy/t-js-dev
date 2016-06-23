using System.Collections.Generic;

namespace Tjs.Collections
{
	public class ListPage<T> : List<T>, IEnumerablePage<T>
	{
		public int TotalCount { get; }

		public ListPage(int totalCount)
		{
			TotalCount = totalCount;
		}

		public ListPage(IEnumerable<T> items, int totalCount) : base(items)
		{
			TotalCount = totalCount;
		}

		public static ListPage<T> Empty() => new ListPage<T>(0);
	}
}
