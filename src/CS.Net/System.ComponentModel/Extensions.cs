// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.Text.RegularExpressions;

namespace System.ComponentModel
{
	/// <summary>
	/// A static class containing extension methods for classes within the <see cref="System.ComponentModel"/> namespace.
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Converts a Win32 error code to a human-readable error message string.
		/// </summary>
		/// <param name="errorcode">
		/// The Win32 error code to convert.
		/// </param>
		/// <returns>
		/// Returns a human-readable error message string corresponding to the given Win32 error code.
		/// </returns>
		public static string ToWin32ErrorMessage(this int errorcode)
		{
			string message = new Win32Exception(errorcode).Message;
			string pattern = @"\s*(.*)\s*\(0x[0-9a-fA-F]{1,8}\)";

			if (Regex.IsMatch(message, pattern))
			{
				return Regex.Match(message, pattern).Groups[1].Value;
			}
			else return message;
		}
	}
}
