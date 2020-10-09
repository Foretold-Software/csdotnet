// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.Diagnostics;

namespace System.Windows.Input
{
	/// <summary>
	/// A base class for implementing the ICommand interface.
	/// </summary>
	public abstract class CommandBase : ICommand
	{
		#region Events
		/// <summary>
		/// Occurs when changes occur that affect
		/// whether or not the command should execute.
		/// </summary>
		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}
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
		public virtual bool CanExecute(object parameter)
		{
			return true;
		}

		/// <summary>
		/// Invokes the command.
		/// </summary>
		/// <param name="parameter">
		/// Data used by the command.
		/// May be null if data is not required data.
		/// </param>
		public virtual void Execute(object parameter) { }
		#endregion
	}
}
