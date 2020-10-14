// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.Linq;

namespace System
{
	/// <summary>
	/// Provides utility methods to use with enumerations.
	/// </summary>
	public static class _Enum
	{
		/// <summary>
		/// Converts a string value representing the name or numeric
		/// value of an enumeration to its equivalent enum value.
		/// </summary>
		/// <typeparam name="T">The enumeration type to return.</typeparam>
		/// <param name="value">The string representation of the enum value.</param>
		/// <param name="result">The resulting enumeration value, returned.</param>
		/// <returns>Return true if the conversion was successful, otherwise false.</returns>
		public static bool TryParse<T>(string value, out T result) where T : struct
		{
			return TryParse<T>(value, false, out result);
		}

		/// <summary>
		/// Converts a string value representing the name or numeric
		/// value of an enumeration to its equivalent enum value.
		/// </summary>
		/// <typeparam name="T">The enumeration type to return.</typeparam>
		/// <param name="value">The string representation of the enum value.</param>
		/// <param name="ignoreCase">Indicates whether to ignore case when converting the string.</param>
		/// <param name="result">The resulting enumeration value, returned.</param>
		/// <returns>Return true if the conversion was successful, otherwise false.</returns>
		public static bool TryParse<T>(string value, bool ignoreCase, out T result) where T : struct
		{
#if NET35 // .Net 3.5
			try
			{
				decimal decimalValue;

				if (decimal.TryParse(value, out decimalValue))
				{
					var underlyingType = Enum.GetUnderlyingType(typeof(T));

					var minValue = Convert.ToDecimal(underlyingType.GetField("MinValue").GetValue(null));
					var maxValue = Convert.ToDecimal(underlyingType.GetField("MaxValue").GetValue(null));

					if (decimalValue >= minValue && decimalValue <= maxValue)
					{
						if (underlyingType == typeof(ulong))
						{
							result = (T)Enum.ToObject(typeof(T), (ulong)decimalValue);
						}
						else //All other underlying types fall within the range of long values.
						{
							result = (T)Enum.ToObject(typeof(T), (long)decimalValue);
						}

						return true;
					}
					//else the value is out of range
				}
				else
				{
					//If it's not a number, then it's a string representation of the enum value.
					//If this Parse method fails, it'll throw an exception and this method will return false.
					result = (T)Enum.Parse(typeof(T), value, ignoreCase);
					return true;
				}
			}
			catch { }


			result = default(T);
			return false;

#else // .Net 4+
			return Enum.TryParse<T>(value, out result);
#endif
		}

		/// <summary>
		/// Converts an enum value into a ulong value, maintaining its bit
		/// sequence, regardless of the original enumeration's underlying type.
		/// </summary>
		/// <param name="value">The enum value to convert.</param>
		/// <returns>
		/// Returns the enum as a ulong value, with the value's bit order maintained.
		/// </returns>
		public static ulong Normalize(this Enum value)
		{
			ulong ulongValue = 0;

			if (typeof(ulong) == Enum.GetUnderlyingType(value.GetType()))
			{
				ulongValue = Convert.ToUInt64(value);
			}
			else
			{
				//Test this scenario: uint.MaxValue => long => ulong.
				// Since the largest bit is 1, will Convert.ToInt64 keep the same bits (2's compliment).

				long longValue = Convert.ToInt64(value);
				ulongValue = (ulong)longValue;
			}

			return ulongValue;
		}

		/// <summary>
		/// Determines whether the current bitmask enum value contains the given flag value.
		/// </summary>
		/// <param name="thisValue">The current bitmask enum value.</param>
		/// <param name="flagValue">The bitmask flag to test for.</param>
		/// <remarks>
		/// This method is an alias for <see cref="HasFlag(Enum, bool, Enum)"/> with
		/// no enum type equality enforcement.
		/// </remarks>
		/// <returns>
		/// Returns true if the current enum contains the flag value, otherwise false.
		/// </returns>
		public static bool HasFlag(this Enum thisValue, Enum flagValue)
		{
			return HasFlag(thisValue, false, flagValue);
		}

		/// <summary>
		/// Determines whether the current bitmask enum value contains the given flag value.
		/// </summary>
		/// <param name="thisValue">The current bitmask enum value.</param>
		/// <param name="enforceType">Indicates whether to check the enum values for type equality.</param>
		/// <param name="flagValue">The bitmask flag to test for.</param>
		/// <returns>
		/// Returns true if the current enum contains the flag value, otherwise false.
		/// </returns>
		public static bool HasFlag(this Enum thisValue, bool enforceType, Enum flagValue)
		{
			if (enforceType && thisValue.GetType() != flagValue.GetType())
			{
				throw new ArgumentException("The enum values are not of the same type.", "flagValues");
			}
			else
			{
				ulong thisValueAsULong = Normalize(thisValue);
				ulong flagValueAsULong = Normalize(flagValue);

				return flagValueAsULong == (thisValueAsULong & flagValueAsULong);
			}
		}

		/// <summary>
		/// Determines whether the current bitmask enum value contains the given flag values.
		/// </summary>
		/// <param name="thisValue">The current bitmask enum value.</param>
		/// <param name="flagValues">The bitmask flags to test for.</param>
		/// <remarks>
		/// This method is an alias for <see cref="HasFlag(Enum, bool, Enum[])"/> with
		/// no enum type equality enforcement.
		/// </remarks>
		/// <returns>
		/// Returns true if the flag values are present in the current enum, otherwise false.
		/// </returns>
		public static bool HasFlag(this Enum thisValue, params Enum[] flagValues)
		{
			return HasFlag(thisValue, false, flagValues);
		}

		/// <summary>
		/// Determines whether the current bitmask enum value contains all the given flag values.
		/// </summary>
		/// <param name="thisValue">The current bitmask enum value.</param>
		/// <param name="enforceType">Indicates whether to check the enum values for type equality.</param>
		/// <param name="flagValues">The bitmask flags to test for.</param>
		/// <returns>
		/// Returns true if all the flag values are present in the current enum, otherwise false.
		/// </returns>
		public static bool HasFlag(this Enum thisValue, bool enforceType, params Enum[] flagValues)
		{
			Type thisValueType = thisValue.GetType();

			if (enforceType && flagValues.Any(flagValue => thisValueType != flagValue.GetType()))
			{
				throw new ArgumentException("The enum values are not of the same type.", "flagValues");
			}
			else
			{
				ulong thisValueAsULong = Normalize(thisValue);
				ulong flagValueAsULong = 0;

				foreach (var flagValue in flagValues)
				{
					flagValueAsULong = Normalize(flagValue);

					if (flagValueAsULong != (thisValueAsULong & flagValueAsULong))
					{
						return false;
					}
				}

				return true;
			}
		}

		/// <summary>
		/// Determines whether the current bitmask enum value contains any of the given flag values.
		/// </summary>
		/// <param name="thisValue">The current bitmask enum value.</param>
		/// <param name="flagValues">The bitmask flags to test for.</param>
		/// <remarks>
		/// This method is an alias for <see cref="HasAnyFlag(Enum, bool, Enum[])"/> with
		/// no enum type equality enforcement.
		/// </remarks>
		/// <returns>
		/// Returns true if any of the flag values are present in the current enum, otherwise false.
		/// </returns>
		public static bool HasAnyFlag(this Enum thisValue, params Enum[] flagValues)
		{
			return HasAnyFlag(thisValue, false, flagValues);
		}

		/// <summary>
		/// Determines whether the current bitmask enum value contains any of the given flag values.
		/// </summary>
		/// <param name="thisValue">The current bitmask enum value.</param>
		/// <param name="enforceType">Indicates whether to check the enum values for type equality.</param>
		/// <param name="flagValues">The bitmask flags to test for.</param>
		/// <returns>
		/// Returns true if any of the flag values are present in the current enum, otherwise false.
		/// </returns>
		public static bool HasAnyFlag(this Enum thisValue, bool enforceType, params Enum[] flagValues)
		{
			Type thisValueType = thisValue.GetType();

			if (enforceType && flagValues.Any(flagValue => thisValueType != flagValue.GetType()))
			{
				throw new ArgumentException("The enum values are not of the same type.", "flagValues");
			}
			else
			{
				ulong thisValueAsULong = Normalize(thisValue);
				ulong flagValueAsULong = 0;

				foreach (var flagValue in flagValues)
				{
					flagValueAsULong = Normalize(flagValue);

					if (flagValueAsULong == (thisValueAsULong & flagValueAsULong))
					{
						return true;
					}
				}

				return false;
			}
		}

	}
}
