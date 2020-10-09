// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

namespace System
{
	/// <summary>
	/// The log level of the message.
	/// </summary>
	public enum LogLevel
	{
		/// <summary>
		/// No log level specified
		/// </summary>
		None = 0,
		/// <summary>
		/// Generic informative log message
		/// </summary>
		Message = 1,
		/// <summary>
		/// Verbose logging message
		/// </summary>
		Verbose = 2,
		/// <summary>
		/// Debug log message
		/// </summary>
		Debug = 3,
		/// <summary>
		/// Error message
		/// </summary>
		Error = 4,
	}
}
