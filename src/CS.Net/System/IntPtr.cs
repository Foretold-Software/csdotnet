// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

namespace System
{
	/// <summary>
	/// A static class with helper methods to simplify pointer operations.
	/// </summary>
	public static class _IntPtr
	{
		/// <summary>
		/// Adds an offset to the value of a pointer.
		/// </summary>
		/// <param name="ptr">The pointer to add the offset to.</param>
		/// <param name="offset">The offset to add.</param>
		/// <returns>A new pointer that reflects the addition of offset to pointer.</returns>
		/// <remarks>
		/// The .Net Framework 4.0 and newer provides a built-in operator:
		/// System.IntPtr + System.Int32
		/// </remarks>
		public static IntPtr Offset(IntPtr ptr, int offset)
		{
#if NET35 || NET35_CLIENT
			return Offset_Net35(ptr, offset);
#else
			return ptr + offset;
#endif
		}

		/// <summary>
		/// Adds an offset to the value of a pointer.
		/// </summary>
		/// <param name="ptr">The pointer to add the offset to.</param>
		/// <param name="offset">The offset to add.</param>
		/// <returns>A new pointer that reflects the addition of offset to pointer.</returns>
		/// <remarks>
		/// The .Net Framework 4.0 and newer provides a built-in operator:
		/// System.IntPtr + System.Int32
		/// </remarks>
		internal static IntPtr Offset_Net35(IntPtr ptr, int offset)
		{
			//TODO IMPORTANT ASAP: Test this somehow to make sure the values are correct.
			if (IntPtr.Size == 8)
			{
				return new IntPtr(ptr.ToInt64() + offset);
			}
			else
			{
				return new IntPtr(ptr.ToInt32() + offset);
			}
		}
	}
}
