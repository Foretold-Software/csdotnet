// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Tests
{
	[TestClass]
	public partial class GuidTest
	{

		[TestMethod]
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
