// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

namespace System
{
	/// <summary>
	/// Provides methods to manage a disposable singleton object, using
	/// the <see cref="SingletonManager"/> class.
	/// </summary>
	/// <typeparam name="T">
	/// The type of the singleton class inheritting from <see cref="Singleton{T}"/>.
	/// </typeparam>
	public abstract class Singleton<T> where T : Singleton<T>
	{
		#region Methods
		/// <summary>
		/// Gets the singleton instance of the specified class type that is managed
		/// by <see cref="SingletonManager"/>. If no instance exists, then
		/// one is created using the type's parameterless contructor.
		/// </summary>
		/// <returns>Returns the singleton instance of the specified type.</returns>
		public static T GetInstance()
		{
			return SingletonManager.GetInstance<T>();
		}
		#endregion
	}
}
