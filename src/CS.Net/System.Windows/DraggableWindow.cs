// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

namespace System.Windows
{
	public class DraggableWindow : Window
	{
		public DraggableWindow()
		{
			this.MouseLeftButtonDown += (s, e) => { this.DragMove(); };
		}
	}
}
