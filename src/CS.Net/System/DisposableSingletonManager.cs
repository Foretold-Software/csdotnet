// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.Collections.Generic;

namespace System
{
	/// <summary>
	/// Provides methods to create and manage singleton classes that implement
	/// the <see cref="IDisposableSingleton"/> interface. The instance management
	/// implements reference counting so that multiple references to a singleton
	/// instance do not conflict and cause the instance to be disposed before
	/// both callers have finished using the instance.
	/// </summary>
	public static class DisposableSingletonManager
	{

		#region Fields
		/// <summary>
		/// Gets a mutex to be used to manage the singleton instances.
		/// </summary>
		/// <remarks>
		/// Since this class deals with singleton objects, it is not unreasonable to
		/// assume that a singleton instance might be used in multiple threads.
		/// This mutex is necessary so that there cannot be two threads creating nor
		/// disposing a singleton object of a particular type, both at the same time.
		/// Were that to happen, we could end up with multiple "singleton" instances
		/// of a particular type, or the disposal logic in the second thread's call
		/// to <see cref="Disposable.Dispose(bool)"/> might begin before the first
		/// thread's call to <see cref="Disposable.Dispose(bool)"/> is able to set the
		/// <see cref="Disposable.disposed"/> field to true.
		/// In this case, the disposal logic would be run more than once, which has
		/// the potential to cause run-time bugs.
		/// </remarks>
		private static volatile object SingletonManagementMutex = new object();

		/// <summary>
		/// Backing field for the <see cref="SingletonInstances"/> property.
		/// </summary>
		private static Dictionary<Type, IDisposableSingleton> _SingletonInstances;

		/// <summary>
		/// Backing field for the <see cref="SingletonInstanceReferenceCount"/> property.
		/// </summary>
		private static Dictionary<Type, ulong> _SingletonInstanceReferenceCount;
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
		private static Dictionary<Type, IDisposableSingleton> SingletonInstances
		{ get { return _SingletonInstances ?? (_SingletonInstances = new Dictionary<Type, IDisposableSingleton>()); } }

		/// <summary>
		/// Gets the dictionary that maps each type to its singleton instance reference count.
		/// </summary>
		/// <remarks>
		/// The <see cref="Type"/> class overrides the <see cref="object.Equals(object)"/>
		/// and <see cref="object.GetHashCode"/> methods, which are used to determine
		/// instance equality by the default key comparer (<see cref="EqualityComparer{T}.Default"/>)
		/// for types not implementing <see cref="IEquatable{T}"/>.
		/// Therefore, we do not need to provide a comparer for a
		/// dictionary with the <see cref="Type"/> class as its key type.
		/// </remarks>
		private static Dictionary<Type, ulong> SingletonInstanceReferenceCount
		{ get { return _SingletonInstanceReferenceCount ?? (_SingletonInstanceReferenceCount = new Dictionary<Type, ulong>()); } }
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
		private static IDisposableSingleton CreateInstance(Func<IDisposableSingleton> createInstance, Type type)
		{
			try
			{
				if (createInstance != null)
				{
					return createInstance();
				}
				else
				{
					return Activator.CreateInstance(type) as IDisposableSingleton;
				}
			}
			catch (Exception exc)
			{
				throw new ArgumentException("Could not create a new instance of type " + type, exc);
			}
		}

		/// <summary>
		/// Gets the singleton instance of the specified class type that is managed
		/// by <see cref="DisposableSingletonManager"/>. If no instance exists, then
		/// one is created using the type's parameterless contructor.
		/// This method also increments the singleton instance's reference count so that
		/// the instance's <see cref="IDisposable.Dispose"/> method is not called when there
		/// are multiple references to the instance in existence.
		/// </summary>
		/// <typeparam name="T">The singleton instance's class type.</typeparam>
		/// <returns>Returns the singleton instance of the specified type.</returns>
		/// <remarks>
		/// This method is an alias for <see cref="GetInstance(Func{IDisposableSingleton}, Type)"/>.
		/// </remarks>
		public static T GetInstance<T>() where T : IDisposableSingleton
		{
			return (T)GetInstance(null, typeof(T));
		}

		/// <summary>
		/// Gets the singleton instance of the specified class type that is managed
		/// by <see cref="DisposableSingletonManager"/>. If no instance exists, then
		/// one is created using the provided function or the type's parameterless contructor.
		/// This method also increments the singleton instance's reference count so that
		/// the instance's <see cref="IDisposable.Dispose"/> method is not called when there
		/// are multiple references to the instance in existence.
		/// </summary>
		/// <typeparam name="T">The singleton instance's class type.</typeparam>
		/// <param name="createInstance">
		/// A function to create the singleton instance, or null to use its parameterless contructor.
		/// </param>
		/// <returns>Returns the singleton instance of the specified type.</returns>
		/// <remarks>
		/// This method is an alias for <see cref="GetInstance(Func{IDisposableSingleton}, Type)"/>.
		/// </remarks>
		public static T GetInstance<T>(Func<T> createInstance) where T : IDisposableSingleton
		{
			if (createInstance == null)
			{
				return (T)GetInstance(null, typeof(T));
			}
			else
			{
				return (T)GetInstance(() => createInstance(), typeof(T));
			}
		}

		/// <summary>
		/// Gets the singleton instance of the specified class type that is managed
		/// by <see cref="DisposableSingletonManager"/>. If no instance exists, then
		/// one is created using the type's parameterless contructor.
		/// This method also increments the singleton instance's reference count so that
		/// the instance's <see cref="IDisposable.Dispose"/> method is not called when there
		/// are multiple references to the instance in existence.
		/// </summary>
		/// <param name="type">The singleton instance's class type.</param>
		/// <returns>Returns the singleton instance of the specified type.</returns>
		public static IDisposableSingleton GetInstance(Type type)
		{
			return GetInstance(null, type);
		}

		/// <summary>
		/// Gets the singleton instance of the specified class type that is managed
		/// by <see cref="DisposableSingletonManager"/>. If no instance exists, then
		/// one is created using the provided function or the type's parameterless contructor.
		/// This method also increments the singleton instance's reference count so that
		/// the instance's <see cref="IDisposable.Dispose"/> method is not called when there
		/// are multiple references to the instance in existence.
		/// </summary>
		/// <param name="createInstance">
		/// A function to create the singleton instance, or null to use its parameterless contructor.
		/// </param>
		/// <param name="type">The singleton instance's class type.</param>
		/// <returns>Returns the singleton instance of the specified type.</returns>
		public static IDisposableSingleton GetInstance(Func<IDisposableSingleton> createInstance, Type type)
		{
			//Since the singleton paradigm allows for only one instance of a
			// particular type, we will use the type as a key to identify which
			// singleton instance to return.
			Type key = type;


			lock (SingletonManagementMutex)
			{
				//Add the reference counter key if it does not exist.
				if (!SingletonInstanceReferenceCount.ContainsKey(key))
				{
					SingletonInstanceReferenceCount.Add(key, 0);
				}

				//Add the singleton instance key if it does not exist.
				if (!SingletonInstances.ContainsKey(key))
				{
					SingletonInstances.Add(key, null);
				}

				//If the reference count is 0, create a new singleton instance.
				if (SingletonInstanceReferenceCount[key] == 0)
				{
					SingletonInstances[key] = CreateInstance(createInstance, type);
				}


				//Increment the reference count.
				SingletonInstanceReferenceCount[key]++;

				//Return the singleton instance.
				return SingletonInstances[key] as IDisposableSingleton;
			}
		}

		/// <summary>
		/// Decrements the reference count for the singleton instance of the given type,
		/// and calls its <see cref="IDisposable.Dispose"/> method if the reference
		/// count gets to zero.
		/// </summary>
		/// <typeparam name="T">The singleton instance's class type.</typeparam>
		/// <returns>
		/// Returns true if the singleton instance's reference count drops to zero and
		/// its <see cref="IDisposable.Dispose"/> method is called, otherwise returns false.
		/// </returns>
		/// <remarks>
		/// This method is an alias for <see cref="Dereference(Type)"/>.
		/// </remarks>
		public static bool Dereference<T>() where T : IDisposableSingleton
		{
			return Dereference(typeof(T));
		}

		/// <summary>
		/// Decrements the reference count for the singleton instance of the given type,
		/// and calls its <see cref="IDisposable.Dispose"/> method if the reference
		/// count gets to zero.
		/// </summary>
		/// <param name="type">The singleton instance's class type.</param>
		/// <returns>
		/// Returns true if the singleton instance's reference count drops to zero and
		/// its <see cref="IDisposable.Dispose"/> method is called, otherwise returns false.
		/// </returns>
		public static bool Dereference(Type type)
		{
			//Since the singleton paradigm allows for only one instance of a
			// particular type, we will use the type as a key to identify which
			// singleton instance to return.
			Type key = type;
			IDisposableSingleton instance;


			lock (SingletonManagementMutex)
			{
				//Add the reference counter key if it does not exist.
				if (!SingletonInstanceReferenceCount.ContainsKey(key))
				{
					SingletonInstanceReferenceCount.Add(key, 1);
				}

				//Add the singleton instance key if it does not exist.
				if (!SingletonInstances.ContainsKey(key))
				{
					SingletonInstances.Add(key, null);
				}

				//If the reference count is 0, dispose the singleton instance.
				if (0 == (--SingletonInstanceReferenceCount[key]))
				{
					if (null != (instance = SingletonInstances[key]))
					{
						//Remove the reference from the singleton instance dictionary before disposing.
						SingletonInstances[key] = null;

						instance.DisposeWhenDereferenced();

						return true;
					}
				}

				return false;
			}
		}
		#endregion
	}
}
