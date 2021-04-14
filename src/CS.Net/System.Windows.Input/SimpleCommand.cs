// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

namespace System.Windows.Input
{
	/// <summary>
	/// A class for implementing the ICommand interface for a simple command.
	/// </summary>
	public class SimpleCommand : CommandBase, ICommand
	{
		#region Constructors
		/// <summary>
		/// Creates a new instance of <see cref="SimpleCommand"/>.
		/// </summary>
		public SimpleCommand()
		{
			Action = new CommandAction();
		}

		/// <summary>
		/// Creates a new instance of <see cref="SimpleCommand"/>.
		/// </summary>
		/// <param name="action">The action to be invoked.</param>
		public SimpleCommand(Action action)
		{
			Action = new CommandAction(action);
		}

		/// <summary>
		/// Creates a new instance of <see cref="SimpleCommand"/>.
		/// </summary>
		/// <param name="action">The action to be invoked.</param>
		/// <param name="condition">The condition upon which the action invocation is based. If this value is null, the condition evaluates to true.</param>
		public SimpleCommand(Action action, Func<bool> condition)
		{
			Action = new CommandAction(action, condition);
		}

		/// <summary>
		/// Creates a new instance of <see cref="SimpleCommand"/>.
		/// </summary>
		/// <param name="action">The action to be invoked.</param>
		public SimpleCommand(CommandAction action)
		{
			Action = action;
		}
		#endregion

		#region Fields
		/// <summary>
		/// The action to be invoked.
		/// </summary>
		protected CommandAction Action;
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
			Action.InvokeConditionally();
		}
		#endregion
	}
}
