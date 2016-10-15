using System;
using System.Collections.Generic;

namespace Mvc.Models
{
	public class Select2Model
	{
		public IEnumerable<Select2Item> Items { get; set; }
	}

	public class Select2Item
	{
		public Guid Id { get; set; }
		public string StringItem { get; set; }
	}
}