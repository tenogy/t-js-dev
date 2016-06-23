using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Tjs.Collections
{
	public static class PagingHelper
	{
		public delegate string PaginItemCaptionEvaluator(int index, PagingItemType type, int pageSize);

		public static int GetPageByItemIndex(int itemIndex, int total, int pageSize)
		{
			if (pageSize <= 0) throw new ArgumentOutOfRangeException("pageSize");
			return AdaptPageIndex(itemIndex / pageSize + 1, total, pageSize);
		}

		public static int AdaptPageIndex(int pageIndex, int total, int pageSize)
		{
			if (pageSize <= 0) throw new ArgumentOutOfRangeException("pageSize");
			if (total <= 0) return 0;

			int pagesCount = total / pageSize + (total % pageSize > 0 ? 1 : 0);
			if (pageIndex < 1) pageIndex = 1;
			if (pageIndex > pagesCount) pageIndex = pagesCount;
			return pageIndex;
		}

		public static IEnumerable<PagingItem> GetPaging(int total, int pageSize, int activePageIndex)
		{
			return GetPaging(total, pageSize, activePageIndex, PageNumberAsCaption);
		}

		public static IEnumerable<PagingItem> GetPaging(
			int total,
			int pageSize,
			int activePageIndex,
			PaginItemCaptionEvaluator captionEvaluator
			)
		{
			if (pageSize <= 0) throw new ArgumentOutOfRangeException("pageSize");

			if (total <= 0) return Enumerable.Empty<PagingItem>();

			int pagesCount = total / pageSize + (total % pageSize > 0 ? 1 : 0);
			if (activePageIndex < 1) activePageIndex = 1;
			if (activePageIndex > pagesCount) activePageIndex = pagesCount;

			const int delta = 4;
			int lastIndex = pagesCount;
			int from = Math.Max(1, activePageIndex - delta);
			int to = Math.Min(lastIndex, activePageIndex + delta);

			IEnumerable<PagingItem> pagingItems = Enumerable.
				Range(from, to - from + 1).
				Select(
					index =>
					{
						var type = PagingItemType.Ordinal;
						if (index == activePageIndex) type = PagingItemType.Active;
						else if (index == 1) type = PagingItemType.First;
						else if (index == lastIndex) type = PagingItemType.Last;
						else if (index == activePageIndex - delta && from > 2) type = PagingItemType.EtcBack;
						else if (index == activePageIndex + delta && to < pagesCount - 1) type = PagingItemType.EtcForward;
						return new PagingItem
						{
							Index = index,
							Active = (type == PagingItemType.Active),
							Caption = captionEvaluator(index, type, pageSize)
						};
					}
					);
			if (from > 1)
			{
				var firstPagingItem = new PagingItem(1, captionEvaluator(1, PagingItemType.First, pageSize), false);
				pagingItems = Enumerable.Repeat(firstPagingItem, 1).Concat(pagingItems);
			}
			if (to < lastIndex)
			{
				var lastPagingItem = new PagingItem(lastIndex, captionEvaluator(lastIndex, PagingItemType.Last, pageSize), false);
				pagingItems = pagingItems.Concat(Enumerable.Repeat(lastPagingItem, 1));
			}
			return pagingItems.ToList();
		}

		public static string PageNumberAsCaption(int index, PagingItemType type, int pageSize)
		{
			if (type == PagingItemType.EtcBack || type == PagingItemType.EtcForward) return "...";
			return index.ToString(CultureInfo.CurrentCulture);
		}

		public static IEnumerable<T> Pagify<T>(T[] source, int? page, int pageSize, out PagingItem[] paging)
		{
			return Pagify(source, page ?? 1, pageSize, out paging);
		}

		public static IEnumerable<T> Pagify<T>(T[] source, int page, int pageSize, out PagingItem[] paging)
		{
			int totalCount = source.Count();
			page = AdaptPageIndex(page, totalCount, pageSize);
			paging = GetPaging(totalCount, pageSize, page).ToArray();
			return source.Skip((page - 1) * pageSize).Take(pageSize);
		}


		public enum PagingItemType
		{
			Ordinal,
			First,
			Last,
			Active,
			EtcBack,
			EtcForward
		}
	}

	public class PagingItem
	{
		public bool Active { get; set; }
		public int Index { get; set; }
		public string Caption { get; set; }

		public PagingItem()
		{
		}

		public PagingItem(int index, string caption, bool active)
		{
			Index = index;
			Active = active;
			Caption = caption;
		}
	}
}
