// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

namespace System
{
	public static class _Environment
	{
		public static bool Is64BitOperatingSystem
		{
			get
			{
#if DOTNET35
				// .Net 3.5
				return IntPtr.Size == 8;
#elif DOTNET45
				// .Net 4+
				return Environment.Is64BitOperatingSystem;
#endif
			}
		}
	}
}
