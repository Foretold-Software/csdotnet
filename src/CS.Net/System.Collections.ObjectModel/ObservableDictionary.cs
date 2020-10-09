// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;

namespace System.Collections.ObjectModel
{
	/// <summary>
	/// Represents a dynamic collection of keys and values that provides notifications
	/// when items get added, removed, or when the whole collection is refreshed.
	/// </summary>
	/// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
	/// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
	[DebuggerDisplay("Count = {Count}")]
	public class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, INotifyCollectionChanged, INotifyPropertyChanged
	{
		#region Constructors
		/// <summary>
		/// Creates a new instance of <see cref="ObservableDictionary{TKey, TValue}"/>.
		/// </summary>
		public ObservableDictionary()
		{
			this.Dictionary = new Dictionary<TKey, TValue>();
		}

		/// <summary>
		/// Creates a new instance of <see cref="ObservableDictionary{TKey, TValue}"/>.
		/// </summary>
		/// <param name="dictionary">
		/// The <see cref="IDictionary{TKey, TValue}"/> whose elements are
		/// copied to the new <see cref="ObservableDictionary{TKey, TValue}"/>.
		/// </param>
		public ObservableDictionary(IDictionary<TKey, TValue> dictionary)
		{
			this.Dictionary = new Dictionary<TKey, TValue>(dictionary);
		}

		/// <summary>
		/// Creates a new instance of <see cref="ObservableDictionary{TKey, TValue}"/>.
		/// </summary>
		/// <param name="comparer">
		/// The <see cref="IEqualityComparer{T}"/> implementation to use when comparing keys,
		/// or null to use the default <see cref="EqualityComparer{T}"/> for the type of the key.
		/// </param>
		public ObservableDictionary(IEqualityComparer<TKey> comparer)
		{
			this.Dictionary = new Dictionary<TKey, TValue>(comparer);
		}

		/// <summary>
		/// Creates a new instance of <see cref="ObservableDictionary{TKey, TValue}"/>.
		/// </summary>
		/// <param name="capacity">
		/// The initial number of elements that the <see cref="ObservableDictionary{TKey, TValue}"/> can contain.
		/// </param>
		public ObservableDictionary(int capacity)
		{
			this.Dictionary = new Dictionary<TKey, TValue>(capacity);
		}

		/// <summary>
		/// Creates a new instance of <see cref="ObservableDictionary{TKey, TValue}"/>.
		/// </summary>
		/// <param name="dictionary">
		/// The <see cref="IDictionary{TKey, TValue}"/> whose elements are
		/// copied to the new <see cref="ObservableDictionary{TKey, TValue}"/>.
		/// </param>
		/// <param name="comparer">
		/// The <see cref="IEqualityComparer{T}"/> implementation to use when comparing keys,
		/// or null to use the default <see cref="EqualityComparer{T}"/> for the type of the key.
		/// </param>
		public ObservableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
		{
			this.Dictionary = new Dictionary<TKey, TValue>(dictionary, comparer);
		}

		/// <summary>
		/// Creates a new instance of <see cref="ObservableDictionary{TKey, TValue}"/>.
		/// </summary>
		/// <param name="capacity">
		/// The initial number of elements that the <see cref="ObservableDictionary{TKey, TValue}"/> can contain.
		/// </param>
		/// <param name="comparer">
		/// The <see cref="IEqualityComparer{T}"/> implementation to use when comparing keys,
		/// or null to use the default <see cref="EqualityComparer{T}"/> for the type of the key.
		/// </param>
		public ObservableDictionary(int capacity, IEqualityComparer<TKey> comparer)
		{
			this.Dictionary = new Dictionary<TKey, TValue>(capacity, comparer);
		}
		#endregion

		#region Fields
		/// <summary>
		/// An instance of <see cref="Dictionary{TKey, TValue}"/>
		/// to use as a backing store for an instance of this class.
		/// </summary>
		private IDictionary<TKey, TValue> Dictionary;

		/// <summary>
		/// The names of the properties that must be refreshed
		/// any time there's a change to the collection.
		/// </summary>
		private string[] PropertyNames = new[] { "Count", "Item[]", "Keys", "Values" };
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

		#region Properties / Indexer
		/// <summary>
		/// Gets the number of key/value pairs contained in the <see cref="ObservableDictionary{TKey, TValue}"/>.
		/// </summary>
		public int Count
		{ get { return this.Dictionary.Count; } }

		/// <summary>
		/// Gets a value indicating whether the <see cref="ObservableDictionary{TKey, TValue}"/> is read-only.
		/// </summary>
		public bool IsReadOnly
		{ get { return this.Dictionary.IsReadOnly; } }

		/// <summary>
		/// Gets a collection containing the keys in the <see cref="ObservableDictionary{TKey, TValue}"/>.
		/// </summary>
		public ICollection<TKey> Keys
		{ get { return this.Dictionary.Keys; } }

		/// <summary>
		/// Gets a collection containing the values in the <see cref="ObservableDictionary{TKey, TValue}"/>.
		/// </summary>
		public ICollection<TValue> Values
		{ get { return this.Dictionary.Values; } }

		/// <summary>
		/// Gets or sets the value associated with the specified key.
		/// </summary>
		/// <param name="key">The key of the value to get or set.</param>
		/// <returns>
		/// The value associated with the specified key.
		/// If the specified key is not found, a get operation throws a <see cref="KeyNotFoundException"/>,
		/// and a set operation creates a new element with the specified key.
		/// </returns>
		public TValue this[TKey key]
		{
			get { return this.Dictionary[key]; }
			set
			{
				TValue oldValue;
				KeyValuePair<TKey, TValue> oldItem;
				KeyValuePair<TKey, TValue> newItem = new KeyValuePair<TKey, TValue>(key, value);

				if (this.Dictionary.TryGetValue(key, out oldValue))
				{
					if (!object.Equals(oldValue, value))
					{
						oldItem = new KeyValuePair<TKey, TValue>(key, oldValue);

						this.Dictionary[key] = value;
						this.OnCollectionChanged(NotifyCollectionChangedAction.Replace, newItem, oldItem);
					}
				}
				else
				{
					this.Dictionary[key] = value;
					this.OnCollectionChanged(NotifyCollectionChangedAction.Add, newItem);
				}
			}
		}
		#endregion

		#region Methods - IDictionary Members
		/// <summary>
		/// Adds the specified key/value pair to the dictionary.
		/// </summary>
		/// <param name="item">The key/value pair to add.</param>
		public void Add(KeyValuePair<TKey, TValue> item)
		{
			this.Dictionary.Add(item);
			this.OnCollectionChanged(NotifyCollectionChangedAction.Add, item);
		}

		/// <summary>
		/// Adds the specified key and value to the dictionary.
		/// </summary>
		/// <param name="key">The key of the element to add.</param>
		/// <param name="value">The value of the element to add. The value can be null for reference types.</param>
		public void Add(TKey key, TValue value)
		{
			var item = new KeyValuePair<TKey, TValue>(key, value);

			this.Dictionary.Add(item);
			this.OnCollectionChanged(NotifyCollectionChangedAction.Add, item);
		}

		/// <summary>
		/// Removes all keys and values from the <see cref="ObservableDictionary{TKey, TValue}"/>.
		/// </summary>
		public void Clear()
		{
			if (this.Dictionary.Count > 0)
			{
				this.Dictionary.Clear();
				this.OnCollectionChanged();
			}
		}

		/// <summary>
		/// Determines whether the <see cref="ObservableDictionary{TKey, TValue}"/> contains a specific value.
		/// </summary>
		/// <param name="item">The key/value pair to find.</param>
		/// <returns>
		/// Returns true if the key/value pair is found, otherwise false.
		/// </returns>
		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			return this.Dictionary.Contains(item);
		}

		/// <summary>
		/// Determines whether the <see cref="ObservableDictionary{TKey, TValue}"/> contains the specified key.
		/// </summary>
		/// <param name="key">
		/// The key to locate in the <see cref="ObservableDictionary{TKey, TValue}"/>.
		/// </param>
		/// <returns>
		/// Returns true if the <see cref="ObservableDictionary{TKey, TValue}"/>
		/// contains an element with the specified key, otherwise false.
		/// </returns>
		public bool ContainsKey(TKey key)
		{
			return this.Dictionary.ContainsKey(key);
		}

		/// <summary>
		/// Copies the elements of the <see cref="ObservableDictionary{TKey, TValue}"/>
		/// to an <see cref="Array"/>, starting at a particular index.
		/// </summary>
		/// <param name="array">
		/// The one-dimensional array that is the destination of the elements copied
		/// from <see cref="ObservableDictionary{TKey, TValue}"/>. The array must have
		/// zero-based indexing.
		/// </param>
		/// <param name="arrayIndex">
		/// The zero-based index in array at which copying begins.
		/// </param>
		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			this.Dictionary.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Returns an enumerator that iterates through
		/// the <see cref="ObservableDictionary{TKey, TValue}"/>.
		/// </summary>
		/// <returns>
		/// Returns a <see cref="Dictionary{TKey, TValue}.Enumerator"/> structure
		/// for the <see cref="ObservableDictionary{TKey, TValue}"/>.
		/// </returns>
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return this.Dictionary.GetEnumerator();
		}

		/// <summary>
		/// Removes the first occurrence of a specific object from the
		/// </summary>
		/// <param name="item">The key/value pair to remove.</param>
		/// <returns>
		/// Returns true if the key/value pair was successfully found and removed, otherwise false.
		/// This method also returns false if the key/value pair was not found.
		/// </returns>
		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			if (this.Dictionary.Remove(item))
			{
				this.OnCollectionChanged(NotifyCollectionChangedAction.Remove, item);
				return true;
			}
			else return false;
		}

		/// <summary>
		/// Removes the value with the specified key from the <see cref="ObservableDictionary{TKey, TValue}"/>.
		/// </summary>
		/// <param name="key">The key of the element to remove.</param>
		/// <returns>
		/// Returns true if the key/value pair was successfully found and removed, otherwise false.
		/// This method also returns false if the key/value pair was not found.
		/// </returns>
		public bool Remove(TKey key)
		{
			TValue removedValue;

			//First get the value so the OnCollectionChanged event args can be
			// populated. If key is null, then this method call will throw an exception.
			if (this.Dictionary.TryGetValue(key, out removedValue) && this.Dictionary.Remove(key))
			{
				this.OnCollectionChanged(NotifyCollectionChangedAction.Remove, new KeyValuePair<TKey, TValue>(key, removedValue));
				return true;
			}
			else return false;
		}

		/// <summary>
		/// Gets the value associated with the specified key.
		/// </summary>
		/// <param name="key">The key of the value to get.</param>
		/// <param name="value">
		/// When this method returns, contains the value associated with the specified key, if the key is found.
		/// Otherwise, the default value for the type of the value parameter.
		/// This parameter is passed uninitialized.
		/// </param>
		/// <returns>
		/// Returns true if the <see cref="ObservableDictionary{TKey, TValue}"/>
		/// contains an element with the specified key, otherwise false.
		/// </returns>
		public bool TryGetValue(TKey key, out TValue value)
		{
			return this.Dictionary.TryGetValue(key, out value);
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="IEnumerator"/> object that can be used to iterate through the collection.
		/// </returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)this.Dictionary).GetEnumerator();
		}
		#endregion

		#region Methods - OnChanged
		/// <summary>
		/// Calls the <see cref="PropertyChanged"/> event for
		/// each of the properties in <see cref="PropertyNames"/>.
		/// </summary>
		private void OnPropertyChanged()
		{
			foreach (var name in PropertyNames)
			{
				OnPropertyChanged(name);
			}
		}

		/// <summary>
		/// Calls the <see cref="PropertyChanged"/> event.
		/// </summary>
		/// <param name="propertyName">The name of the property.</param>
		protected virtual void OnPropertyChanged(string propertyName)
		{
			var handler = this.PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		/// <summary>
		/// Calls the <see cref="CollectionChanged"/> and <see cref="PropertyChanged"/> events.
		/// </summary>
		protected virtual void OnCollectionChanged()
		{
			OnPropertyChanged();

			var handler = this.CollectionChanged;
			if (handler != null)
			{
				handler(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			}
		}

		/// <summary>
		/// Calls the <see cref="CollectionChanged"/> and <see cref="PropertyChanged"/> events.
		/// </summary>
		/// <param name="action">The action that caused the event.</param>
		/// <param name="changedItem">The item involved in the change.</param>
		protected virtual void OnCollectionChanged(NotifyCollectionChangedAction action, KeyValuePair<TKey, TValue> changedItem)
		{
			OnPropertyChanged();

			var handler = this.CollectionChanged;
			if (handler != null)
			{
				handler(this, new NotifyCollectionChangedEventArgs(action, changedItem));
			}
		}

		/// <summary>
		/// Calls the <see cref="CollectionChanged"/> and <see cref="PropertyChanged"/> events.
		/// </summary>
		/// <param name="action">The action that caused the event.</param>
		/// <param name="newItem">The new item involved in the change.</param>
		/// <param name="oldItem">The old item involved in the change.</param>
		protected virtual void OnCollectionChanged(NotifyCollectionChangedAction action, KeyValuePair<TKey, TValue> newItem, KeyValuePair<TKey, TValue> oldItem)
		{
			OnPropertyChanged();

			var handler = this.CollectionChanged;
			if (handler != null)
			{
				handler(this, new NotifyCollectionChangedEventArgs(action, newItem, oldItem));
			}
		}

		/// <summary>
		/// Calls the <see cref="CollectionChanged"/> and <see cref="PropertyChanged"/> events.
		/// </summary>
		/// <param name="action">The action that caused the event.</param>
		/// <param name="newItems">The list of new items involved in the change.</param>
		protected virtual void OnCollectionChanged(NotifyCollectionChangedAction action, IList newItems)
		{
			OnPropertyChanged();

			var handler = this.CollectionChanged;
			if (handler != null)
			{
				handler(this, new NotifyCollectionChangedEventArgs(action, newItems));
			}
		}
		#endregion
	}
}
