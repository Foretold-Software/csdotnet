// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

namespace System
{
	/// <summary>
	/// Provides methods to manage a disposable singleton object, using
	/// the <see cref="DisposableSingletonManager"/> class.
	/// </summary>
	/// <typeparam name="T">
	/// The type of the singleton class inheritting from <see cref="DisposableSingleton{T}"/>.
	/// </typeparam>
	/// <remarks>
	/// This class provides the typical functions of a class that inherits
	/// from <see cref="Disposable"/> and <see cref="IDisposableSingleton"/>.
	/// A class that inherits from <see cref="DisposableSingleton{T}"/> need
	/// only provide overrides for the <see cref="Disposable.FreeManagedResources"/>
	/// and <see cref="Disposable.FreeUnmanagedResources"/> methods, as needed.
	/// 
	/// Classes that inherit from this class should be instantiated in a "using" block
	/// so as to guarantee that the object's <see cref="IDisposable.Dispose"/> method
	/// is called when the object's variable goes out of scope.
	/// The object's variable going out of scope thus being dereferenced will not, by
	/// itself, result in the <see cref="IDisposable.Dispose"/> method being called,
	/// because the instance is still referenced in the
	/// <see cref="DisposableSingletonManager"/> class statically.
	/// </remarks>
	public abstract class DisposableSingleton<T> : Disposable, IDisposable, IDisposableSingleton where T : DisposableSingleton<T>
	{
		#region Methods
		/// <summary>
		/// Gets the singleton instance of the specified class type that is managed
		/// by <see cref="DisposableSingletonManager"/>. If no instance exists, then
		/// one is created using the type's parameterless contructor.
		/// This method also increments the singleton instance's reference count so that
		/// the instance's <see cref="IDisposable.Dispose"/> method is not called when there
		/// are multiple references to the instance in existence.
		/// </summary>
		/// <returns>Returns the singleton instance of the specified type.</returns>
		public static T GetInstance()
		{
			return DisposableSingletonManager.GetInstance<T>();
		}

		/// <summary>
		/// Frees the resources used by instances of this class.
		/// </summary>
		public new void Dispose()
		{
			DisposableSingletonManager.Dereference<T>();
		}

		/// <summary>
		/// Performs the functions normally done by
		/// the <see cref="IDisposable.Dispose"/> method. This method
		/// is called when the reference count for the <typeparamref name="T"/> type
		/// reaches zero in the <see cref="DisposableSingletonManager"/>
		/// class's disposable singleton management functions.
		/// </summary>
		public void DisposeWhenDereferenced()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion
	}
}
