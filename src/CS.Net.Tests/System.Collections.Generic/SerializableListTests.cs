// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Collections.Generic.Tests
{
	[TestClass]
	public class SerializableListTests
	{
		[DataRow("",         null)]
		[DataRow("",         "")]
		[DataRow(",",        "", null)]
		[DataRow(",",        "", "")]
		[DataRow(",,",       "", null, "")]
		[DataRow("one",      "one")]
		[DataRow("one,two",  "one", "two")]
		[DataRow("one,,two", "one", null, "two")]
		[DataRow("one,,two", "one", "",   "two")]
		[DataTestMethod]
		public void ToStringTest(string expected, params string[] input)
		{
			//Replacing the delimiter here means that the delimiter in
			// the 'expected' string does not need to be updated
			// if SerializableList.delimiter is changed.
			expected = expected.Replace(",", SerializableList.delimiter);

			string actual = new SerializableList<string>(input).ToString();

			Assert.AreEqual(expected, actual);
		}

		[DataRow("",         null)]
		[DataRow("",         "")]
		[DataRow(",",        "", null)]
		[DataRow(",",        "", "")]
		[DataRow(",,",       "", null, "")]
		[DataRow("one",      "one")]
		[DataRow("one,two",  "one", "two")]
		[DataRow("one,,two", "one", null, "two")]
		[DataRow("one,,two", "one", "",   "two")]
		[DataTestMethod]
		public void ToStringTest_Net35(string expected, params string[] input)
		{
			//Replacing the delimiter here means that the delimiter in
			// the 'expected' string does not need to be updated
			// if SerializableList.delimiter is changed.
			expected = expected.Replace(",", SerializableList.delimiter);

			string actual = SerializableList.ToString_Net35(new SerializableList<string>(input));

			Assert.AreEqual(expected, actual);
		}
	}
}
