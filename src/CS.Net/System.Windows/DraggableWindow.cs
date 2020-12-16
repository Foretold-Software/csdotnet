// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

namespace System.Windows
{
	/// <summary>
	/// A base class for WPF window objects where a mouse press
	/// anywhere within the window results in a window drag operation.
	/// </summary>
	public class DraggableWindow : Window
	{
		/// <summary>
		/// Creates a new instance of <see cref="DraggableWindow"/>.
		/// </summary>
		public DraggableWindow()
		{
			this.MouseLeftButtonDown += (s, e) => { this.DragMove(); };
		}
	}
}
