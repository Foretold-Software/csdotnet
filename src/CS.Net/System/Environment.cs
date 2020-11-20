// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

namespace System
{
	public static class _Environment
	{
		/// <summary>
		/// Determines whether the current operating system is a 64-bit operating system.
		/// </summary>
		/// <remarks>
		/// The .Net Framework 4.0 and newer provides a built-in property:
		/// System.Environment.Is64BitOperatingSystem
		/// </remarks>
		public static bool Is64BitOperatingSystem
		{
			get
			{
#if NET35 || NET35_CLIENT
				return IntPtr.Size == 8;
#else
				return Environment.Is64BitOperatingSystem;
#endif
			}
		}
	}
}
