// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Tests
{
	[TestClass]
	public partial class GuidTest
	{
		[DataRow(null)]
		[DataRow("")]
		[DataRow("{}")]
		[DataRow("{-}")]
		[DataRow("{----}")]
		[DataRow("{0-0-0-0-0}")]
		[DataRow("00000000")]
		[DataRow("00000000-0000")]
		[DataRow("00000000-0000-0000-0000-000000000000")]
		[DataRow("4CA6938A")]
		[DataRow("4CA6938A-61D5")]
		[DataRow("4CA6938A-61D5-4404-B4E0-F1BF590EEA5E")]
		[DataRow("{00000000}")]
		[DataRow("{00000000-0000}")]
		[DataRow("{00000000-0000-0000-0000-000000000000}")]
		[DataRow("{4CA6938A}")]
		[DataRow("{4CA6938A-61D5}")]
		[DataRow("{4CA6938A-61D5-4404-B4E0-F1BF590EEA5E}")]
		[DataTestMethod]
		public void ParseTest(string value)
		{
			Assert.That.SameReturnValueAndException(
				() => Guid.Parse(value),
				() => _Guid.Parse(value));
		}

		[DataRow(null)]
		[DataRow("")]
		[DataRow("{}")]
		[DataRow("{-}")]
		[DataRow("{----}")]
		[DataRow("{0-0-0-0-0}")]
		[DataRow("00000000")]
		[DataRow("00000000-0000")]
		[DataRow("00000000-0000-0000-0000-000000000000")]
		[DataRow("4CA6938A")]
		[DataRow("4CA6938A-61D5")]
		[DataRow("4CA6938A-61D5-4404-B4E0-F1BF590EEA5E")]
		[DataRow("{00000000}")]
		[DataRow("{00000000-0000}")]
		[DataRow("{00000000-0000-0000-0000-000000000000}")]
		[DataRow("{4CA6938A}")]
		[DataRow("{4CA6938A-61D5}")]
		[DataRow("{4CA6938A-61D5-4404-B4E0-F1BF590EEA5E}")]
		[DataTestMethod]
		public void ParseTest_Net35(string value)
		{
			Assert.That.SameReturnValueAndException(
				() => Guid.Parse(value),
				() => _Guid.Parse_Net35(value));
		}
	}
}
