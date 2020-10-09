// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

namespace System.Windows.Input
{
	/// <summary>
	/// A class to contain a command's action and that action's invocation condition.
	/// </summary>
	public class CommandAction
	{
		#region Constructors
		/// <summary>
		/// Creates a new instance of <see cref="CommandAction"/>.
		/// </summary>
		public CommandAction()
		{
			Action = null;
			Condition = null;
		}

		/// <summary>
		/// Creates a new instance of <see cref="CommandAction"/>.
		/// </summary>
		/// <param name="action">The action to be invoked.</param>
		public CommandAction(Action action)
		{
			Action = action;
			Condition = null;
		}

		/// <summary>
		/// Creates a new instance of <see cref="CommandAction"/>.
		/// </summary>
		/// <param name="action">The action to be invoked.</param>
		/// <param name="condition">
		/// The condition upon which the action invocation is based.
		/// If this value is null, the condition evaluates to true.
		/// </param>
		public CommandAction(Action action, Func<bool> condition)
		{
			Action = action;
			Condition = condition;
		}
		#endregion

		#region Properties
		/// <summary>
		/// The action to be invoked.
		/// </summary>
		public Action Action { get; set; }

		/// <summary>
		/// The condition upon which the action invocation is based.
		/// If this value is null, the condition evaluates to true.
		/// </summary>
		public Func<bool> Condition { get; set; }
		#endregion

		#region Methods
		/// <summary>
		/// Checks the <see cref="Condition"/> property.
		/// If it's null or evaluates to true, then
		/// the <see cref="Action"/> property's action is executed.
		/// </summary>
		public void InvokeConditionally()
		{
			if (Condition == null)
			{
				if (Action != null)
				{
					Action();
				}
			}
			else
			{
				if (Condition())
				{
					if (Action != null)
					{
						Action();
					}
				}
			}
		}
		#endregion
	}
}
