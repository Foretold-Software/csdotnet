// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Tests
{
	/// <summary>This class contains parameterized unit tests for _String</summary>
	[TestClass]
	public partial class StringTest
	{

		[TestMethod]
		public string WithQuotesTest(string value)
		{
			string expected = null;
			string actual = null;

			if (string.IsNullOrEmpty(value))
			{
				expected = "\"\"";
			}
			else
			{
				expected = "\"" + value + "\"";
			}

			actual = value.WithQuotes();

			Assert.AreEqual(expected, actual, "Values are not the same: '{0}' '{1}'", expected, actual);

			return actual;
		}

		/// <summary>Test stub for IsNullOrWhiteSpace(String)</summary>
		[TestMethod]
		public bool IsNullOrWhiteSpaceTest(string value)
		{
			bool expected = string.IsNullOrWhiteSpace(value);
			bool actual = _String.IsNullOrWhiteSpace(value);

			Assert.AreEqual(expected, actual);

			return actual;
		}

		/// <summary>Test stub for SplitLines(String)</summary>
		[TestMethod]
		public string[] SplitLinesTest(string value)
		{
			//TODO: Complete this method.
			string[] expected = null;

			try
			{
				expected = value.Split('\r', '\n');
			}
			catch (NullReferenceException)
			{
				if (value == null)
				{

				}
			}



			string[] actual = _String.SplitLines(value);



			Assert.AreEqual(expected.Length, actual.Length);

			for (int i = 0; i < expected.Length; i++)
			{
				Assert.AreEqual(expected[i], actual[i]);
			}

			return actual;
			// TODO: add assertions to method _StringTest.SplitLinesTest(String)
		}

		/// <summary>Test stub for SplitLines(String, StringSplitOptions)</summary>
		[TestMethod]
		public string[] SplitLinesTest01(string value, StringSplitOptions options)
		{
			string[] expected = _String.SplitLines(value, options);
			string[] actual = _String.SplitLines(value, options);




			return actual;
			// TODO: add assertions to method _StringTest.SplitLinesTest01(String, StringSplitOptions)
		}
	}
}
