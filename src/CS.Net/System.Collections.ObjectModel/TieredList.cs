// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.Collections.Generic;
using System.Diagnostics;

namespace System.Collections.ObjectModel
{
	/// <summary>
	/// Provides members for implementing a tiered list of items.
	/// </summary>
	/// <typeparam name="T">
	/// The type of the item at each node in the tiered list.
	/// </typeparam>
	[DebuggerDisplay("Count = {Count}")]
	public class TieredList<T>
		: ListContainer<ITieredListItem<T>>
		, ITieredList<T>
		, IList<ITieredListItem<T>>, ICollection<ITieredListItem<T>>, IEnumerable<ITieredListItem<T>>
		, IList, ICollection, IEnumerable
	{
		#region Constructors
		/// <summary>
		/// Creates a new instance of <see cref="TieredList{T}"/>.
		/// </summary>
		public TieredList()
		{
			ListItems = new List<ITieredListItem<T>>();
		}

		/// <summary>
		/// Creates a new instance of <see cref="TieredList{T}"/>.
		/// </summary>
		/// <param name="owner">
		/// The tiered list item that owns this list as its children.
		/// </param>
		public TieredList(ITieredListItem<T> owner)
		{
			Owner = owner;
			ListItems = new List<ITieredListItem<T>>();
		}

		/// <summary>
		/// Creates a new instance of <see cref="TieredList{T}"/> that
		/// is empty but has an initial capacity of the amount specified.
		/// </summary>
		/// <param name="capacity">
		/// The initial capacity of the new list.
		/// </param>
		public TieredList(int capacity)
		{
			ListItems = new List<ITieredListItem<T>>(capacity);
		}

		/// <summary>
		/// Creates a new instance of <see cref="TieredList{T}"/> that
		/// is empty but has an initial capacity of the amount specified.
		/// </summary>
		/// <param name="owner">
		/// The tiered list item that owns this list as its children.
		/// </param>
		/// <param name="capacity">
		/// The initial capacity of the new list.
		/// </param>
		public TieredList(ITieredListItem<T> owner, int capacity)
		{
			Owner = owner;
			ListItems = new List<ITieredListItem<T>>(capacity);
		}

		/// <summary>
		/// Creates a new instance of <see cref="TieredList{T}"/> that
		/// is populated with items copied from the specified collection.
		/// </summary>
		/// <param name="collection">
		/// The collection whose items are copied to the new list.
		/// </param>
		public TieredList(IEnumerable<ITieredListItem<T>> collection)
		{
			ListItems = new List<ITieredListItem<T>>(collection);
		}

		/// <summary>
		/// Creates a new instance of <see cref="TieredList{T}"/> that
		/// is populated with items copied from the specified collection.
		/// </summary>
		/// <param name="owner">
		/// The tiered list item that owns this list as its children.
		/// </param>
		/// <param name="collection">
		/// The collection whose items are copied to the new list.
		/// </param>
		public TieredList(ITieredListItem<T> owner, IEnumerable<ITieredListItem<T>> collection)
		{
			if (collection != null)
			{
				foreach (var item in collection)
				{
					item.Parent = owner;
				}
			}

			Owner = owner;
			ListItems = new List<ITieredListItem<T>>(collection);
		}
		#endregion

		#region Properties
		/// <summary>
		/// The tiered list item that owns this list as
		/// its children, or null if this list is the root list.
		/// </summary>
		public virtual ITieredListItem<T> Owner { get; set; }

		/// <summary>
		/// Gets the number of tiers down from the root of the current list.
		/// A tier level of 0 means this is the root list.
		/// </summary>
		public int TierLevel
		{
			get
			{
				int tierLevel = 0;
				ITieredListItem<T> item = Owner;

				while (item != null)
				{
					tierLevel++;
					item = item.Parent;
				}

				return tierLevel;
			}
		}
		#endregion
	}
}
