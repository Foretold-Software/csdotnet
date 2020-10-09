// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.Collections.Generic;
using System.Linq;

namespace System.Collections.ObjectModel
{
	/// <summary>
	/// Provides members for implementing a distinct collection of values.
	/// </summary>
	/// <typeparam name="T">
	/// The type of the values in the distinct collection.
	/// </typeparam>
	public class DistinctCollection<T> : ListContainer<T>
	{
		#region Constructors
		/// <summary>
		/// Creates a new instance of <see cref="DistinctCollection{T}"/>.
		/// </summary>
		public DistinctCollection()
		{
			DistinctListItems = new List<T>();
		}

		/// <summary>
		/// Creates a new instance of <see cref="DistinctCollection{T}"/> with the given capacity.
		/// </summary>
		public DistinctCollection(int capacity)
		{
			DistinctListItems = new List<T>(capacity);
		}

		/// <summary>
		/// Creates a new instance of <see cref="DistinctCollection{T}"/> with the given values.
		/// </summary>
		public DistinctCollection(IEnumerable<T> collection)
		{
			DistinctListItems = new List<T>(collection.Distinct());
		}
		#endregion

		#region Properties
		/// <summary>
		/// The list of items being wrapped by this class.
		/// </summary>
		protected virtual List<T> DistinctListItems { get; set; }

		/// <summary>
		/// The list of items being wrapped by this class, redirected to the <see cref="DistinctListItems"/> property.
		/// </summary>
		protected override IList<T> ListItems
		{
			get { return DistinctListItems; }
			set { DistinctListItems = value as List<T>; }
		}
		#endregion

		#region Methods - Overridden
		/// <summary>
		/// Gets or sets the item at the specified index,
		/// unless the item already exists at another index.
		/// </summary>
		/// <param name="index">The zero-based index of the item to get or set.</param>
		/// <returns>Returns the item at the specified index.</returns>
		public override T this[int index]
		{
			get { return base[index]; }
			set
			{
				int currentIndex = DistinctListItems.IndexOf(value);

				if (currentIndex == -1 || currentIndex == index)
				{
					base[index] = value;
				}
				//else do nothing.
			}
		}

		/// <summary>
		/// Adds an item to the collection if it is not already present.
		/// </summary>
		/// <param name="item">The item to add.</param>
		public override void Add(T item)
		{
			if (-1 == DistinctListItems.IndexOf(item))
			{
				base.Add(item);
			}
			//else do nothing.
		}

		/// <summary>
		/// Inserts an item into the collection at the specified index,
		/// if the item is not already present.
		/// </summary>
		/// <param name="index">The index at which to insert the item.</param>
		/// <param name="item">The item to insert into the collection.</param>
		public override void Insert(int index, T item)
		{
			//Note: No error handling necessary.
			//      If the index is out of bounds, then let the list's insert method throw an exception.

			if (-1 == DistinctListItems.IndexOf(item))
			{
				base.Insert(index, item);
			}
			//else do nothing.
		}

		/// <summary>
		/// Returns a string that represents the current instance.
		/// </summary>
		/// <returns>Returns a string that represents the current instance.</returns>
		public override string ToString()
		{
			return "Count = " + Count;
		}
		#endregion
	}
}
