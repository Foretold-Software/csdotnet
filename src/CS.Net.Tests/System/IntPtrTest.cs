// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Tests
{
	[TestClass]
	public partial class IntPtrTest
	{
		[TestMethod]
		public IntPtr OffsetTest(IntPtr ptr, int offset)
		{
			IntPtr result = _IntPtr.Offset(ptr, offset);
			return result;
			// TODO: add assertions to method _IntPtrTest.OffsetTest(IntPtr, Int32)
		}
	}
}
