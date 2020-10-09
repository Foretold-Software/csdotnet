// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.Collections.Generic;

namespace System.Collections.ObjectModel
{
	/// <summary>
	/// Wraps an internal <see cref="IList{T}"/> and provides
	/// all the class members required to implement several list
	/// interfaces, redirecting their method calls to the internal
	/// instance of <see cref="IList{T}"/>.
	/// </summary>
	/// <typeparam name="T">
	/// The type of the items contained in the list.
	/// </typeparam>
	public abstract class ListContainer<T> : IList<T>, ICollection<T>, IEnumerable<T>, IList, ICollection, IEnumerable, IEquatable<ListContainer<T>>
	{
		#region Properties
		/// <summary>
		/// The list of items being wrapped by this class.
		/// </summary>
		protected virtual IList<T> ListItems { get; set; }
		#endregion

		#region Interface Members - Properties/Indexers
		/// <summary>
		/// Gets or sets the item at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the item to get or set.</param>
		/// <returns>Returns the item at the specified index.</returns>
		public virtual T this[int index]
		{
			get { return ListItems[index]; }
			set { ListItems[index] = value; }
		}

		/// <summary>
		/// Gets the number of elements contained in the collection.
		/// </summary>
		public virtual int Count
		{ get { return ListItems.Count; } }
		#endregion

		#region Interface Members - Methods
		/// <summary>
		/// Adds an item to the collection.
		/// </summary>
		/// <param name="item">The item to add.</param>
		public virtual void Add(T item)
		{
			ListItems.Add(item);
		}

		/// <summary>
		/// Removes all items from the collection.
		/// </summary>
		public virtual void Clear()
		{
			ListItems.Clear();
		}

		/// <summary>
		/// Determines whether the collection contains a specific value.
		/// </summary>
		/// <param name="item">The value to locate in the collection.</param>
		/// <returns>Returns true if the item is found, otherwise false.</returns>
		public virtual bool Contains(T item)
		{
			return ListItems.Contains(item);
		}

		/// <summary>
		/// Copies the items of the collection to an array,
		/// starting at a particular array index.
		/// </summary>
		/// <param name="array">The one-dimensional array that is the destination of the copy procedure.</param>
		/// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
		public virtual void CopyTo(T[] array, int arrayIndex)
		{
			ListItems.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Gets an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>Returns an enumerator that iterates through the collection.</returns>
		public virtual IEnumerator<T> GetEnumerator()
		{
			return ListItems.GetEnumerator();
		}

		/// <summary>
		/// Finds the index of the specified item in the collection.
		/// </summary>
		/// <param name="item">The item to find.</param>
		/// <returns>Returns the index of item if it is found, otherwise returns -1.</returns>
		public virtual int IndexOf(T item)
		{
			return ListItems.IndexOf(item);
		}

		/// <summary>
		/// Inserts an item into the collection at the specified index.
		/// </summary>
		/// <param name="index">The index at which to insert the item.</param>
		/// <param name="item">The item to insert into the collection.</param>
		public virtual void Insert(int index, T item)
		{
			ListItems.Insert(index, item);
		}

		/// <summary>
		/// Removes the first occurrence of a specific item from the collection.
		/// </summary>
		/// <param name="item">The item to remove from the collection.</param>
		/// <returns>
		/// Returns true if the item was successfully removed from the collection, otherwise false.
		/// This method also returns false if item was not found in the collection.
		/// </returns>
		public virtual bool Remove(T item)
		{
			return ListItems.Remove(item);
		}

		/// <summary>
		/// Removes the item at the specified index.
		/// </summary>
		/// <param name="index">The index at which to remove the item.</param>
		public virtual void RemoveAt(int index)
		{
			ListItems.RemoveAt(index);
		}

		/// <summary>
		/// Indicates whether the current collection is the same instance as the other collection.
		/// </summary>
		/// <param name="obj">
		/// An object instance to compare with this object.
		/// </param>
		/// <returns>
		/// Returns true if the current instance is equal to the other instance, otherwise false.
		/// </returns>
		public override bool Equals(object obj)
		{
			return Equals(obj as ListContainer<T>);
		}

		/// <summary>
		/// Indicates whether the current collection is the same instance as the other collection.
		/// </summary>
		/// <param name="other">
		/// An object instance to compare with this object.
		/// </param>
		/// <returns>
		/// Returns true if the current instance is equal to the other instance, otherwise false.
		/// </returns>
		public bool Equals(ListContainer<T> other)
		{
			if (other == null)
			{
				return false;
			}
			else
			{
				return ReferenceEquals(this, other);
			}
		}

		/// <summary>
		/// Serves as a hash function for a particular type.
		/// </summary>
		/// <returns>
		/// Returns a hash code for the current instance.
		/// </returns>
		public override int GetHashCode()
		{
			return ListItems.GetHashCode();
		}

		/// <summary>
		/// Returns a string that represents the current collection instance.
		/// </summary>
		/// <returns>Returns a string that represents the current collection instance.</returns>
		public override string ToString()
		{
			return string.Format("Count = {0}", Count);
		}
		#endregion

		#region Interface Members - Hidden
		object IList.this[int index]
		{
			get { return ListItems[index]; }
			set { ((IList)ListItems)[index] = value; }
		}

		bool IList.IsFixedSize
		{ get { return ((IList)ListItems).IsFixedSize; } }

		bool IList.IsReadOnly
		{ get { return ((IList)ListItems).IsReadOnly; } }

		bool ICollection<T>.IsReadOnly
		{ get { return ((IList<T>)ListItems).IsReadOnly; } }

		bool ICollection.IsSynchronized
		{ get { return ((IList)ListItems).IsSynchronized; } }

		object ICollection.SyncRoot
		{ get { return ((IList)ListItems).SyncRoot; } }

		int IList.Add(object value)
		{
			return ((IList)ListItems).Add(value);
		}

		bool IList.Contains(object value)
		{
			return ((IList)ListItems).Contains(value);
		}

		void ICollection.CopyTo(Array array, int index)
		{
			((IList)ListItems).CopyTo(array, index);
		}

		int IList.IndexOf(object value)
		{
			return ((IList)ListItems).IndexOf(value);
		}

		void IList.Insert(int index, object value)
		{
			((IList)ListItems).Insert(index, value);
		}

		void IList.Remove(object value)
		{
			((IList)ListItems).Remove(value);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ListItems.GetEnumerator();
		}
		#endregion
	}
}
