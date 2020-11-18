// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

namespace System
{
	public static class _IntPtr
	{
		public static IntPtr Offset(IntPtr ptr, int offset)
		{
#if NET35 || NET35_CLIENT
			// .Net 3.5
			//TODO IMPORTANT ASAP: Test this somehow to make sure the values are correct.
			if (IntPtr.Size == 8)
			{
				return new IntPtr(ptr.ToInt64() + offset);
			}
			else
			{
				return new IntPtr(ptr.ToInt32() + offset);
			}
#else
		// .Net 4+
		return ptr + offset;
#endif
		}
	}
}
