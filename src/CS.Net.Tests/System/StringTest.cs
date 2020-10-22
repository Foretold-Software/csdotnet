// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Tests
{
	[TestClass]
	public partial class StringTest
	{
		[DataRow(null)]
		[DataRow("")]
		[DataRow("\"\"")]
		[DataRow("xxxxxxxx")]
		[DataRow("xxxx\"xxxxxxxx")]
		[DataRow("\"xxxxxxxx\"")]
		[DataRow("\"xxxx\"xxxx\"")]
		[DataTestMethod]
		public void WithQuotesTest(string value)
		{
			_Assert.SameReturnValueAndException(
				() => "\"" + (value ?? string.Empty) + "\"",
				() => value.WithQuotes());
		}

		[DataRow(null)]
		[DataRow("")]
		[DataRow("xxxx")]
		[DataTestMethod]
		public void IsNullOrWhiteSpaceTest(string value)
		{
			_Assert.SameReturnValueAndException(
				() => string.IsNullOrWhiteSpace(value),
				() => _String.IsNullOrWhiteSpace(value));
		}

		[DataRow(null)]
		[DataRow(""                            , "")]
		[DataRow("xxxx"                        , "xxxx")]
		[DataRow("xxxx\rxxxx"                  , "xxxx",     "xxxx")]
		[DataRow("xxxx\r\rxxxx"                , "xxxx", "", "xxxx")]
		[DataRow("xxxx\nxxxx"                  , "xxxx",     "xxxx")]
		[DataRow("xxxx\n\nxxxx"                , "xxxx", "", "xxxx")]
		[DataRow("xxxx\r\nxxxx"                , "xxxx",     "xxxx")]
		[DataRow("xxxx\r\n\r\nxxxx"            , "xxxx", "", "xxxx")]
		[DataRow("xxxx\rxxxx\rxxxx"            , "xxxx",     "xxxx",     "xxxx")]
		[DataRow("xxxx\r\rxxxx\r\rxxxx"        , "xxxx", "", "xxxx", "", "xxxx")]
		[DataRow("xxxx\nxxxx\nxxxx"            , "xxxx",     "xxxx",     "xxxx")]
		[DataRow("xxxx\n\nxxxx\n\nxxxx"        , "xxxx", "", "xxxx", "", "xxxx")]
		[DataRow("xxxx\r\nxxxx\r\nxxxx"        , "xxxx",     "xxxx",     "xxxx")]
		[DataRow("xxxx\r\n\r\nxxxx\r\n\r\nxxxx", "xxxx", "", "xxxx", "", "xxxx")]
		[DataRow("xxxx\n\r\n\rxxxx\n\r\n\rxxxx", "xxxx", "", "xxxx", "", "xxxx")]
		[DataTestMethod]
		public void SplitLinesTest(string input, params string[] expected)
		{
			string[] actual = null;

			try
			{
				actual = _String.SplitLines(input);
			}
			catch (Exception exc)
			{
				Assert.IsTrue(input == null && exc is NullReferenceException, "Unexpected exception: " + exc.GetType());
			}

			if (input != null)
			{
				_Assert.SameCollectionValues(expected, actual);
			}
		}

		[DataRow(null)]
		[DataRow(""                            , "")]
		[DataRow("xxxx"                        , "xxxx")]
		[DataRow("xxxx\rxxxx"                  , "xxxx", "xxxx")]
		[DataRow("xxxx\r\rxxxx"                , "xxxx", "xxxx")]
		[DataRow("xxxx\nxxxx"                  , "xxxx", "xxxx")]
		[DataRow("xxxx\n\nxxxx"                , "xxxx", "xxxx")]
		[DataRow("xxxx\r\nxxxx"                , "xxxx", "xxxx")]
		[DataRow("xxxx\r\n\r\nxxxx"            , "xxxx", "xxxx")]
		[DataRow("xxxx\rxxxx\rxxxx"            , "xxxx", "xxxx", "xxxx")]
		[DataRow("xxxx\r\rxxxx\r\rxxxx"        , "xxxx", "xxxx", "xxxx")]
		[DataRow("xxxx\nxxxx\nxxxx"            , "xxxx", "xxxx", "xxxx")]
		[DataRow("xxxx\n\nxxxx\n\nxxxx"        , "xxxx", "xxxx", "xxxx")]
		[DataRow("xxxx\r\nxxxx\r\nxxxx"        , "xxxx", "xxxx", "xxxx")]
		[DataRow("xxxx\r\n\r\nxxxx\r\n\r\nxxxx", "xxxx", "xxxx", "xxxx")]
		[DataRow("xxxx\n\r\n\rxxxx\n\r\n\rxxxx", "xxxx", "xxxx", "xxxx")]
		[DataTestMethod]
		public void SplitLinesTestWithRemove(string input, params string[] expected)
		{
			string[] actual = null;

			try
			{
				actual = _String.SplitLines(input, StringSplitOptions.RemoveEmptyEntries);
			}
			catch (Exception exc)
			{
				Assert.IsTrue(input == null && exc is NullReferenceException, "Unexpected exception: " + exc.GetType());
			}

			if (input == string.Empty)
			{
				Assert.IsTrue(actual != null && actual.Length == 0, "Zero-length array expected.");
			}
			else if (input != null)
			{
				_Assert.SameCollectionValues(expected, actual);
			}
		}
	}
}
