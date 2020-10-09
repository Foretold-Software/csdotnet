// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.Collections.Generic;

namespace System
{
	/// <summary>
	/// Provides methods to create and manage singleton classes.
	/// </summary>
	public static class SingletonManager
	{
		#region Fields
		/// <summary>
		/// Gets a mutex to be used to manage the singleton instances.
		/// </summary>
		/// <remarks>
		/// Since this class deals with singleton objects, it is not unreasonable to
		/// assume that a singleton instance might be used in multiple threads.
		/// This mutex is necessary so that there cannot be two threads creating
		/// a singleton object of a particular type, both at the same time.
		/// Were that to happen, we could end up with multiple "singleton" instances
		/// of a particular type.
		/// </remarks>
		private static volatile object SingletonManagementMutex = new object();

		/// <summary>
		/// Backing field for the <see cref="SingletonInstances"/> property.
		/// </summary>
		private static Dictionary<Type, object> _SingletonInstances;
		#endregion

		#region Properties
		/// <summary>
		/// Gets the dictionary that maps each type to its singleton instance.
		/// </summary>
		/// <remarks>
		/// The <see cref="Type"/> class overrides the <see cref="object.Equals(object)"/>
		/// and <see cref="object.GetHashCode"/> methods, which are used to determine
		/// instance equality by the default key comparer (<see cref="EqualityComparer{T}.Default"/>)
		/// for types not implementing <see cref="IEquatable{T}"/>.
		/// Therefore, we do not need to provide a comparer for a
		/// dictionary with the <see cref="Type"/> class as its key type.
		/// </remarks>
		private static Dictionary<Type, object> SingletonInstances
		{ get { return _SingletonInstances ?? (_SingletonInstances = new Dictionary<Type, object>()); } }
		#endregion

		#region Methods
		/// <summary>
		/// Creates the singleton instance of the specified type using the function
		/// provided, or the type's parameterless constructor if the function is null.
		/// </summary>
		/// <param name="createInstance">
		/// A function to create the singleton instance, or null to use its parameterless contructor.
		/// </param>
		/// <param name="type">The singleton instance's class type.</param>
		/// <returns>Returns the newly created instance of the specified type.</returns>
		private static object CreateInstance(Func<object> createInstance, Type type)
		{
			try
			{
				if (createInstance != null)
				{
					return createInstance();
				}
				else
				{
					return Activator.CreateInstance(type);
				}
			}
			catch (Exception exc)
			{
				throw new ArgumentException("Could not create a new instance of type " + type, exc);
			}
		}

		/// <summary>
		/// Gets the singleton instance of the specified class type that is managed
		/// by <see cref="SingletonManager"/>. If no instance exists, then one is
		/// created using the type's parameterless contructor.
		/// </summary>
		/// <typeparam name="T">The singleton instance's class type.</typeparam>
		/// <returns>Returns the singleton instance of the specified type.</returns>
		public static T GetInstance<T>()
		{
			return GetInstance<T>(null);
		}

		/// <summary>
		/// Gets the singleton instance of the specified class type that is managed
		/// by <see cref="SingletonManager"/>. If no instance exists, then one is
		/// created using the provided function or the type's parameterless contructor.
		/// </summary>
		/// <typeparam name="T">The singleton instance's class type.</typeparam>
		/// <param name="createInstance">
		/// A function to create the singleton instance, or null to use its parameterless contructor.
		/// </param>
		/// <returns>Returns the singleton instance of the specified type.</returns>
		public static T GetInstance<T>(Func<T> createInstance)
		{
			if (createInstance == null)
			{
				return (T)GetInstance(null, typeof(T));
			}
			else
			{
				return (T)GetInstance(() => createInstance, typeof(T));
			}
		}


		/// <summary>
		/// Gets the singleton instance of the specified class type that is managed
		/// by <see cref="SingletonManager"/>. If no instance exists, then one is
		/// created using the type's parameterless contructor.
		/// </summary>
		/// <param name="type">The singleton instance's class type.</param>
		/// <returns>Returns the singleton instance of the specified type.</returns>
		public static object GetInstance(Type type)
		{
			return GetInstance(null, type);
		}

		/// <summary>
		/// Gets the singleton instance of the specified class type that is managed
		/// by <see cref="SingletonManager"/>. If no instance exists, then one is
		/// created using the provided function or the type's parameterless contructor.
		/// </summary>
		/// <param name="createInstance">
		/// A function to create the singleton instance, or null to use its parameterless contructor.
		/// </param>
		/// <param name="type">The singleton instance's class type.</param>
		/// <returns>Returns the singleton instance of the specified type.</returns>
		public static object GetInstance(Func<object> createInstance, Type type)
		{
			//Since the singleton paradigm allows for only one instance of a
			// particular type, we will use the type as a key to identify which
			// singleton instance to return.
			Type key = type;


			lock (SingletonManagementMutex)
			{
				//Add the singleton instance key if it does not exist.
				if (!SingletonInstances.ContainsKey(key))
				{
					SingletonInstances.Add(key, null);
				}

				//If the singleton instance does not exist, create one.
				if (SingletonInstances[key] == null)
				{
					SingletonInstances[key] = CreateInstance(createInstance, type);
				}

				//Return the singleton instance.
				return SingletonInstances[key];
			}
		}
		#endregion
	}
}
