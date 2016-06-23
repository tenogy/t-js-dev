namespace Tjs.Collections
{
	public class SortRule<T>
	{
		public T Type { get; set; }
		public SortDirection Direction { get; set; }

		public SortRule()
		{
		}

		public SortRule(T type, SortDirection direction)
		{
			Type = type;
			Direction = direction;
		}
	}
}
