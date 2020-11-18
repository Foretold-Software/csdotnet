// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

namespace System
{
	/// <summary>
	/// A class containing extension methods for objects of type <see cref="System.String"/>.
	/// </summary>
	public static class _String
	{
		/// <summary>
		/// The array of strings, in the proper order, to use for the SplitLines methods.
		/// </summary>
		private static string[] SplitLinesSeparator
			=> new string[] { "\r\n", "\n\r", "\r", "\n" };

		/// <summary>
		/// Determines whether the given string is null, empty, or contains only whitespace.
		/// </summary>
		/// <param name="value">The string to test.</param>
		/// <returns>
		/// Returns true if the string is null, empty, or contains only whitespace.
		/// Returns false otherwise.
		/// </returns>
		public static bool IsNullOrWhiteSpace(string value)
		{
#if NET35 || NET35_CLIENT
			return value == null || value.Trim().Length == 0;
#else
			return string.IsNullOrWhiteSpace(value);
#endif
		}

		/// <summary>
		/// Determines whether the given string is null, empty, or contains only whitespace.
		/// A non-blank string has length greater than zero, and at least one non-whitespace character.
		/// </summary>
		/// <param name="value">The string to test.</param>
		/// <returns>
		/// Returns true if the string is null, empty, or contains only whitespace.
		/// Returns false otherwise.
		/// </returns>
		/// <remarks>
		/// This method is intended for use in situations where the <see cref="IsNullOrWhiteSpace(string)"/> method
		/// is too long or unreadable, or in some way makes the code more cumbersome to read than it ought to be.
		/// </remarks>
		public static bool IsBlank(this string value)
		{
			return _String.IsNullOrWhiteSpace(value);
		}

		/// <summary>
		/// Trims and returns the given string, returning null if the trimmed string is empty.
		/// </summary>
		/// <param name="value">The string to trim.</param>
		/// <returns>
		/// Returns null if the string is null, empty, or contains only whitespace.
		/// Returns the trimmed string otherwise.
		/// </returns>
		public static string TrimToNull(this string value)
		{
			return TrimToNull(value, null);
		}

		/// <summary>
		/// Trims and returns the given string, returning null if the trimmed string is empty.
		/// </summary>
		/// <param name="value">The string to trim.</param>
		/// <param name="trimChars">An array of characters to remove.</param>
		/// <returns>
		/// Returns null if the string is null or the trimmed string is empty.
		/// Returns the trimmed string otherwise.
		/// </returns>
		public static string TrimToNull(this string value, params char[] trimChars)
		{
			if (value == null)
			{
				return null;
			}
			else
			{
				value = value.Trim(trimChars);

				return value.Length == 0 ? null : value;
			}

		}

		/// <summary>
		/// Returns the given string, unchanged, or null if the string is blank.
		/// </summary>
		/// <param name="value">The string to trim.</param>
		/// <returns>Returns the given string, unchanged, or null if the string is blank.</returns>
		public static string ToNullIfBlank(this string value)
		{
			return value.IsBlank() ? null : value;
		}

		/// <summary>
		/// Determines whether the given string contains only whitespace.
		/// </summary>
		/// <param name="value">The string to test.</param>
		/// <returns>
		/// Returns true if the string is comprised entirely of whitespace, and is not null or empty.
		/// Returns false otherwise.
		/// </returns>
		public static bool IsWhitespace(this string value)
		{
			return value != null && value.Length > 0 && value.Trim().Length == 0;
		}

		/// <summary>
		/// Surround the given string in double quotes.
		/// </summary>
		/// <param name="value">The string to be surrounded in double quotes.</param>
		/// <returns>The string specified, surrounded in double quotes.</returns>
		public static string WithQuotes(this string value)
		{
			return string.Format("\"{0}\"", value ?? string.Empty);
		}

		[Obsolete("Remove this method from this project.")]
		public static string Format(string format, params object[] args)
		{
			string result = string.Empty;

			if (format == null)
			{
				result = "Formatting error: format string is null.";
			}
			else if (args == null)
			{
				result = "Formatting error: format string 'args' is null. | " + format;
			}
			else
			{
				try
				{
					result = string.Format(format, args);
				}
				catch
				{
					result = "Formatting error: Issue with string formatting. | " + format;
				}
			}

			return result;
		}

		/// <summary>
		/// Splits a string based on line break characters, including combined line terminators, such as \r\n.
		/// </summary>
		/// <param name="value">The string to split.</param>
		/// <returns>Returns an array of strings, split by line terminators.</returns>
		public static string[] SplitLines(this string value)
		{
			return value.Split(SplitLinesSeparator, StringSplitOptions.None);
		}

		/// <summary>
		/// Splits a string based on line break characters, including combined line terminators, such as \r\n.
		/// </summary>
		/// <param name="value">The string to split.</param>
		/// <param name="options">String split options to use during the split.</param>
		/// <returns>Returns an array of strings, split by line terminators.</returns>
		public static string[] SplitLines(this string value, StringSplitOptions options)
		{
			return value.Split(SplitLinesSeparator, options);
		}
	}
}
