// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

namespace System
{
	public static class _Guid
	{
		public static Guid Parse(string value)
		{
#if DOTNET35
			return new Guid(value);
#elif DOTNET45
		return Guid.Parse(value);
#endif
		}
	}
}
