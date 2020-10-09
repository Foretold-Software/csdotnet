// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace System.Collections.ObjectModel
{
	/// <summary>
	/// Provides members for implementing a recursive collection of values.
	/// </summary>
	/// <typeparam name="T">
	/// The type of the value at each node in the recursive collection.
	/// </typeparam>
	public class ObservableRecursiveCollection<T> : ListContainer<ObservableRecursiveCollection<T>>, INotifyCollectionChanged, INotifyPropertyChanged
	{
		#region Constructors
		/// <summary>
		/// Creates a new instance of <see cref="ObservableRecursiveCollection{T}"/>.
		/// </summary>
		public ObservableRecursiveCollection()
		{
			Children = new List<ObservableRecursiveCollection<T>>();
		}

		/// <summary>
		/// Creates a new instance of <see cref="ObservableRecursiveCollection{T}"/>.
		/// </summary>
		/// <param name="value">
		/// The value at this node of the <see cref="ObservableRecursiveCollection{T}"/>.
		/// </param>
		public ObservableRecursiveCollection(T value)
		{
			Value = value;
			Children = new List<ObservableRecursiveCollection<T>>();
		}

		/// <summary>
		/// Creates a new instance of <see cref="ObservableRecursiveCollection{T}"/>.
		/// </summary>
		/// <param name="value">
		/// The value at this node of the <see cref="ObservableRecursiveCollection{T}"/>.
		/// </param>
		/// <param name="parent">
		/// The collection of items that is the hierarchical parent of this collection.
		/// </param>
		public ObservableRecursiveCollection(T value, ObservableRecursiveCollection<T> parent)
		{
			Value = value;
			Parent = parent;
			Children = new List<ObservableRecursiveCollection<T>>();
		}

		/// <summary>
		/// Creates a new instance of <see cref="ObservableRecursiveCollection{T}"/>.
		/// </summary>
		/// <param name="value">
		/// The value at this node of the <see cref="ObservableRecursiveCollection{T}"/>.
		/// </param>
		/// <param name="childItems">
		/// A collection of value to initialize this collection with, as
		/// the hierarchical children of this node.
		/// </param>
		public ObservableRecursiveCollection(T value, params T[] childItems)
		{
			Value = value;
			Children = new List<ObservableRecursiveCollection<T>>(childItems.Select(childItem => new ObservableRecursiveCollection<T>(childItem, this)));
		}

		/// <summary>
		/// Creates a new instance of <see cref="ObservableRecursiveCollection{T}"/>.
		/// </summary>
		/// <param name="value">
		/// The value at this node of the <see cref="ObservableRecursiveCollection{T}"/>.
		/// </param>
		/// <param name="parent">
		/// The collection of items that is the hierarchical parent of this collection.
		/// </param>
		/// <param name="childItems">
		/// A collection of value to initialize this collection with, as
		/// the hierarchical children of this node.
		/// </param>
		public ObservableRecursiveCollection(T value, ObservableRecursiveCollection<T> parent, params T[] childItems)
		{
			Value = value;
			Parent = parent;
			Children = new List<ObservableRecursiveCollection<T>>(childItems.Select(childItem => new ObservableRecursiveCollection<T>(childItem, this)));
		}

		/// <summary>
		/// Creates a new instance of <see cref="ObservableRecursiveCollection{T}"/>.
		/// </summary>
		/// <param name="value">
		/// The value at this node of the <see cref="ObservableRecursiveCollection{T}"/>.
		/// </param>
		/// <param name="childItems">
		/// A collection of value to initialize this collection with, as
		/// the hierarchical children of this node.
		/// </param>
		public ObservableRecursiveCollection(T value, IEnumerable<T> childItems)
		{
			Value = value;
			Children = new List<ObservableRecursiveCollection<T>>(childItems.Select(childItem => new ObservableRecursiveCollection<T>(childItem, this)));
		}

		/// <summary>
		/// Creates a new instance of <see cref="ObservableRecursiveCollection{T}"/>.
		/// </summary>
		/// <param name="value">
		/// The value at this node of the <see cref="ObservableRecursiveCollection{T}"/>.
		/// </param>
		/// <param name="parent">
		/// The collection of items that is the hierarchical parent of this collection.
		/// </param>
		/// <param name="childItems">
		/// A collection of value to initialize this collection with, as
		/// the hierarchical children of this node.
		/// </param>
		public ObservableRecursiveCollection(T value, ObservableRecursiveCollection<T> parent, IEnumerable<T> childItems)
		{
			Value = value;
			Parent = parent;
			Children = new List<ObservableRecursiveCollection<T>>(childItems.Select(childItem => new ObservableRecursiveCollection<T>(childItem, this)));
		}
		#endregion

		#region Events
		/// <summary>
		/// Occurs when the collection changes.
		/// </summary>
		public event NotifyCollectionChangedEventHandler CollectionChanged;

		/// <summary>
		/// Occurs when a property value changes.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

		#region Fields
		private T _Value;
		private ObservableRecursiveCollection<T> _Parent;
		#endregion
		
		#region Properties
		/// <summary>
		/// The value at this node of the <see cref="ObservableRecursiveCollection{T}"/>.
		/// </summary>
		public virtual T Value
		{
			get { return _Value; }
			set
			{
				if (!object.Equals(value, _Value))
				{
					_Value = value;
					OnPropertyChanged(nameof(Value));
				}
			}
		}

		/// <summary>
		/// The collection of items that is the hierarchical parent of this collection.
		/// </summary>
		public virtual ObservableRecursiveCollection<T> Parent
		{
			get { return _Parent; }
			protected set
			{
				if (!object.Equals(value, _Parent))
				{
					_Parent = value;
					OnPropertyChanged(nameof(Parent));
				}
			}
		}

		/// <summary>
		/// The list of items being wrapped by this class.
		/// </summary>
		protected virtual List<ObservableRecursiveCollection<T>> Children { get; set; }

		/// <summary>
		/// The list of items being wrapped by this class, redirected to the <see cref="Children"/> property.
		/// </summary>
		protected override IList<ObservableRecursiveCollection<T>> ListItems
		{
			get { return Children; }
			set { Children = value as List<ObservableRecursiveCollection<T>>; }
		}
		#endregion

		#region Event Trigger Methods
		protected virtual void OnPropertyChanged(string propertyName)
		{
			OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
		{
			PropertyChanged?.Invoke(this, args);
		}

		protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
		{
			CollectionChanged?.Invoke(this, args);
		}
		#endregion

		#region Methods
		/// <summary>
		/// Creates a new instance of <see cref="ObservableRecursiveCollection{T}"/> with the
		/// specified value as its <see cref="Value"/>, and adds it into the collection.
		/// </summary>
		/// <param name="value">The value of the item to add to the collection.</param>
		public virtual void Add(T value)
		{
			Add(new ObservableRecursiveCollection<T>(value));
		}

		/// <summary>
		/// Determines whether the collection contains an item with the specified value.
		/// </summary>
		/// <param name="value">The value of the item to locate in the collection.</param>
		/// <returns>Returns true if the item with the specified value is found, otherwise false.</returns>
		public bool Contains(T value)
		{
			return ListItems.Any(childItem => object.Equals(value, childItem.Value));
		}

		/// <summary>
		/// Finds the index of the first item in the collection with the specified value.
		/// </summary>
		/// <param name="value">The value of the item to locate in the collection.</param>
		/// <returns>
		/// Returns the index of the first occurence of an item with the
		/// specified value if it is found, otherwise returns -1.
		/// </returns>
		public int IndexOf(T value)
		{
			Predicate<T> predicate = childItemValue => object.Equals(value, childItemValue);

			return FindIndex(predicate);
		}

		/// <summary>
		/// Finds the index of the first item that matches the given criteria.
		/// </summary>
		/// <param name="match">
		/// The condition that must evaluate to true for a match to be found.
		/// </param>
		/// <returns>
		/// Returns the index of the first item in the collection that matches the
		/// given criteria. If none are found or the collection is empty, returns -1.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// Thrown if the match argument is null.
		/// </exception>
		public int FindIndex(Predicate<ObservableRecursiveCollection<T>> match)
		{
			if (match == null)
			{
				throw new ArgumentNullException("match", "The match predicate cannot be null.");
			}
			else
			{
				for (int i = 0; i < ListItems.Count; i++)
				{
					if (match(ListItems[i]))
					{
						return i;
					}
				}

				return -1;
			}
		}

		/// <summary>
		/// Finds the index of the first non-null item whose value matches the given criteria.
		/// </summary>
		/// <param name="match">
		/// The condition that must evaluate to true for a match to be found.
		/// </param>
		/// <returns>
		/// Returns the index of the first item in the collection whose value matches the
		/// given criteria. If none are found or the collection is empty, returns -1.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// Thrown if the match argument is null.
		/// </exception>
		public int FindIndex(Predicate<T> match)
		{
			if (match == null)
			{
				throw new ArgumentNullException("match", "The match predicate cannot be null.");
			}
			else
			{
				for (int i = 0; i < ListItems.Count; i++)
				{
					if (ListItems[i] != null && match(ListItems[i].Value))
					{
						return i;
					}
				}

				return -1;
			}
		}

		/// <summary>
		/// Creates a new instance of <see cref="ObservableRecursiveCollection{T}"/> with the
		/// specified value as its <see cref="Value"/>, and inserts it into the collection.
		/// </summary>
		/// <param name="index">The index at which to insert the item.</param>
		/// <param name="value">The value of the item to insert into the collection.</param>
		public virtual void Insert(int index, T value)
		{
			Insert(index, new ObservableRecursiveCollection<T>(value));
		}
		#endregion

		#region Methods - Overridden
		/// <summary>
		/// Gets or sets the item at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the item to get or set.</param>
		/// <returns>Returns the item at the specified index.</returns>
		public override ObservableRecursiveCollection<T> this[int index]
		{
			get { return base[index]; }
			set
			{
				//Note: No error handling necessary.
				//      If the index is out of bounds, then let the list's indexer throw an exception.
				var oldItem = ListItems[index];

				base[index] = value;

				OnPropertyChanged(Binding.IndexerName);

				//Note: We must cast the item to type 'object' so that the correct overload is called, since our list items are also collections.
				OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, (object)value, (object)oldItem, index));

				if (oldItem.Parent == this && !Contains(oldItem))
				{
					oldItem.Parent = null;
				}

				value.Parent = this;
			}
		}

		/// <summary>
		/// Adds an item to the collection.
		/// </summary>
		/// <param name="item">The item to add.</param>
		public override void Add(ObservableRecursiveCollection<T> item)
		{
			base.Add(item);

			OnPropertyChanged(nameof(Count));
			OnPropertyChanged(Binding.IndexerName);

			//Note: We must cast the item to type 'object' so that the correct overload is called, since our list items are also collections.
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, (object)item));

			item.Parent = this;
		}

		/// <summary>
		/// Removes all items from the collection.
		/// </summary>
		public override void Clear()
		{
			var oldItems = new ObservableRecursiveCollection<T>[Count];
			ListItems.CopyTo(oldItems, 0);

			base.Clear();

			OnPropertyChanged(nameof(Count));
			OnPropertyChanged(Binding.IndexerName);

			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

			foreach (var item in oldItems)
			{
				if (item.Parent == this)
				{
					item.Parent = null;
				}
			}
		}

		/// <summary>
		/// Inserts an item into the collection at the specified index.
		/// </summary>
		/// <param name="index">The index at which to insert the item.</param>
		/// <param name="item">The item to insert into the collection.</param>
		public override void Insert(int index, ObservableRecursiveCollection<T> item)
		{
			//Note: No error handling necessary.
			//      If the index is out of bounds, then let the list's insert method throw an exception.
			base.Insert(index, item);

			OnPropertyChanged(nameof(Count));
			OnPropertyChanged(Binding.IndexerName);

			//Note: We must cast the item to type 'object' so that the correct overload is called, since our list items are also collections.
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, (object)item, index));

			item.Parent = this;
		}

		/// <summary>
		/// Removes the first occurrence of a specific item from the collection.
		/// </summary>
		/// <param name="item">The item to remove from the collection.</param>
		/// <returns>
		/// Returns true if the item was successfully removed from the collection, otherwise false.
		/// This method also returns false if item was not found in the collection.
		/// </returns>
		public override bool Remove(ObservableRecursiveCollection<T> item)
		{
			bool removed = base.Remove(item);

			if (removed)
			{
				OnPropertyChanged(nameof(Count));
				OnPropertyChanged(Binding.IndexerName);

				//Note: We must cast the item to type 'object' so that the correct overload is called, since our list items are also collections.
				OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, (object)item));

				if (item.Parent == this && !Contains(item))
				{
					item.Parent = null;
				}
			}

			return removed;
		}

		/// <summary>
		/// Removes the item at the specified index.
		/// </summary>
		/// <param name="index">The index at which to remove the item.</param>
		public override void RemoveAt(int index)
		{
			//Note: No error handling necessary.
			//      If the index is out of bounds, then let the list's indexer throw an exception.
			var item = ListItems[index];

			base.RemoveAt(index);

			OnPropertyChanged(nameof(Count));
			OnPropertyChanged(Binding.IndexerName);

			//Note: We must cast the item to type 'object' so that the correct overload is called, since our list items are also collections.
			OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, (object)item, index));

			if (item.Parent == this && !Contains(item))
			{
				item.Parent = null;
			}
		}

		/// <summary>
		/// Returns a string that represents the current instance.
		/// </summary>
		/// <returns>Returns a string that represents the current instance.</returns>
		public override string ToString()
		{
			return string.Format("{0}, Count = {1}", Value?.ToString() ?? "null", Count);
		}
		#endregion
	}
}
