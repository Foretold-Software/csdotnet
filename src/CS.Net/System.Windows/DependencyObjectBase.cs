// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.ComponentModel;

namespace System.Windows
{
	/// <summary>
	/// A base class with shortcut coding for use when creating
	/// classes that inherit from <see cref="DependencyObject"/>.
	/// </summary>
	public class DependencyObjectBase : DO
	{
		#region Properties
		/// <summary>
		/// Gets the value of the <see cref="DesignerProperties.IsInDesignModeProperty"/> attached property for the current <see cref="DependencyObject"/>.
		/// </summary>
		public bool IsInDesignMode
		{
			get
			{
				return this.GetIsInDesignMode();
			}
		}
		#endregion
	}
}
