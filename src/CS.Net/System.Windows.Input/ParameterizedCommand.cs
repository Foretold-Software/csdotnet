// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

namespace System.Windows.Input
{
	/// <summary>
	/// A class for implementing the ICommand interface for a command requiring a parameter.
	/// </summary>
	public class ParameterizedCommand : CommandBase, ICommand
	{
		#region Constructors
		/// <summary>
		/// Creates a new instance of <see cref="ParameterizedCommand"/>.
		/// </summary>
		public ParameterizedCommand()
		{
			Action = new ParameterizedCommandAction();
		}

		/// <summary>
		/// Creates a new instance of <see cref="ParameterizedCommand"/>.
		/// </summary>
		/// <param name="action">The action to be invoked.</param>
		public ParameterizedCommand(Action<object> action)
		{
			Action = new ParameterizedCommandAction(action);
		}

		/// <summary>
		/// Creates a new instance of <see cref="ParameterizedCommand"/>.
		/// </summary>
		/// <param name="action">The action to be invoked.</param>
		/// <param name="condition">The condition upon which the action invocation is based. If this value is null, the condition evaluates to true.</param>
		public ParameterizedCommand(Action<object> action, Func<object, bool> condition)
		{
			Action = new ParameterizedCommandAction(action, condition);
		}

		/// <summary>
		/// Creates a new instance of <see cref="ParameterizedCommand"/>.
		/// </summary>
		/// <param name="action">The action to be invoked.</param>
		public ParameterizedCommand(ParameterizedCommandAction action)
		{
			Action = action;
		}
		#endregion

		#region Fields
		/// <summary>
		/// The action to be invoked.
		/// </summary>
		protected ParameterizedCommandAction Action;
		#endregion

		#region Methods
		/// <summary>
		/// Invokes the command's action.
		/// </summary>
		/// <param name="parameter">
		/// Data used by the command.
		/// May be null if data is not required data.
		/// </param>
		public override void Execute(object parameter)
		{
			Action.InvokeConditionally(parameter);
		}
		#endregion
	}
}
