// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.Text.RegularExpressions;

namespace System.ComponentModel
{
	public static class Extensions
	{
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
