// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

namespace System.Collections.Generic
{
	public static class _ICollection
	{
		/// <summary>
		/// Conditionally adds an item to the collection.
		/// </summary>
		/// <typeparam name="T">The type of the collection's elements and the item to add to it.</typeparam>
		/// <param name="collection">The collection to which the item will be added.</param>
		/// <param name="item">The item to add to the collection.</param>
		/// <param name="condition">A function that determines whether to add the item to the collection.</param>
		public static void AddIf<T>(this ICollection<T> collection, T item, Func<T, bool> condition)
		{
			if (condition(item))
			{
				collection.Add(item);
			}
		}

		/// <summary>
		/// Adds an item to the collection if it is not already present.
		/// </summary>
		/// <typeparam name="T">The type of the collection's elements and the item to add to it.</typeparam>
		/// <param name="collection">The collection to which the item will be added.</param>
		/// <param name="item">The item to add to the collection.</param>
		public static void AddIfNew<T>(this ICollection<T> collection, T item)
		{
			collection.AddIf(item, queriedItem => !collection.Contains(queriedItem));
		}

		/// <summary>
		/// Adds items to the collection.
		/// </summary>
		/// <typeparam name="T">The type of the collection's elements and the items to add.</typeparam>
		/// <param name="collection">The collection to which the items will be added.</param>
		/// <param name="items">The items to add to the collection.</param>
		public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
		{
			foreach (var item in items)
			{
				collection.Add(item);
			}
		}

		/// <summary>
		/// Adds items to the collection.
		/// </summary>
		/// <typeparam name="T">The type of the collection's elements and the items to add.</typeparam>
		/// <param name="collection">The collection to which the items will be added.</param>
		/// <param name="items">The items to add to the collection.</param>
		public static void AddRange<T>(this ICollection<T> collection, params T[] items)
		{
			foreach (var item in items)
			{
				collection.Add(item);
			}
		}

		/// <summary>
		/// Conditionally adds items to the collection.
		/// </summary>
		/// <typeparam name="T">The type of the collection's elements and the items to add.</typeparam>
		/// <param name="collection">The collection to which the items will be added.</param>
		/// <param name="items">The items to add to the collection.</param>
		/// <param name="condition">A function that determines whether to add each item to the collection.</param>
		public static void AddRangeIf<T>(this ICollection<T> collection, Func<T, bool> condition, IEnumerable<T> items)
		{
			foreach (var item in items)
			{
				if (condition(item))
				{
					collection.Add(item);
				}
			}
		}

		/// <summary>
		/// Conditionally adds items to the collection.
		/// </summary>
		/// <typeparam name="T">The type of the collection's elements and the items to add.</typeparam>
		/// <param name="collection">The collection to which the items will be added.</param>
		/// <param name="items">The items to add to the collection.</param>
		/// <param name="condition">A function that determines whether to add each item to the collection.</param>
		public static void AddRangeIf<T>(this ICollection<T> collection, Func<T, bool> condition, params T[] items)
		{
			foreach (var item in items)
			{
				if (condition(item))
				{
					collection.Add(item);
				}
			}
		}

		/// <summary>
		/// Adds items to the collection if they are not already present.
		/// </summary>
		/// <typeparam name="T">The type of the collection's elements and the items to add.</typeparam>
		/// <param name="collection">The collection to which the items will be added.</param>
		/// <param name="items">The items to add to the collection.</param>
		public static void AddRangeIfNew<T>(this ICollection<T> collection, IEnumerable<T> items)
		{
			collection.AddRangeIf(queriedItem => !collection.Contains(queriedItem), items);
		}

		/// <summary>
		/// Adds items to the collection if they are not already present.
		/// </summary>
		/// <typeparam name="T">The type of the collection's elements and the items to add.</typeparam>
		/// <param name="collection">The collection to which the items will be added.</param>
		/// <param name="items">The items to add to the collection.</param>
		public static void AddRangeIfNew<T>(this ICollection<T> collection, params T[] items)
		{
			collection.AddRangeIf(queriedItem => !collection.Contains(queriedItem), items);
		}
	}
}
