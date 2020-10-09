// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

namespace System.Windows.Threading
{
	public static class Extensions
	{
		/// <summary>
		/// Executes the specified <see cref="Action"/> synchronously on the
		/// thread the <see cref="Dispatcher"/> is associated with.
		/// </summary>
		/// <param name="dispatcher">The dispatcher to use for thread-safe invocation</param>
		/// <param name="action">The delegate to invoke through the dispatcher.</param>
		public static void CheckAndInvoke(this Dispatcher dispatcher, Action action)
		{
			if (dispatcher.CheckAccess())
			{
				action();
			}
			else
			{
				dispatcher.Invoke(action);
			}
		}

		/// <summary>
		/// Executes the specified <see cref="Action"/> asynchronously on the
		/// thread the <see cref="Dispatcher"/> is associated with.
		/// </summary>
		/// <param name="dispatcher">The dispatcher to use for thread-safe invocation</param>
		/// <param name="action">The delegate to invoke through the dispatcher.</param>
		public static void CheckAndBeginInvoke(this Dispatcher dispatcher, Action action)
		{
			if (dispatcher.CheckAccess())
			{
				action();
			}
			else
			{
				dispatcher.BeginInvoke(action);
			}
		}

		/// <summary>
		/// Executes the specified <see cref="Action"/> synchronously on the
		/// thread the <see cref="Dispatcher"/> is associated with.
		/// </summary>
		/// <param name="dispatcherObject">The dispatcher object, usually a UI control to use for thread-safe invocation</param>
		/// <param name="action">The delegate to invoke through the dispatcher.</param>
		public static void CheckAndInvoke(this DispatcherObject dispatcherObject, Action action)
		{
			if (dispatcherObject != null)
			{
				var dispatcher = dispatcherObject.Dispatcher;

				if (dispatcher.CheckAccess())
				{
					action();
				}
				else
				{
					dispatcher.Invoke(action);
				}
			}
		}

		/// <summary>
		/// Executes the specified <see cref="Action"/> asynchronously on the
		/// thread the <see cref="Dispatcher"/> is associated with.
		/// </summary>
		/// <param name="dispatcherObject">The dispatcher object, usually a UI control to use for thread-safe invocation</param>
		/// <param name="action">The delegate to invoke through the dispatcher.</param>
		public static void CheckAndBeginInvoke(this DispatcherObject dispatcherObject, Action action)
		{
			if (dispatcherObject != null)
			{
				var dispatcher = dispatcherObject.Dispatcher;

				if (dispatcher.CheckAccess())
				{
					action();
				}
				else
				{
					dispatcher.BeginInvoke(action);
				}
			}
		}
	}
}
