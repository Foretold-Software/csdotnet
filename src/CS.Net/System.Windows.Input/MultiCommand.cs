// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.Collections.Generic;
using System.Linq;

namespace System.Windows.Input
{
	/// <summary>
	/// A base class for implementing the ICommand interface when multiple command actions are required.
	/// </summary>
	public abstract class MultiCommand : CommandBase, ICommand
	{
		#region Constructors
		/// <summary>
		/// Creates a new instance of <see cref="MultiCommand"/>.
		/// </summary>
		public MultiCommand()
		{
			Actions = new List<CommandAction>();
		}

		/// <summary>
		/// Creates a new instance of <see cref="MultiCommand"/>.
		/// </summary>
		/// <param name="action">The action to be invoked.</param>
		public MultiCommand(Action action)
		{
			Actions = new List<CommandAction> { new CommandAction(action) };
		}

		/// <summary>
		/// Creates a new instance of <see cref="MultiCommand"/>.
		/// </summary>
		/// <param name="action">The action to be invoked.</param>
		/// <param name="condition">The condition upon which the action invocation is based. If this value is null, the condition evaluates to true.</param>
		public MultiCommand(Action action, Func<bool> condition)
		{
			Actions = new List<CommandAction> { new CommandAction(action, condition) };
		}

		/// <summary>
		/// Creates a new instance of <see cref="MultiCommand"/>.
		/// </summary>
		/// <param name="actions">The actions to be invoked.</param>
		public MultiCommand(params Action[] actions)
		{
			Actions = new List<CommandAction>(actions.Select(action => new CommandAction(action)));
		}

		/// <summary>
		/// Creates a new instance of <see cref="MultiCommand"/>.
		/// </summary>
		/// <param name="action">The action to be invoked.</param>
		public MultiCommand(CommandAction action)
		{
			Actions = new List<CommandAction> { action };
		}

		/// <summary>
		/// Creates a new instance of <see cref="MultiCommand"/>.
		/// </summary>
		/// <param name="actions">The actions to be invoked.</param>
		public MultiCommand(params CommandAction[] actions)
		{
			Actions = new List<CommandAction>(actions);
		}
		#endregion

		#region Fields
		/// <summary>
		/// The list of Actions to be invoked.
		/// </summary>
		protected List<CommandAction> Actions;
		#endregion

		#region Methods
		/// <summary>
		/// Invokes the command's actions.
		/// </summary>
		/// <param name="parameter">
		/// Data used by the command.
		/// May be null if data is not required data.
		/// </param>
		public override void Execute(object parameter)
		{
			Actions.ForEach(action => action.InvokeConditionally());
		}
		#endregion
	}
}
