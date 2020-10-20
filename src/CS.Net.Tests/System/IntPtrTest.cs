// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Tests
{
    /// <summary>This class contains parameterized unit tests for _IntPtr</summary>
    [TestClass]
    public partial class IntPtrTest
    {
        /// <summary>Test stub for Offset(IntPtr, Int32)</summary>
        [TestMethod]
        public IntPtr OffsetTest(IntPtr ptr, int offset)
        {
            IntPtr result = _IntPtr.Offset(ptr, offset);
            return result;
            // TODO: add assertions to method _IntPtrTest.OffsetTest(IntPtr, Int32)
        }
    }
}
