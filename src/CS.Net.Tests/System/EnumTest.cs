// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Tests
{
	[TestClass]
	public partial class EnumTest
	{

		[TestMethod()]
		public void NormalizeTest_Byte()
		{
			ByteEnum enumValue;
			ulong expected;
			ulong actual;

			enumValue = ByteEnum.ValueMin;
			expected = (ulong)enumValue;
			actual = enumValue.Normalize();
			Assert.AreEqual(expected, actual);

			enumValue = ByteEnum.ValueMax;
			expected = (ulong)enumValue;
			actual = enumValue.Normalize();
			Assert.AreEqual(expected, actual);

			enumValue = ByteEnum.Value0;
			expected = (ulong)enumValue;
			actual = enumValue.Normalize();
			Assert.AreEqual(expected, actual);

			enumValue = ByteEnum.Value1;
			expected = (ulong)enumValue;
			actual = enumValue.Normalize();
			Assert.AreEqual(expected, actual);

			enumValue = ByteEnum.Value2;
			expected = (ulong)enumValue;
			actual = enumValue.Normalize();
			Assert.AreEqual(expected, actual);

			enumValue = ByteEnum.Value4;
			expected = (ulong)enumValue;
			actual = enumValue.Normalize();
			Assert.AreEqual(expected, actual);

			enumValue = ByteEnum.Value8;
			expected = (ulong)enumValue;
			actual = enumValue.Normalize();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod()]
		public void NormalizeTest_SByte()
		{
			SByteEnum enumValue;
			ulong expected;
			ulong actual;

			enumValue = SByteEnum.ValueMin;
			expected = (ulong)enumValue;
			actual = enumValue.Normalize();
			Assert.AreEqual(expected, actual);

			enumValue = SByteEnum.ValueMax;
			expected = (ulong)enumValue;
			actual = enumValue.Normalize();
			Assert.AreEqual(expected, actual);

			enumValue = SByteEnum.Value0;
			expected = (ulong)enumValue;
			actual = enumValue.Normalize();
			Assert.AreEqual(expected, actual);

			enumValue = SByteEnum.Value1;
			expected = (ulong)enumValue;
			actual = enumValue.Normalize();
			Assert.AreEqual(expected, actual);

			enumValue = SByteEnum.Value2;
			expected = (ulong)enumValue;
			actual = enumValue.Normalize();
			Assert.AreEqual(expected, actual);

			enumValue = SByteEnum.Value4;
			expected = (ulong)enumValue;
			actual = enumValue.Normalize();
			Assert.AreEqual(expected, actual);

			enumValue = SByteEnum.Value8;
			expected = (ulong)enumValue;
			actual = enumValue.Normalize();
			Assert.AreEqual(expected, actual);

			enumValue = SByteEnum.ValueN1;
			expected = (ulong)enumValue;
			actual = enumValue.Normalize();
			Assert.AreEqual(expected, actual);

			enumValue = SByteEnum.ValueN2;
			expected = (ulong)enumValue;
			actual = enumValue.Normalize();
			Assert.AreEqual(expected, actual);

			enumValue = SByteEnum.ValueN4;
			expected = (ulong)enumValue;
			actual = enumValue.Normalize();
			Assert.AreEqual(expected, actual);

			enumValue = SByteEnum.ValueN8;
			expected = (ulong)enumValue;
			actual = enumValue.Normalize();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod()]
		public void NormalizeTest_Long()
		{
			LongEnum enumValue;
			ulong expected;
			ulong actual;

			enumValue = LongEnum.ValueMin;
			expected = (ulong)enumValue;
			actual = enumValue.Normalize();
			Assert.AreEqual(expected, actual);

			enumValue = LongEnum.ValueMax;
			expected = (ulong)enumValue;
			actual = enumValue.Normalize();
			Assert.AreEqual(expected, actual);

			enumValue = LongEnum.Value0;
			expected = (ulong)enumValue;
			actual = enumValue.Normalize();
			Assert.AreEqual(expected, actual);

			enumValue = LongEnum.Value1;
			expected = (ulong)enumValue;
			actual = enumValue.Normalize();
			Assert.AreEqual(expected, actual);

			enumValue = LongEnum.Value2;
			expected = (ulong)enumValue;
			actual = enumValue.Normalize();
			Assert.AreEqual(expected, actual);

			enumValue = LongEnum.Value4;
			expected = (ulong)enumValue;
			actual = enumValue.Normalize();
			Assert.AreEqual(expected, actual);

			enumValue = LongEnum.Value8;
			expected = (ulong)enumValue;
			actual = enumValue.Normalize();
			Assert.AreEqual(expected, actual);

			enumValue = LongEnum.ValueN1;
			expected = (ulong)enumValue;
			actual = enumValue.Normalize();
			Assert.AreEqual(expected, actual);

			enumValue = LongEnum.ValueN2;
			expected = (ulong)enumValue;
			actual = enumValue.Normalize();
			Assert.AreEqual(expected, actual);

			enumValue = LongEnum.ValueN4;
			expected = (ulong)enumValue;
			actual = enumValue.Normalize();
			Assert.AreEqual(expected, actual);

			enumValue = LongEnum.ValueN8;
			expected = (ulong)enumValue;
			actual = enumValue.Normalize();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod()]
		public void NormalizeTest_ULong()
		{
			ULongEnum enumValue;
			ulong expected;
			ulong actual;

			enumValue = ULongEnum.ValueMin;
			expected = (ulong)enumValue;
			actual = enumValue.Normalize();
			Assert.AreEqual(expected, actual);

			enumValue = ULongEnum.ValueMax;
			expected = (ulong)enumValue;
			actual = enumValue.Normalize();
			Assert.AreEqual(expected, actual);

			enumValue = ULongEnum.Value0;
			expected = (ulong)enumValue;
			actual = enumValue.Normalize();
			Assert.AreEqual(expected, actual);

			enumValue = ULongEnum.Value1;
			expected = (ulong)enumValue;
			actual = enumValue.Normalize();
			Assert.AreEqual(expected, actual);

			enumValue = ULongEnum.Value2;
			expected = (ulong)enumValue;
			actual = enumValue.Normalize();
			Assert.AreEqual(expected, actual);

			enumValue = ULongEnum.Value4;
			expected = (ulong)enumValue;
			actual = enumValue.Normalize();
			Assert.AreEqual(expected, actual);

			enumValue = ULongEnum.Value8;
			expected = (ulong)enumValue;
			actual = enumValue.Normalize();
			Assert.AreEqual(expected, actual);
		}

		[TestMethod()]
		public void HasFlagTest_Byte()
		{
			ByteEnum value;

			value = ByteEnum.ValueMin;
			if (value.HasFlag(ByteEnum.Value1) || value.HasFlag(ByteEnum.Value2) || value.HasFlag(ByteEnum.Value4) || value.HasFlag(ByteEnum.Value8) || value.HasFlag(ByteEnum.ValueMax))
			{
				Assert.Fail();
			}

			value = ByteEnum.ValueMax;
			if (!(value.HasFlag(ByteEnum.Value1) && value.HasFlag(ByteEnum.Value2) && value.HasFlag(ByteEnum.Value4) && value.HasFlag(ByteEnum.Value8) && value.HasFlag(ByteEnum.ValueMin) && value.HasFlag(ByteEnum.ValueMax)))
			{
				Assert.Fail();
			}

			value = ByteEnum.Value2 | ByteEnum.Value8;
			if (value.HasFlag(ByteEnum.Value4) || value.HasFlag(ByteEnum.Value1))
			{
				Assert.Fail();
			}
			if (!value.HasFlag(ByteEnum.Value2) || !value.HasFlag(ByteEnum.Value8))
			{
				Assert.Fail();
			}
			if (!value.HasFlag(value))
			{
				Assert.Fail();
			}
		}

		[TestMethod()]
		public void HasFlagTest_Long()
		{
			LongEnum value;

			value = LongEnum.ValueMax;
			if (!(value.HasFlag(LongEnum.Value1) && value.HasFlag(LongEnum.Value2) && value.HasFlag(LongEnum.Value4) && value.HasFlag(LongEnum.Value8) && value.HasFlag(LongEnum.ValueMax)))
			{
				Assert.Fail();
			}

			value = LongEnum.Value2 | LongEnum.Value8;
			if (value.HasFlag(LongEnum.Value4) || value.HasFlag(LongEnum.Value1))
			{
				Assert.Fail();
			}
			if (!value.HasFlag(LongEnum.Value2) || !value.HasFlag(LongEnum.Value8))
			{
				Assert.Fail();
			}
			if (!value.HasFlag(value))
			{
				Assert.Fail();
			}
		}

		[TestMethod()]
		public void HasFlagTest_ULong()
		{
			ULongEnum value;

			value = ULongEnum.ValueMin;
			if (value.HasFlag(ULongEnum.Value1) || value.HasFlag(ULongEnum.Value2) || value.HasFlag(ULongEnum.Value4) || value.HasFlag(ULongEnum.Value8) || value.HasFlag(ULongEnum.ValueMax))
			{
				Assert.Fail();
			}

			value = ULongEnum.ValueMax;
			if (!(value.HasFlag(ULongEnum.Value1) && value.HasFlag(ULongEnum.Value2) && value.HasFlag(ULongEnum.Value4) && value.HasFlag(ULongEnum.Value8) && value.HasFlag(ULongEnum.ValueMin) && value.HasFlag(ULongEnum.ValueMax)))
			{
				Assert.Fail();
			}

			value = ULongEnum.Value2 | ULongEnum.Value8;
			if (value.HasFlag(ULongEnum.Value4) || value.HasFlag(ULongEnum.Value1))
			{
				Assert.Fail();
			}
			if (!value.HasFlag(ULongEnum.Value2) || !value.HasFlag(ULongEnum.Value8))
			{
				Assert.Fail();
			}
			if (!value.HasFlag(value))
			{
				Assert.Fail();
			}
		}

		[TestMethod()]
		public void HasAnyFlagTest_Byte()
		{
			ByteEnum value;

			value = ByteEnum.ValueMin;
			if (value.HasAnyFlag(ByteEnum.Value1, ByteEnum.Value2, ByteEnum.Value4, ByteEnum.Value8, ByteEnum.ValueMax))
			{
				Assert.Fail();
			}

			value = ByteEnum.ValueMax;
			if (!(value.HasAnyFlag(ByteEnum.Value1) && value.HasAnyFlag(ByteEnum.Value2) && value.HasAnyFlag(ByteEnum.Value4) && value.HasAnyFlag(ByteEnum.Value8) && value.HasAnyFlag(ByteEnum.ValueMin) && value.HasAnyFlag(ByteEnum.ValueMax)))
			{
				Assert.Fail();
			}

			value = ByteEnum.Value2 | ByteEnum.Value8;
			if (value.HasAnyFlag(ByteEnum.Value4, ByteEnum.Value1))
			{
				Assert.Fail();
			}
			if (!value.HasAnyFlag(ByteEnum.Value2, ByteEnum.Value8))
			{
				Assert.Fail();
			}
			if (!value.HasAnyFlag(value))
			{
				Assert.Fail();
			}
		}

		[TestMethod]
		public void TryParseTest()
		{
			UShortEnum expected, actual;
			bool expectedTry, actualTry;
			Exception expectedExc, actualExc;

			bool[] ignoreCaseValues = { true, false };
			string[] inputValues =
			{
				null,
				"",
				" ",
				"\t",
				"	",
				"\r\n",
				"\0",
				"\0a",
				"0",
				"4",
				"6",
				"Value4",
				"UShortEnum.Value4",
				"-4",
				ushort.MinValue.ToString(),
				ushort.MaxValue.ToString(),
				(ushort.MinValue - 1).ToString(),
				(ushort.MaxValue + 1).ToString(),
				"MinValue",
				"MaxValue",
				UShortEnum.Value4.ToString(),
				(UShortEnum.Value4 | UShortEnum.Value2).ToString(),
				"jasdfwual8uw4qr89auiwj3eawrp834w8eijdsfd"
			};

			foreach (var input in inputValues)
			{
				foreach (var ignoreCase in ignoreCaseValues)
				{
					expected = actual = default(UShortEnum);
					expectedTry = actualTry = default(bool);
					expectedExc = actualExc = default(Exception);

					try
					{
						expectedTry = Enum.TryParse(input, ignoreCase, out expected);
					}
					catch (Exception exc)
					{
						expectedExc = exc;
					}

					try
					{
						actualTry = _Enum.TryParse(input, ignoreCase, out actual);
					}
					catch (Exception exc)
					{
						actualExc = exc;
					}


					if (expectedExc != null)
					{
						if (actualExc != null)
						{
							Assert.AreEqual(expectedExc.GetType(), actualExc.GetType());
							Assert.AreEqual(expectedExc.Message, actualExc.Message);
						}
						else
						{
							Assert.Fail("Exception expected. Type: '{0}' Message: '{1}'", expectedExc.GetType(), expectedExc.Message ?? "null");
						}
					}
					else if (actualExc != null)
					{
						Assert.Fail("No exceptions should be thrown by this method.");
					}
					else //no exceptions by either.
					{
						Assert.AreEqual(expected, actual);
						Assert.AreEqual(expectedTry, actualTry);
					}
				}
			}
		}

		[TestMethod]
		public void TryParseTest_Net35()
		{
			UShortEnum expected, actual;
			bool expectedTry, actualTry;
			Exception expectedExc, actualExc;

			bool[] ignoreCaseValues = { true, false };
			string[] inputValues =
			{
				null,
				"",
				" ",
				"\t",
				"	",
				"\r\n",
				"\0",
				"\0a",
				"0",
				"4",
				"6",
				"Value4",
				"UShortEnum.Value4",
				"-4",
				ushort.MinValue.ToString(),
				ushort.MaxValue.ToString(),
				(ushort.MinValue - 1).ToString(),
				(ushort.MaxValue + 1).ToString(),
				"MinValue",
				"MaxValue",
				UShortEnum.Value4.ToString(),
				(UShortEnum.Value4 | UShortEnum.Value2).ToString(),
				"jasdfwual8uw4qr89auiwj3eawrp834w8eijdsfd"
			};

			foreach (var input in inputValues)
			{
				foreach (var ignoreCase in ignoreCaseValues)
				{
					expected = actual = default(UShortEnum);
					expectedTry = actualTry = default(bool);
					expectedExc = actualExc = default(Exception);

					try
					{
						expectedTry = Enum.TryParse(input, ignoreCase, out expected);
					}
					catch (Exception exc)
					{
						expectedExc = exc;
					}

					try
					{
						actualTry = _Enum.TryParse_Net35(input, ignoreCase, out actual);
					}
					catch (Exception exc)
					{
						actualExc = exc;
					}


					if (expectedExc != null)
					{
						if (actualExc != null)
						{
							Assert.AreEqual(expectedExc.GetType(), actualExc.GetType());
							Assert.AreEqual(expectedExc.Message, actualExc.Message);
						}
						else
						{
							Assert.Fail("Exception expected. Type: '{0}' Message: '{1}'", expectedExc.GetType(), expectedExc.Message ?? "null");
						}
					}
					else if (actualExc != null)
					{
						Assert.Fail("No exceptions should be thrown by this method.");
					}
					else //no exceptions by either.
					{
						Assert.AreEqual(expected, actual);
						Assert.AreEqual(expectedTry, actualTry);
					}
				}
			}
		}
	}
}
