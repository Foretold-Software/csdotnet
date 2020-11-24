// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.Collections.Generic;

namespace System.Collections.ObjectModel
{
	/// <summary>
	/// Provides a convenient method for quickly disposing of a collection
	/// of objects that implement the <see cref="IDisposable"/> interface.
	/// </summary>
	/// <typeparam name="T">The type of the items in the collection.</typeparam>
	/// <remarks>
	/// Dispose Pattern reference:
	/// https://msdn.microsoft.com/en-us/library/b1yfkh5e.aspx
	/// </remarks>
	public class DisposableCollection<T> : ListContainer<T>, IDisposable where T : IDisposable
	{
		#region Constructors
		/// <summary>
		/// Creates a new instance of <see cref="DisposableCollection{T}"/>.
		/// </summary>
		public DisposableCollection()
		{
			ListItems = new List<T>();
		}

		/// <summary>
		/// Creates a new instance of <see cref="DisposableCollection{T}"/> with the given capacity.
		/// </summary>
		/// <param name="capacity">The number of elements that the new collection can initially store.</param>
		public DisposableCollection(int capacity)
		{
			ListItems = new List<T>(capacity);
		}

		/// <summary>
		/// Creates a new instance of <see cref="DisposableCollection{T}"/> with the given values.
		/// </summary>
		/// <param name="collection">The collection of items to use.</param>
		public DisposableCollection(IEnumerable<T> collection)
		{
			ListItems = new List<T>(collection);
		}

		/// <summary>
		/// Creates a new instance of <see cref="DisposableCollection{T}"/> with the given values.
		/// </summary>
		/// <param name="collection">The collection of items to use.</param>
		public DisposableCollection(IList<T> collection)
		{
			ListItems = collection;
		}
		#endregion

		#region Fields
		/// <summary>
		/// Used to detect redundant calls.
		/// </summary>
		private bool disposed = false;
		#endregion

		#region IDisposable members
		/// <summary>
		/// Frees the resources used by items in this collection.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Frees the resources used by items in this collection.
		/// </summary>
		/// <param name="disposing">
		/// Indicates whether this method is being called from the <see cref="Dispose()"/> method.
		/// </param>
		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					foreach (var item in this)
					{
						item.Dispose();
					}
				}
				else
				{
					Clear();
				}

				disposed = true;
			}
		}

		/// <summary>
		/// Finalizes the object and each of its items if
		/// their references have not been saved elsewhere.
		/// </summary>
		~DisposableCollection()
		{
			Dispose(false);
		}
		#endregion
	}

	/// <summary>
	/// A class providing extension methods for the <see cref="DisposableCollection{T}"/> class.
	/// </summary>
	public static class DisposableCollection
	{
		/// <summary>
		/// Returns a <see cref="DisposableCollection"/> containing the items in the given collection.
		/// </summary>
		/// <typeparam name="T">The type of the items in the collection.</typeparam>
		/// <param name="collection">The collection of items to use.</param>
		/// <returns>Returns a <see cref="DisposableCollection"/> containing the items in the given collection.</returns>
		public static DisposableCollection<T> AsDisposableCollection<T>(this IList<T> collection) where T : IDisposable
		{
			return new DisposableCollection<T>(collection);
		}

		/// <summary>
		/// Returns a <see cref="DisposableCollection"/> containing the items in the given collection.
		/// </summary>
		/// <typeparam name="T">The type of the items in the collection.</typeparam>
		/// <param name="collection">The collection of items to use.</param>
		/// <returns>Returns a <see cref="DisposableCollection"/> containing the items in the given collection.</returns>
		public static DisposableCollection<T> AsDisposableCollection<T>(this IEnumerable<T> collection) where T : IDisposable
		{
			return new DisposableCollection<T>(collection);
		}
	}
}
