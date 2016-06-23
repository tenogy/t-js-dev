using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Tjs.Collections
{
	public static class CollectionExtensions
	{
		public static IEnumerable<T> NotNull<T>(this IEnumerable<T> source) where T : class
		{
			return source.Where(e => e != null);
		}

		public static IEnumerable<T?> NotNull<T>(this IEnumerable<T?> source) where T : struct
		{
			return source.Where(e => e.HasValue);
		}

		public static IEnumerable<Indexified<T>> Indexify<T>(this IEnumerable<T> source)
		{
			return source.Select((e, i) => new Indexified<T> { Index = i, Value = e });
		}

		public static IEnumerable<T> TakeExact<T>(this IEnumerable<T> source, int count, T defaultItem)
		{
			return source.Concat(Enumerable.Repeat(defaultItem, count)).Take(count);
		}

		public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> list, int parts)
		{
			int i = 0;
			var splits = from item in list
				group item by i++ % parts
					into part
					select part.AsEnumerable();
			return splits;
		}

		public static Dictionary<TKey, IEnumerable<T>> Split<T, TKey>(
			this IEnumerable<T> source,
			Func<T, TKey> keySelector,
			IEqualityComparer<TKey> keyComparer = null
			)
		{
			keyComparer = keyComparer ?? EqualityComparer<TKey>.Default;
			return source.
				GroupBy(keySelector, keyComparer).
				ToDictionary(g => g.Key, g => g.AsEnumerable(), keyComparer);
		}

		public static Dictionary<TKey, IEnumerable<TGroupElement>> Split<T, TKey, TGroupElement>(
			this IEnumerable<T> source,
			Func<T, TKey> keySelector,
			Func<T, TGroupElement> groupElementSelector,
			IEqualityComparer<TKey> keyComparer = null
			)
		{
			keyComparer = keyComparer ?? EqualityComparer<TKey>.Default;
			return source.
				GroupBy(keySelector, groupElementSelector, keyComparer).
				ToDictionary(g => g.Key, g => g.AsEnumerable(), keyComparer);
		}

		public static Dictionary<TKey, TSplitGroup> SplitAndReduce<T, TKey, TSplitGroup>(
			this IEnumerable<T> source,
			Func<T, TKey> keySelector,
			Func<IEnumerable<T>, TSplitGroup> splitGroupSelector,
			IEqualityComparer<TKey> keyComparer = null
			)
		{
			keyComparer = keyComparer ?? EqualityComparer<TKey>.Default;
			return source.
				GroupBy(keySelector, keyComparer).
				ToDictionary(g => g.Key, g => splitGroupSelector(g), keyComparer);
		}

		public static Dictionary<TKey, TSplitGroup> SplitAndReduce<T, TKey, TSplitGroup>(
			this IEnumerable<T> source,
			Func<T, TKey> keySelector,
			Func<TKey, IEnumerable<T>, TSplitGroup> splitGroupSelector,
			IEqualityComparer<TKey> keyComparer = null
			)
		{
			keyComparer = keyComparer ?? EqualityComparer<TKey>.Default;
			return source.
				GroupBy(keySelector, _ => _, (key, groupItems) => new { key, groupItems }, keyComparer).
				ToDictionary(g => g.key, g => splitGroupSelector(g.key, g.groupItems), keyComparer);
		}

		public static IEnumerable<TSource> DistinctBy<TSource, TKey>
			(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
		{
			var seenKeys = new HashSet<TKey>();
			return source.Where(element => seenKeys.Add(keySelector(element)));
		}

		/// <summary>
		/// Batches the source sequence into sized buckets.
		/// </summary>
		/// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
		/// <param name="source">The source sequence.</param>
		/// <param name="size">Size of buckets.</param>
		/// <returns>A sequence of equally sized buckets containing elements of the source collection.</returns>
		/// <remarks> This operator uses deferred execution and streams its results (buckets and bucket content).</remarks>
		public static IEnumerable<IEnumerable<TSource>> Batch<TSource>(this IEnumerable<TSource> source, int size)
		{
			return Batch(source, size, x => x);
		}

		/// <summary>
		/// Batches the source sequence into sized buckets and applies a projection to each bucket.
		/// </summary>
		/// <typeparam name="TSource">Type of elements in <paramref name="source"/> sequence.</typeparam>
		/// <typeparam name="TResult">Type of result returned by <paramref name="resultSelector"/>.</typeparam>
		/// <param name="source">The source sequence.</param>
		/// <param name="size">Size of buckets.</param>
		/// <param name="resultSelector">The projection to apply to each bucket.</param>
		/// <returns>A sequence of projections on equally sized buckets containing elements of the source collection.</returns>
		/// <remarks> This operator uses deferred execution and streams its results (buckets and bucket content).</remarks>
		public static IEnumerable<TResult> Batch<TSource, TResult>(this IEnumerable<TSource> source, int size,
			Func<IEnumerable<TSource>, TResult> resultSelector)
		{
			return BatchImpl(source, size, resultSelector);
		}

		private static IEnumerable<TResult> BatchImpl<TSource, TResult>(this IEnumerable<TSource> source, int size,
			Func<IEnumerable<TSource>, TResult> resultSelector)
		{
			Debug.Assert(source != null);
			Debug.Assert(size > 0);
			Debug.Assert(resultSelector != null);

			TSource[] bucket = null;
			var count = 0;

			foreach (var item in source)
			{
				if (bucket == null)
				{
					bucket = new TSource[size];
				}

				bucket[count++] = item;

				// The bucket is fully buffered before it's yielded
				if (count != size)
				{
					continue;
				}

				// Select is necessary so bucket contents are streamed too
				yield return resultSelector(bucket.Select(x => x));

				bucket = null;
				count = 0;
			}

			// Return the last bucket with all remaining elements
			if (bucket != null && count > 0)
			{
				yield return resultSelector(bucket.Take(count));
			}
		}

		public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(
			this IEnumerable<TSource> source,
			Func<TSource, TKey> keySelector,
			SortDirection direstion)
		{
			return direstion == SortDirection.Asc ?
						source.OrderBy(keySelector) :
						source.OrderByDescending(keySelector);
		}

		public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(
			this IQueryable<TSource> source,
			System.Linq.Expressions.Expression<Func<TSource, TKey>> keySelector,
			SortDirection direstion)
		{
			return direstion == SortDirection.Asc ?
						source.OrderBy(keySelector) :
						source.OrderByDescending(keySelector);
		}

		public static TResult FirstOrDefault<T, TResult>(
			this IEnumerable<T> source,
			Func<T, bool> predicate,
			Func<T, TResult> resultSelector,
			TResult defaultValue
			)
			where TResult : class
		{
			return source.Where(predicate).Select(resultSelector).FirstOrDefault() ?? defaultValue;
		}

		public static TResult? FirstOrDefault<T, TResult>(
			this IEnumerable<T> source,
			Func<T, bool> predicate,
			Func<T, TResult?> resultSelector,
			TResult? defaultValue
			)
			where TResult : struct
		{
			return source.Where(predicate).Select(resultSelector).FirstOrDefault() ?? defaultValue;
		}

		public static IEnumerable<T> Prepend<T>(this IEnumerable<T> source, IEnumerable<T> prefix)
		{
			return prefix.Concat(source);
		}

		public static IEnumerable<T> Prepend<T>(this IEnumerable<T> source, T prefix)
		{
			return source.Prepend(new[] { prefix });
		}

		public static TValue Get<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
		{
			TValue val;
			return dict.TryGetValue(key, out val) ? val : default(TValue);
		}
	}


	public sealed class Indexified<T>
	{
		public int Index { get; set; }
		public T Value { get; set; }
	}
}
