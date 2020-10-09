// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

namespace System
{
	/// <summary>
	/// Provides utility methods to use with exceptions.
	/// </summary>
	public static class _Exception
	{
		/// <summary>
		/// Retrieves the inner exception of the given type, or null if none are found.
		/// </summary>
		/// <param name="exc">The current exception whose inner exceptions are searched.</param>
		/// <param name="exceptionType">The exception type to search for.</param>
		/// <returns>
		/// Returns an inner exception of the specified type, or null if none are found.
		/// </returns>
		public static Exception GetInnerExceptionExact(this Exception exc, Type exceptionType)
		{
			if (exceptionType == null)
			{
				return null;
			}
			else
			{
				Exception exception = exc;

				while (exception != null && exception.GetType() != exceptionType)
				{
					exception = exception.InnerException;
				}

				return exception;
			}
		}

		/// <summary>
		/// Retrieves the inner exception of the given type, or null if none are found.
		/// </summary>
		/// <typeparam name="T">The exception type to search for.</typeparam>
		/// <param name="exc">The current exception whose inner exceptions are searched.</param>
		/// <returns>
		/// Returns an inner exception of the specified type, or null if none are found.
		/// </returns>
		public static T GetInnerExceptionExact<T>(this Exception exc) where T : Exception
		{
			return GetInnerExceptionExact(exc, typeof(T)) as T;
		}

		/// <summary>
		/// Retrieves the inner exception of the given type or descendant type, or null if none are found.
		/// </summary>
		/// <param name="exc">The current exception whose inner exceptions are searched.</param>
		/// <param name="exceptionType">The exception type to search for.</param>
		/// <returns>
		/// Returns an inner exception of the specified type or descendant type, or null if none are found.
		/// </returns>
		public static Exception GetInnerException(this Exception exc, Type exceptionType)
		{
			if (exceptionType == null)
			{
				return null;
			}
			else
			{
				Exception exception = exc;

				while (exception != null && !exceptionType.IsAssignableFrom(exception.GetType()))
				{
					exception = exception.InnerException;
				}

				return exception;
			}
		}

		/// <summary>
		/// Retrieves the inner exception of the given type or descendant type, or null if none are found.
		/// </summary>
		/// <typeparam name="T">The exception type to search for.</typeparam>
		/// <param name="exc">The current exception whose inner exceptions are searched.</param>
		/// <returns>
		/// Returns an inner exception of the specified type or descendant type, or null if none are found.
		/// </returns>
		public static T GetInnerException<T>(this Exception exc) where T : Exception
		{
			return GetInnerException(exc, typeof(T)) as T;
		}
	}
}
