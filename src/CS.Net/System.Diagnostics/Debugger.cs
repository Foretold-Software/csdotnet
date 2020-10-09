// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

namespace System.Diagnostics
{
	/// <summary>
	/// A class that extends the abilities of the <see cref="Debugger"/> class.
	/// </summary>
	public static class _Debugger
	{
		/// <summary>
		/// Signals a breakpoint to an attached debugger.
		/// If a debugger is not attached to the process, then one is launched and attached.
		/// </summary>
		[Conditional("DEBUG")]
		[DebuggerStepThrough]
		public static void Break()
		{
			if (Debugger.IsAttached)
				Debugger.Break();
			else
			{
				Debugger.Launch();
				Debugger.Break();
			}
		}

		/// <summary>
		/// Signals a breakpoint to an attached debugger.
		/// If a debugger is not attached to the process, then one is launched and attached.
		/// </summary>
		/// <param name="condition">
		/// A value indicating whether to launch/attach the debugger and signal a breakpoint.
		/// </param>
		[Conditional("DEBUG")]
		[DebuggerStepThrough]
		public static void Break(bool condition)
		{
			if (condition)
			{
				if (Debugger.IsAttached)
					Debugger.Break();
				else
				{
					Debugger.Launch();
					Debugger.Break();
				}
			}
		}
	}
}
