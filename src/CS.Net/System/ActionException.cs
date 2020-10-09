// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.Runtime.Serialization;

namespace System
{
	/// <summary>
	/// Represents an exception that holds an action that is
	/// intended to be performed when the exception is caught.
	/// </summary>
	[Serializable]
	public class ActionException : Exception
	{
		#region Constructors
		/// <summary>
		/// Creates a new instance of <see cref="ActionException"/>.
		/// </summary>
		/// <param name="action">The action to perform when the exception is caught.</param>
		public ActionException(Action action)
		{
			this.Action = action;
		}

		/// <summary>
		/// Creates a new instance of <see cref="ActionException"/>.
		/// </summary>
		/// <param name="action">The action to perform when the exception is caught.</param>
		/// <param name="message">The message that describes the reason for the exception.</param>
		public ActionException(Action action, string message) : base(message)
		{
			this.Action = action;
		}

		/// <summary>
		/// Creates a new instance of <see cref="ActionException"/>.
		/// </summary>
		/// <param name="action">The action to perform when the exception is caught.</param>
		/// <param name="message">The message that describes the reason for the exception.</param>
		/// <param name="inner">The exception that is the cause of the current exception, or null if no inner exception is specified.</param>
		public ActionException(Action action, string message, Exception inner) : base(message, inner)
		{
			this.Action = action;
		}

		/// <summary>
		/// Creates a new instance of <see cref="ActionException"/>.
		/// </summary>
		/// <param name="action">The action to perform when the exception is caught.</param>
		/// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
		/// <exception cref="ArgumentNullException">The info parameter is null.</exception>
		/// <exception cref="SerializationException">The class name is null or <see cref="Exception.HResult"/> is zero (0).</exception>
		protected ActionException(Action action, SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this.Action = action;
		}
		#endregion

		/// <summary>
		/// Gets the action that is to be performed when this <see cref="ActionException"/> is caught.
		/// </summary>
		public Action Action { get; private set; }

		/// <summary>
		/// Executes the action safely, catching any exceptions thrown by the action.
		/// </summary>
		public void ExecuteActionSafe()
		{
			var action = this.Action;

			if (action != null)
			{
				try
				{
					action();
				}
				catch { }
			}
		}
	}
}
