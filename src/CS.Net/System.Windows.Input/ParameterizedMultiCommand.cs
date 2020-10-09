// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.Collections.Generic;
using System.Linq;

namespace System.Windows.Input
{
	/// <summary>
	/// A base class for implementing the ICommand interface when multiple command actions are required.
	/// </summary>
	public abstract class ParameterizedMultiCommand : CommandBase, ICommand
	{
		#region Constructors
		/// <summary>
		/// Creates a new instance of <see cref="ParameterizedMultiCommand"/>.
		/// </summary>
		public ParameterizedMultiCommand()
		{
			Actions = new List<ParameterizedCommandAction>();
		}

		/// <summary>
		/// Creates a new instance of <see cref="ParameterizedMultiCommand"/>.
		/// </summary>
		/// <param name="action">The action to be invoked.</param>
		public ParameterizedMultiCommand(Action<object> action)
		{
			Actions = new List<ParameterizedCommandAction> { new ParameterizedCommandAction(action) };
		}

		/// <summary>
		/// Creates a new instance of <see cref="ParameterizedMultiCommand"/>.
		/// </summary>
		/// <param name="action">The action to be invoked.</param>
		/// <param name="condition">The condition upon which the action invocation is based. If this value is null, the condition evaluates to true.</param>
		public ParameterizedMultiCommand(Action<object> action, Func<object, bool> condition)
		{
			Actions = new List<ParameterizedCommandAction> { new ParameterizedCommandAction(action, condition) };
		}

		/// <summary>
		/// Creates a new instance of <see cref="ParameterizedMultiCommand"/>.
		/// </summary>
		/// <param name="actions">The actions to be invoked.</param>
		public ParameterizedMultiCommand(params Action<object>[] actions)
		{
			Actions = new List<ParameterizedCommandAction>(actions.Select(action => new ParameterizedCommandAction(action)));
		}

		/// <summary>
		/// Creates a new instance of <see cref="ParameterizedMultiCommand"/>.
		/// </summary>
		/// <param name="action">The action to be invoked.</param>
		public ParameterizedMultiCommand(ParameterizedCommandAction action)
		{
			Actions = new List<ParameterizedCommandAction> { action };
		}

		/// <summary>
		/// Creates a new instance of <see cref="ParameterizedMultiCommand"/>.
		/// </summary>
		/// <param name="actions">The actions to be invoked.</param>
		public ParameterizedMultiCommand(params ParameterizedCommandAction[] actions)
		{
			Actions = new List<ParameterizedCommandAction>(actions);
		}
		#endregion

		#region Fields
		/// <summary>
		/// The list of Actions to be invoked.
		/// </summary>
		protected List<ParameterizedCommandAction> Actions;
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
			Actions.ForEach(action => action.InvokeConditionally(parameter));
		}
		#endregion
	}
}
