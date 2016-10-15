using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Mvc.Models;

namespace Mvc.Controllers
{
	public class Select2Controller : Controller
	{
		public ActionResult Index()
		{
			return View(new Select2Model
			{
				Items = StubData()
			});
		}

		private ICollection<Select2Item> StubData()
		{
			var result = new List<Select2Item>
			{
				new Select2Item
				{
					Id = Guid.NewGuid(),
					StringItem = "Test string 1"
				},
				new Select2Item
				{
					Id = Guid.NewGuid(),
					StringItem = "A string 1"
				},
				new Select2Item
				{
					Id = Guid.NewGuid(),
					StringItem = "Z string 1"
				},
				new Select2Item
				{
					Id = Guid.NewGuid(),
					StringItem = "DH string 1"
				}
			};

			return result;
		}
	}
}