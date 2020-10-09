// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace System.Collections.ObjectModel
{
	/// <summary>
	/// Provides members for implementing a tiered list of items.
	/// </summary>
	/// <typeparam name="T">
	/// The type of the item at each node in the tiered list.
	/// </typeparam>
	[DebuggerDisplay("{Item}, Count = {Count}")]
	public class TieredListItem<T>
		: ListContainer<ITieredListItem<T>>
		, ITieredListItem<T>
		, IList<ITieredListItem<T>>, ICollection<ITieredListItem<T>>, IEnumerable<ITieredListItem<T>>
		, IList, ICollection, IEnumerable
	{
		#region Constructors
		/// <summary>
		/// Creates a new instance of <see cref="TieredList{T}"/>.
		/// </summary>
		public TieredListItem()
		{
			Children = new TieredList<T>(this);
		}

		/// <summary>
		/// Creates a new instance of <see cref="TieredList{T}"/>.
		/// </summary>
		/// <param name="item">
		/// The object represented by this <see cref="TieredListItem{T}"/>.
		/// </param>
		public TieredListItem(T item)
		{
			Item = item;
			Children = new TieredList<T>(this);
		}

		/// <summary>
		/// Creates a new instance of <see cref="TieredList{T}"/>.
		/// </summary>
		/// <param name="item">
		/// The object represented by this <see cref="TieredListItem{T}"/>.
		/// </param>
		/// <param name="parent">
		/// The <see cref="TieredListItem{T}"/> containing this object.
		/// </param>
		public TieredListItem(T item, TieredListItem<T> parent)
		{
			Item = item;
			Parent = parent;
			Children = new TieredList<T>(this);
		}

		/// <summary>
		/// Creates a new instance of <see cref="TieredList{T}"/> that
		/// is empty but has an initial capacity of the amount specified.
		/// </summary>
		/// <param name="item">
		/// The object represented by this <see cref="TieredListItem{T}"/>.
		/// </param>
		/// <param name="capacity">
		/// The initial capacity of the new list.
		/// </param>
		public TieredListItem(T item, int capacity)
		{
			Item = item;
			Children = new TieredList<T>(this, capacity);
		}

		/// <summary>
		/// Creates a new instance of <see cref="TieredList{T}"/> that
		/// is empty but has an initial capacity of the amount specified.
		/// </summary>
		/// <param name="item">
		/// The object represented by this <see cref="TieredListItem{T}"/>.
		/// </param>
		/// <param name="parent">
		/// The <see cref="TieredListItem{T}"/> containing this object.
		/// </param>
		/// <param name="capacity">
		/// The initial capacity of the new list.
		/// </param>
		public TieredListItem(T item, TieredListItem<T> parent, int capacity)
		{
			Item = item;
			Parent = parent;
			Children = new TieredList<T>(this, capacity);
		}

		/// <summary>
		/// Creates a new instance of <see cref="TieredList{T}"/> that
		/// is populated with items copied from the specified collection.
		/// </summary>
		/// <param name="item">
		/// The object represented by this <see cref="TieredListItem{T}"/>.
		/// </param>
		/// <param name="children">
		/// The collection whose items are copied to the new list.
		/// </param>
		public TieredListItem(T item, IEnumerable<T> children)
		{
			Item = item;
			Children = new TieredList<T>(this, children.Select(child => new TieredListItem<T>(child) as ITieredListItem<T>));
		}

		/// <summary>
		/// Creates a new instance of <see cref="TieredList{T}"/> that
		/// is populated with items copied from the specified collection.
		/// </summary>
		/// <param name="item">
		/// The object represented by this <see cref="TieredListItem{T}"/>.
		/// </param>
		/// <param name="parent">
		/// The <see cref="TieredListItem{T}"/> containing this object.
		/// </param>
		/// <param name="children">
		/// The collection whose items are copied to the new list.
		/// </param>
		public TieredListItem(T item, TieredListItem<T> parent, IEnumerable<T> children)
		{
			Item = item;
			Parent = parent;
			Children = new TieredList<T>(this, children.Select(child => new TieredListItem<T>(child) as ITieredListItem<T>));
		}

		/// <summary>
		/// Creates a new instance of <see cref="TieredList{T}"/> that
		/// is populated with items copied from the specified collection.
		/// </summary>
		/// <param name="item">
		/// The object represented by this <see cref="TieredListItem{T}"/>.
		/// </param>
		/// <param name="children">
		/// The collection whose items are copied to the new list.
		/// </param>
		public TieredListItem(T item, IEnumerable<TieredListItem<T>> children)
		{
			Item = item;
			Children = new TieredList<T>(this, children.Cast<ITieredListItem<T>>());
		}

		/// <summary>
		/// Creates a new instance of <see cref="TieredList{T}"/> that
		/// is populated with items copied from the specified collection.
		/// </summary>
		/// <param name="item">
		/// The object represented by this <see cref="TieredListItem{T}"/>.
		/// </param>
		/// <param name="parent">
		/// The <see cref="TieredListItem{T}"/> containing this object.
		/// </param>
		/// <param name="children">
		/// The collection whose items are copied to the new list.
		/// </param>
		public TieredListItem(T item, TieredListItem<T> parent, IEnumerable<TieredListItem<T>> children)
		{
			Item = item;
			Parent = parent;
			Children = new TieredList<T>(this, children.Cast<ITieredListItem<T>>());
		}
		#endregion

		#region Properties
		/// <summary>
		/// The item at the current node in the tiered list.
		/// </summary>
		public T Item { get; set; }

		/// <summary>
		/// The item that is the tiered parent of the item at the current node.
		/// </summary>
		public TieredListItem<T> Parent { get; set; }

		/// <summary>
		/// The items that are tiered children of the item at the current node.
		/// </summary>
		public virtual TieredList<T> Children { get; set; }

		/// <summary>
		/// The list of items being wrapped by this class.
		/// This property receives list item queries, and redirects
		/// them to the <see cref="Children"/> property.
		/// </summary>
		protected override IList<ITieredListItem<T>> ListItems
		{
			get { return Children; }
			set { Children = value as TieredList<T>; }
		}

		/// <summary>
		/// Gets the number of tiers down from the root of the current list item.
		/// A tier level of 0 means this item is at the root.
		/// </summary>
		public int TierLevel
		{
			get
			{
				int tierLevel = 0;
				TieredListItem<T> item = this;

				while (null != (item = item.Parent))
				{
					tierLevel++;
				}

				return tierLevel;
			}
		}
		#endregion

		#region Operators
		/// <summary>
		/// Converts a <see cref="TieredListItem{T}"/> to a <see cref="T"/> by
		/// returning the value of the <see cref="Item"/> property.
		/// </summary>
		/// <param name="tieredListItem">
		/// The instance of <see cref="TieredListItem{T}"/> to convert.
		/// </param>
		public static implicit operator T(TieredListItem<T> tieredListItem)
		{
			return tieredListItem.Item;
		}

		/// <summary>
		/// Converts a <see cref="TieredListItem{T}"/> to a string by
		/// calling the <see cref="ToString"/> method.
		/// </summary>
		/// <param name="tieredListItem">
		/// The instance of <see cref="TieredListItem{T}"/> to convert.
		/// </param>
		public static implicit operator string(TieredListItem<T> tieredListItem)
		{
			return tieredListItem.ToString();
		}

		/// <summary>
		/// Converts a <see cref="TieredListItem{T}"/> to a string by
		/// calling the <see cref="object.ToString"/> method of its
		/// <see cref="Item"/> property.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return Item.ToString();
		}
		#endregion

		#region Interface Members - Hidden
		/// <summary>
		/// The item that is the tiered parent of the item at the current node.
		/// </summary>
		ITieredListItem<T> ITieredListItem<T>.Parent
		{
			get { return Parent; }
			set { Parent = (TieredListItem<T>)value; }
		}
		#endregion
	}
}
