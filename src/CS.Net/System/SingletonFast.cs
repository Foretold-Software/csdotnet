// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

namespace System
{
	/// <summary>
	/// Provides methods to manage a performance-dependent singleton class.
	/// </summary>
	/// <typeparam name="T">
	/// The type of the singleton class inheritting from <see cref="SingletonFast{T}"/>.
	/// </typeparam>
	public abstract class SingletonFast<T> where T : SingletonFast<T>, new()
	{
		/// <summary>
		/// Gets the singleton instance of the specified class type.
		/// </summary>
		/// <remarks>
		/// Declaring the instance in this manner is the fastest way to do so,
		/// according to the benchmark tests cited at the following reference link:
		/// https://www.dotnetperls.com/singleton
		/// </remarks>
		private static readonly T Instance = SingletonManager.GetInstance(() => new T());
	}
}
