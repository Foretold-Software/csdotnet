// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

namespace System.Windows.Input
{
	/// <summary>
	/// A class to contain a command's action and that action's invocation condition.
	/// </summary>
	public class ParameterizedCommandAction
	{
		#region Constructors
		/// <summary>
		/// Creates a new instance of <see cref="ParameterizedCommandAction"/>.
		/// </summary>
		public ParameterizedCommandAction()
		{
			Action = null;
			Condition = null;
		}

		/// <summary>
		/// Creates a new instance of <see cref="ParameterizedCommandAction"/>.
		/// </summary>
		/// <param name="action">The action to be invoked.</param>
		public ParameterizedCommandAction(Action<object> action)
		{
			Action = action;
			Condition = null;
		}

		/// <summary>
		/// Creates a new instance of <see cref="ParameterizedCommandAction"/>.
		/// </summary>
		/// <param name="action">The action to be invoked.</param>
		/// <param name="condition">
		/// The condition upon which the action invocation is based.
		/// If this value is null, the condition evaluates to true.
		/// </param>
		public ParameterizedCommandAction(Action<object> action, Func<object, bool> condition)
		{
			Action = action;
			Condition = condition;
		}
		#endregion

		#region Properties
		/// <summary>
		/// The action to be invoked.
		/// </summary>
		public Action<object> Action { get; set; }

		/// <summary>
		/// The condition upon which the action invocation is based.
		/// If this value is null, the condition evaluates to true.
		/// </summary>
		public Func<object, bool> Condition { get; set; }
		#endregion

		#region Methods
		/// <summary>
		/// Checks the <see cref="Condition"/> property.
		/// If it's null or evaluates to true, then
		/// the <see cref="Action"/> property's action is executed.
		/// </summary>
		/// <param name="parameter">
		/// Data used by the command action.
		/// May be null if data is not required data.
		/// </param>
		public void InvokeConditionally(object parameter)
		{
			if (Condition == null)
			{
				if (Action != null)
				{
					Action(parameter);
				}
			}
			else
			{
				if (Condition(parameter))
				{
					if (Action != null)
					{
						Action(parameter);
					}
				}
			}
		}
		#endregion
	}
}
