using System.Collections.Generic;

namespace Tjs.Collections
{
	public interface IEnumerablePage<out T> : IEnumerable<T>
	{
		int TotalCount { get; }
	}
}
