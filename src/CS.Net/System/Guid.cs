// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

namespace System
{
	/// <summary>
	/// A static class with helper methods to simplify GUID operations.
	/// </summary>
	public static class _Guid
	{
		/// <summary>
		/// Converts the string representation of a GUID to the equivalent System.Guid structure.
		/// </summary>
		/// <param name="value">The string to convert.</param>
		/// <returns>A structure that contains the value that was parsed.</returns>
		/// <remarks>
		/// The .Net Framework 4.0 and newer provides a built-in method:
		/// System.Guid.Parse(System.String)
		/// </remarks>
		public static Guid Parse(string value)
		{
#if NET35 || NET35_CLIENT
			return Parse_Net35(value);
#else
			return Guid.Parse(value);
#endif
		}

		/// <summary>
		/// Converts the string representation of a GUID to the equivalent System.Guid structure.
		/// </summary>
		/// <param name="value">The string to convert.</param>
		/// <returns>A structure that contains the value that was parsed.</returns>
		internal static Guid Parse_Net35(string value)
		{
			return new Guid(value);
		}
	}
}
