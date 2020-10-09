// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.Diagnostics;
using System.Linq;

namespace System.Windows.Input
{
	/// <summary>
	/// A command class for use when multiple commands are required.
	/// </summary>
	public class AnyCommand : MultiCommand, ICommand
	{
		#region Constructor
		/// <summary>
		/// Creates a new instance of <see cref="AnyCommand"/>.
		/// </summary>
		public AnyCommand() : base() { }

		/// <summary>
		/// Creates a new instance of <see cref="AnyCommand"/>.
		/// </summary>
		/// <param name="action">The action to be invoked.</param>
		public AnyCommand(Action action) : base(action) { }

		/// <summary>
		/// Creates a new instance of <see cref="AnyCommand"/>.
		/// </summary>
		/// <param name="action">The action to be invoked.</param>
		/// <param name="condition">The condition upon which the action invocation is based. If this value is null, the condition evaluates to true.</param>
		public AnyCommand(Action action, Func<bool> condition) : base(action, condition) { }

		/// <summary>
		/// Creates a new instance of <see cref="AnyCommand"/>.
		/// </summary>
		/// <param name="actions">The actions to be invoked.</param>
		public AnyCommand(params Action[] actions) : base(actions) { }

		/// <summary>
		/// Creates a new instance of <see cref="AnyCommand"/>.
		/// </summary>
		/// <param name="action">The action to be invoked.</param>
		public AnyCommand(CommandAction action) : base(action) { }

		/// <summary>
		/// Creates a new instance of <see cref="AnyCommand"/>.
		/// </summary>
		/// <param name="actions">The actions to be invoked.</param>
		public AnyCommand(params CommandAction[] actions) : base(actions) { }
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets a condition that is run once before any of the individual actions' conditions.
		/// If this condition returns true, then the individual conditions may be run.
		/// If false, then the command at large may not execute.
		/// If this condition is null, it will default to true.
		/// </summary>
		public Func<bool> OneTimeCondition { get; set; }
		#endregion

		#region Methods
		/// <summary>
		/// Determines whether the command can execute in its current state.
		/// </summary>
		/// <param name="parameter">
		/// Data used by the command.
		/// May be null if data is not required data.
		/// </param>
		/// <returns>
		/// Returns true if the command can be executed, otherwise false.
		/// </returns>
		[DebuggerStepThrough]
		public override bool CanExecute(object parameter)
		{
			if (OneTimeCondition == null || OneTimeCondition())
			{
				return Actions.Any(action => action.Condition());
			}
			else return false;
		}
		#endregion
	}
}
