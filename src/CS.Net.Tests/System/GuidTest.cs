// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Tests
{
    /// <summary>This class contains parameterized unit tests for _Guid</summary>
    [TestClass]
    [PexClass(typeof(_Guid))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class GuidTest
    {

		/// <summary>Test stub for Parse(String)</summary>
		[PexMethod]
		public Guid ParseTest(string value)
		{
			Guid expected;
			Guid result;
            try
			{
				expected = new Guid(value);
				result = _Guid.Parse(value);

				if (expected != result)
				{
					Assert.Fail();
				}
            }
			catch
			{
				try
				{
					result = _Guid.Parse(value);
					Assert.Fail();
				}
				catch
				{
					return Guid.Empty;
				}
			}

			return result;
		}
	}
}
