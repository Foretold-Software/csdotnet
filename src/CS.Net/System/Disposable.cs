// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

namespace System
{
	/// <summary>
	/// Defines methods to free allocated resources. This class provides all
	/// the required methods for a class that implements the Dispose Pattern.
	/// (see remarks for reference link)
	/// </summary>
	/// <remarks>
	/// Dispose Pattern reference:
	/// https://msdn.microsoft.com/en-us/library/b1yfkh5e.aspx
	/// </remarks>
	public abstract class Disposable : IDisposable
	{
		#region Fields
		/// <summary>
		/// Used to detect redundant calls.
		/// </summary>
		private bool disposed = false;
		#endregion

		#region Methods
		/// <summary>
		/// When overridden, used to free any managed resources during disposal.
		/// </summary>
		protected virtual void FreeManagedResources() { }

		/// <summary>
		/// When overridden, used to free any unmanaged resources during disposal.
		/// </summary>
		protected virtual void FreeUnmanagedResources() { }
		#endregion

		#region IDisposable members
		/// <summary>
		/// Frees the resources used by instances of this class.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Frees the resources used by instances of this class.
		/// Types inheriting from this class should dispose of their resources
		/// by overriding the <see cref="FreeManagedResources"/> method
		/// and the <see cref= "FreeUnmanagedResources" /> method, rather
		/// than by overriding this method.
		/// </summary>
		/// <param name="disposing">
		/// Indicates whether this method is being called from the <see cref="Dispose()"/> method.
		/// </param>
		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				//If calling the Dispose method, then free managed resources.
				if (disposing)
				{
					FreeManagedResources();
				}

				//Free any unmanaged resources.
				FreeUnmanagedResources();

				disposed = true;
			}
		}

		/// <summary>
		/// Finalizes the object.
		/// </summary>
		~Disposable()
		{
			Dispose(false);
		}
		#endregion
	}
}
