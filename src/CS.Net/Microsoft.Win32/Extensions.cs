// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

namespace Microsoft.Win32
{
	public static class Extensions
	{
		/// <summary>
		/// Gets the specified registry key's value as a string.
		/// If it is not a string, or is null, then an empty string is returned.
		/// </summary>
		/// <param name="key">The registry key whose value is checked.</param>
		/// <param name="valueName">The name of the registry key's value that is checked.</param>
		/// <returns>The value of the registry key's value, or an empty string is it is not a string or null</returns>
		public static string GetValueOrEmptyString(this RegistryKey key, string valueName)
		{
			return key.GetValue(valueName) as string ?? string.Empty;
		}
	}
}
