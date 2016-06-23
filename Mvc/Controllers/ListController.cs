using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Mvc.Models;
using Tjs.Collections;
using Tjs.Web;

namespace Mvc.Controllers
{
	public class ListController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public ActionResult GetList(ListRequest request)
		{
			if (request == null) throw new ArgumentNullException(nameof(request));

			if (request.Mode == "error") throw new Exception("Debug: Something wrong with list!");

			int pageIndex = request.PageIndex ?? 1;
			int maxNumbers = request.PageSize ?? 5;
			int numbersToSkip = maxNumbers * (pageIndex - 1);

			var results = CreateList(request.Mode, request.SortRule, numbersToSkip, maxNumbers);

			var list = new PagedListWithMarkupViewModel<ListItem>
			{
				Items = results,
				Paging = PagingHelper.GetPaging(results.TotalCount, maxNumbers, pageIndex).ToArray(),
				ItemsHtml = ViewHelper.WritePartialViewToString(ControllerContext, "_List", results)
			};

			if (request.Mode == "long")
			{
				System.Threading.Thread.Sleep(5000);
			}
			return Json(list);
		}

		private IEnumerablePage<ListItem> CreateList(
			string mode,
			SortRule<ListSortType> sortRule,
			int numbersToSkip,
			int maxNumbers)
		{
			if (mode == "empty") return new ListPage<ListItem>(new List<ListItem>(0), 0);

			var query = StubData();

			if (mode == "top5") query = query.GetRange(0, 5);
			int totalCount = query.Count;
			var domains = Sort(query, sortRule).Skip(numbersToSkip).Take(maxNumbers).ToList();

			return new ListPage<ListItem>(domains, totalCount);
		}

		private IOrderedEnumerable<ListItem> Sort(IEnumerable<ListItem> query, SortRule<ListSortType> sortRule)
		{
			sortRule = sortRule ?? new SortRule<ListSortType>(ListSortType.ByDate, SortDirection.Desc);
			switch (sortRule.Type)
			{
				case ListSortType.ByString:
					return query.OrderBy(it => it.StringItem, sortRule.Direction);
				default:
					return query.OrderBy(it => it.DateItem, sortRule.Direction);
			}
		}

		private List<ListItem> StubData()
		{
			var result = new List<ListItem>
			{
				new ListItem
				{
					StringItem = "Test string 1",
					DateItem = new DateTimeViewModel(DateTime.Now)
				},
				new ListItem
				{
					StringItem = "A string 1",
					DateItem = new DateTimeViewModel(DateTime.Now.AddYears(-3))
				},
				new ListItem
				{
					StringItem = "Z string 1",
					DateItem = new DateTimeViewModel(DateTime.Now.AddYears(+5))
				},
				new ListItem
				{
					StringItem = "DH string 1",
					DateItem = new DateTimeViewModel(DateTime.Now.AddYears(1))
				}
			};

			for (int i = 0; i < 25; i++)
			{
				result.Add(
					new ListItem
					{
						StringItem = $"i string {i}",
						DateItem = new DateTimeViewModel(DateTime.Now.AddDays(i))
					});
			}
			return result;
		}
	}
}