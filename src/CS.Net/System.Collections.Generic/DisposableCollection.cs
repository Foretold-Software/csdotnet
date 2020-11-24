// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.Collections.ObjectModel;

namespace System.Collections.Generic
{
	/// <summary>
	/// Provides a convenient method for quickly disposing of a collection
	/// of objects that implement the <see cref="IDisposable"/> interface.
	/// </summary>
	/// <typeparam name="T">The type of the objects in the collection.</typeparam>
	/// <remarks>
	/// Dispose Pattern reference:
	/// https://msdn.microsoft.com/en-us/library/b1yfkh5e.aspx
	/// </remarks>
	public class DisposableCollection<T> : ListContainer<T>, IDisposable where T : IDisposable
	{
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
}
