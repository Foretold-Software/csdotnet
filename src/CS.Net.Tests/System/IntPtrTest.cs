// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Tests
{
	[TestClass]
	public partial class IntPtrTest
	{
		[DataRow(0, 0)]
		[DataRow(0, 1)]
		[DataRow(0, -1)]
		[DataRow(0, int.MaxValue)]
		[DataRow(0, int.MinValue)]
		[DataRow(1, 0)]
		[DataRow(1, 1)]
		[DataRow(1, -1)]
		[DataRow(1, int.MaxValue)]
		[DataRow(1, int.MinValue)]
		[DataRow(-1, 0)]
		[DataRow(-1, 1)]
		[DataRow(-1, -1)]
		[DataRow(-1, int.MaxValue)]
		[DataRow(-1, int.MinValue)]
		[DataRow(5555, 0)]
		[DataRow(5555, 1)]
		[DataRow(5555, -1)]
		[DataRow(5555, int.MaxValue)]
		[DataRow(5555, int.MinValue)]
		[DataRow(-5555, 0)]
		[DataRow(-5555, 1)]
		[DataRow(-5555, -1)]
		[DataRow(-5555, int.MaxValue)]
		[DataRow(-5555, int.MinValue)]
		[DataRow(int.MaxValue, 0)]
		[DataRow(int.MaxValue, 1)]
		[DataRow(int.MaxValue, -1)]
		[DataRow(int.MaxValue, int.MaxValue)]
		[DataRow(int.MaxValue, int.MinValue)]
		[DataRow(int.MinValue, 0)]
		[DataRow(int.MinValue, 1)]
		[DataRow(int.MinValue, -1)]
		[DataRow(int.MinValue, int.MaxValue)]
		[DataRow(int.MinValue, int.MinValue)]
		[DataTestMethod]
		public void OffsetTest(int ptr, int offset)
		{
			Assert.That.SameReturnValueAndException(
				() => new IntPtr(ptr) + offset,
				() => _IntPtr.Offset(new IntPtr(ptr), offset));
		}

		[DataRow(0, 0)]
		[DataRow(0, 1)]
		[DataRow(0, -1)]
		[DataRow(0, int.MaxValue)]
		[DataRow(0, int.MinValue)]
		[DataRow(1, 0)]
		[DataRow(1, 1)]
		[DataRow(1, -1)]
		[DataRow(1, int.MaxValue)]
		[DataRow(1, int.MinValue)]
		[DataRow(-1, 0)]
		[DataRow(-1, 1)]
		[DataRow(-1, -1)]
		[DataRow(-1, int.MaxValue)]
		[DataRow(-1, int.MinValue)]
		[DataRow(5555, 0)]
		[DataRow(5555, 1)]
		[DataRow(5555, -1)]
		[DataRow(5555, int.MaxValue)]
		[DataRow(5555, int.MinValue)]
		[DataRow(-5555, 0)]
		[DataRow(-5555, 1)]
		[DataRow(-5555, -1)]
		[DataRow(-5555, int.MaxValue)]
		[DataRow(-5555, int.MinValue)]
		[DataRow(int.MaxValue, 0)]
		[DataRow(int.MaxValue, 1)]
		[DataRow(int.MaxValue, -1)]
		[DataRow(int.MaxValue, int.MaxValue)]
		[DataRow(int.MaxValue, int.MinValue)]
		[DataRow(int.MinValue, 0)]
		[DataRow(int.MinValue, 1)]
		[DataRow(int.MinValue, -1)]
		[DataRow(int.MinValue, int.MaxValue)]
		[DataRow(int.MinValue, int.MinValue)]
		[DataTestMethod]
		public void OffsetTest_Net35(int ptr, int offset)
		{
			Assert.That.SameReturnValueAndException(
				() => new IntPtr(ptr) + offset,
				() => _IntPtr.Offset_Net35(new IntPtr(ptr), offset));
		}
	}
}
