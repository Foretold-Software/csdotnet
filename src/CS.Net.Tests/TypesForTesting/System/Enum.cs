// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

namespace System
{
	//Note: Enums' underlying type can be any of the following.
	//      byte, sbyte, short, ushort, int, uint, long, ulong
	//      This file contains an enum structure for each of
	//      the 8 possible underlying types.

	internal enum ByteEnum : byte
	{
		Value0 = 0,
		Value1 = 1,
		Value2 = 2,
		Value4 = 4,
		Value8 = 8,
		ValueMin = byte.MinValue,
		ValueMax = byte.MaxValue
	}

	internal enum SByteEnum : sbyte
	{
		Value0 = 0,
		Value1 = 1,
		Value2 = 2,
		Value4 = 4,
		Value8 = 8,
		ValueN1 = -1,
		ValueN2 = -2,
		ValueN4 = -4,
		ValueN8 = -8,
		ValueMin = sbyte.MinValue,
        ValueMax = sbyte.MaxValue
	}

	internal enum ShortEnum : short
	{
		Value0 = 0,
		Value1 = 1,
		Value2 = 2,
		Value4 = 4,
		Value8 = 8,
		ValueN1 = -1,
		ValueN2 = -2,
		ValueN4 = -4,
		ValueN8 = -8,
		ValueMin = short.MinValue,
		ValueMax = short.MaxValue
	}

	internal enum UShortEnum : ushort
	{
		Value0 = 0,
		Value1 = 1,
		Value2 = 2,
		Value4 = 4,
		Value8 = 8,
		ValueMin = ushort.MinValue,
		ValueMax = ushort.MaxValue
	}

	internal enum IntEnum : int
	{
		Value0 = 0,
		Value1 = 1,
		Value2 = 2,
		Value4 = 4,
		Value8 = 8,
		ValueN1 = -1,
		ValueN2 = -2,
		ValueN4 = -4,
		ValueN8 = -8,
		ValueMin = int.MinValue,
		ValueMax = int.MaxValue
	}

	internal enum UIntEnum : uint
	{
		Value0 = 0,
		Value1 = 1,
		Value2 = 2,
		Value4 = 4,
		Value8 = 8,
		ValueMin = uint.MinValue,
		ValueMax = uint.MaxValue
	}

	internal enum LongEnum : long
	{
		Value0 = 0,
		Value1 = 1,
		Value2 = 2,
		Value4 = 4,
		Value8 = 8,
		ValueN1 = -1,
		ValueN2 = -2,
		ValueN4 = -4,
		ValueN8 = -8,
		ValueMin = long.MinValue,
		ValueMax = long.MaxValue
	}

	internal enum ULongEnum : ulong
	{
		Value0 = 0,
		Value1 = 1,
		Value2 = 2,
		Value4 = 4,
		Value8 = 8,
		ValueMin = ulong.MinValue,
		ValueMax = ulong.MaxValue
	}
}
