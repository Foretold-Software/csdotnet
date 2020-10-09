// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

namespace System
{
	/// <summary>
	/// Defines methods to release allocated resources for
	/// singleton classes using <see cref="DisposableSingletonManager"/>.
	/// </summary>
	public interface IDisposableSingleton : IDisposable
	{
		/// <summary>
		/// When overridden, performs the functions normally done by
		/// the <see cref="IDisposable.Dispose"/> method. This method
		/// is called when the reference count for a given singleton
		/// class type reaches zero in the <see cref="DisposableSingletonManager"/>
		/// class's disposable singleton management functions.
		/// </summary>
		void DisposeWhenDereferenced();
	}
}
