// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;

namespace System
{
	/// <summary>
	/// A callback delegate for sending string messages back to the calling assembly.
	/// </summary>
	/// <param name="message">
	/// The message to send.
	/// </param>
	public delegate void LogCallback(string message);

	/// <summary>
	/// A callback delegate for sending string messages back to the calling assembly at a particular log level.
	/// </summary>
	/// <param name="message">
	/// The message to send.
	/// </param>
	/// <param name="logLevel">
	/// The log level of the message.
	/// </param>
	public delegate void LogCallbackLeveled(string message, LogLevel logLevel);

	/// <summary>
	/// A static class containing helper methods to simplify logging operations.
	/// </summary>
	public static class Log
	{
		#region Fields
		private static string _Prefix = string.Empty;
		private static bool _PrefixDateTime = true;
		#endregion

		#region Properties
		/// <summary>
		/// Indicates whether the log is already initialized.
		/// </summary>
		public static bool IsInitialized
		{
			get { return LogCallback != null || LogCallbackLeveled != null; }
		}

		/// <summary>
		/// Gets or sets a value to use as a prefix for log entries.
		/// </summary>
		public static string Prefix
		{
			get { return _Prefix; }
			set { _Prefix = value ?? string.Empty; }
		}

		/// <summary>
		/// Indicates whether to prefix log entries with a timestamp.
		/// </summary>
		public static bool PrefixTimeStamp
		{
			get { return _PrefixDateTime; }
			set { _PrefixDateTime = value; }
		}

		/// <summary>
		/// The callback method to invoke when a log entry is created.
		/// </summary>
		public static LogCallback LogCallback { get; set; }

		/// <summary>
		/// The callback method to invoke when a log entry is created.
		/// </summary>
		public static LogCallbackLeveled LogCallbackLeveled { get; set; }
		#endregion

		#region Methods - Initialize
		/// <summary>
		/// Initializes the log.
		/// </summary>
		/// <param name="logMethod">
		/// The callback method to invoke when a log entry is created.
		/// </param>
		public static void Initialize(LogCallback logMethod)
		{
			LogCallback = logMethod;
		}

		/// <summary>
		/// Initializes the log.
		/// </summary>
		/// <param name="logMethod">
		/// The callback method to invoke when a log entry is created.
		/// </param>
		/// <param name="prefix">
		/// The value to use as a prefix for log entries.
		/// </param>
		public static void Initialize(LogCallback logMethod, string prefix)
		{
			LogCallback = logMethod;
			Prefix = prefix;
		}

		/// <summary>
		/// Initializes the log.
		/// </summary>
		/// <param name="logMethod">
		/// The callback method to invoke when a log entry is created.
		/// </param>
		/// <param name="prefixTimestamp">
		/// Indicates whether to prefix log entries with a timestamp.
		/// </param>
		public static void Initialize(LogCallback logMethod, bool prefixTimestamp)
		{
			LogCallback = logMethod;
			PrefixTimeStamp = prefixTimestamp;
		}

		/// <summary>
		/// Initializes the log.
		/// </summary>
		/// <param name="logMethod">
		/// The callback method to invoke when a log entry is created.
		/// </param>
		/// <param name="prefix">
		/// The value to use as a prefix for log entries.
		/// </param>
		/// <param name="prefixTimestamp">
		/// Indicates whether to prefix log entries with a timestamp.
		/// </param>
		public static void Initialize(LogCallback logMethod, string prefix, bool prefixTimestamp)
		{
			LogCallback = logMethod;
			Prefix = prefix;
			PrefixTimeStamp = prefixTimestamp;
		}

		/// <summary>
		/// Initializes the log.
		/// </summary>
		/// <param name="logMethod">
		/// The callback method to invoke when a log entry is created.
		/// </param>
		public static void Initialize(LogCallbackLeveled logMethod)
		{
			LogCallbackLeveled = logMethod;
		}

		/// <summary>
		/// Initializes the log.
		/// </summary>
		/// <param name="logMethod">
		/// The callback method to invoke when a log entry is created.
		/// </param>
		/// <param name="prefix">
		/// The value to use as a prefix for log entries.
		/// </param>
		public static void Initialize(LogCallbackLeveled logMethod, string prefix)
		{
			LogCallbackLeveled = logMethod;
			Prefix = prefix;
		}

		/// <summary>
		/// Initializes the log.
		/// </summary>
		/// <param name="logMethod">
		/// The callback method to invoke when a log entry is created.
		/// </param>
		/// <param name="prefixTimestamp">
		/// Indicates whether to prefix log entries with a timestamp.
		/// </param>
		public static void Initialize(LogCallbackLeveled logMethod, bool prefixTimestamp)
		{
			LogCallbackLeveled = logMethod;
			PrefixTimeStamp = prefixTimestamp;
		}

		/// <summary>
		/// Initializes the log.
		/// </summary>
		/// <param name="logMethod">
		/// The callback method to invoke when a log entry is created.
		/// </param>
		/// <param name="prefix">
		/// The value to use as a prefix for log entries.
		/// </param>
		/// <param name="prefixTimestamp">
		/// Indicates whether to prefix log entries with a timestamp.
		/// </param>
		public static void Initialize(LogCallbackLeveled logMethod, string prefix, bool prefixTimestamp)
		{
			LogCallbackLeveled = logMethod;
			Prefix = prefix;
			PrefixTimeStamp = prefixTimestamp;
		}
		#endregion

		#region Methods - Log Message Relays
		/// <summary>
		/// Creates a log entry with the given message.
		/// </summary>
		/// <param name="message">
		/// The message to include as the log entry.
		/// </param>
		public static void WriteLine(string message)
		{
			RelayMessage(GetFormattedString(message));
		}

		/// <summary>
		/// Creates a log entry with the given message.
		/// </summary>
		/// <param name="format">
		/// A formatted string to use for the log entry.
		/// </param>
		/// <param name="args">
		/// String format arguments to apply to the format string, for use as the log entry.
		/// </param>
		public static void WriteLine(string format, params object[] args)
		{
			RelayMessage(GetFormattedString(format, args));
		}

		/// <summary>
		/// Creates a log entry with the given message.
		/// </summary>
		/// <param name="logLevel">
		/// The log level of the log entry.
		/// </param>
		/// <param name="message">
		/// The message to include as the log entry.
		/// </param>
		public static void WriteLine(LogLevel logLevel, string message)
		{
			RelayMessage(GetFormattedString(message), logLevel);
		}

		/// <summary>
		/// Creates a log entry with the given message.
		/// </summary>
		/// <param name="logLevel">
		/// The log level of the log entry.
		/// </param>
		/// <param name="format">
		/// A formatted string to use for the log entry.
		/// </param>
		/// <param name="args">
		/// String format arguments to apply to the format string, for use as the log entry.
		/// </param>
		public static void WriteLine(LogLevel logLevel, string format, params object[] args)
		{
			RelayMessage(GetFormattedString(format, args), logLevel);
		}

		/// <summary>
		/// Creates a log entry with the given message and an error log level.
		/// </summary>
		/// <param name="message">
		/// The message to include as the log entry.
		/// </param>
		public static void WriteException(string message)
		{
			RelayMessage(GetFormattedString(message), LogLevel.Error);
		}

		/// <summary>
		/// Creates a log entry with the given message and an error log level.
		/// </summary>
		/// <param name="format">
		/// A formatted string to use for the log entry.
		/// </param>
		/// <param name="args">
		/// String format arguments to apply to the format string, for use as the log entry.
		/// </param>
		public static void WriteException(string format, params object[] args)
		{
			RelayMessage(GetFormattedString(format, args), LogLevel.Error);
		}

		/// <summary>
		/// Creates a log entry with an error log level, using exception and stack trace information.
		/// </summary>
		/// <param name="exception">
		/// The exception to log.
		/// </param>
		public static void WriteException(Exception exception)
		{
			string format = "{0}{1,20}: {2}";

			string hyphens = string.Empty;

			while (exception != null)
			{
				RelayMessage(GetFormattedString(format, hyphens, "Exception occurred", exception.GetType()), LogLevel.Error);
				RelayMessage(GetFormattedString(format, hyphens, "Message", exception.Message), LogLevel.Error);

				if (exception is Win32Exception)
				{
					RelayMessage(GetFormattedString(format, hyphens, "Native error code", FormatErrorCode((exception as Win32Exception).NativeErrorCode)), LogLevel.Error);
					RelayMessage(GetFormattedString(format, hyphens, "Native error message", new Win32Exception((exception as Win32Exception).NativeErrorCode).Message), LogLevel.Error);
				}
				else if (exception is ExternalException)
				{
					RelayMessage(GetFormattedString(format, hyphens, "Error code", FormatErrorCode((exception as ExternalException).ErrorCode)), LogLevel.Error);
				}
				if (exception is ArgumentException)
				{
					RelayMessage(GetFormattedString(format, hyphens, "Parameter", (exception as ArgumentException).ParamName.WithQuotes()), LogLevel.Error);
				}
				if (exception is FileNotFoundException)
				{
					RelayMessage(GetFormattedString(format, hyphens, "Filename", (exception as FileNotFoundException).FileName.WithQuotes()), LogLevel.Error);
				}

				if (!exception.StackTrace.IsBlank())
				{
					foreach (var stackTraceLine in exception.StackTrace.SplitLines(StringSplitOptions.RemoveEmptyEntries))
					{
						RelayMessage(GetFormattedString(format, hyphens, "Stack Trace", stackTraceLine), LogLevel.Error);
					}
				}

				hyphens += "-";
				exception = exception.InnerException ?? null;
			}
		}

		/// <summary>
		/// Relays the log entry to the callback delegate.
		/// </summary>
		/// <param name="message">
		/// The message to include as the log entry.
		/// </param>
		private static void RelayMessage(string message)
		{
			if (!string.IsNullOrEmpty(message))
			{
				var logMethod = Log.LogCallback;
				var logMethodLeveled = Log.LogCallbackLeveled;

				if (logMethod != null)
				{
					logMethod(message);
				}

				if (logMethodLeveled != null)
				{
					logMethodLeveled(message, LogLevel.Message);
				}
			}
		}

		/// <summary>
		/// Relays the log entry to the callback delegate.
		/// </summary>
		/// <param name="message">
		/// The message to include as the log entry.
		/// </param>
		/// <param name="logLevel">
		/// The log level of the log entry.
		/// </param>
		private static void RelayMessage(string message, LogLevel logLevel)
		{
			if (!string.IsNullOrEmpty(message))
			{
				var logMethod = Log.LogCallback;
				var logMethodLeveled = Log.LogCallbackLeveled;

				if (logMethod != null)
				{
					logMethod(message);
				}

				if (logMethodLeveled != null)
				{
					logMethodLeveled(message, logLevel);
				}
			}
		}
		#endregion

		#region Methods - GetFormattedString
		/// <summary>
		/// Formats the given message using the logging prefix and timestamp, if they are to be used.
		/// </summary>
		/// <param name="message">The string message.</param>
		/// <returns>The formatted string.</returns>
		private static string GetFormattedString(string message)
		{
			return GetFormattedPrefix() + message;
		}

		/// <summary>
		/// Formats the given message using the logging prefix and timestamp, if they are to be used.
		/// </summary>
		/// <param name="format">The string format.</param>
		/// <param name="args">The arguments to used in the formatted string.</param>
		/// <returns>The formatted string.</returns>
		private static string GetFormattedString(string format, params object[] args)
		{
			return GetFormattedPrefix() + string.Format(format ?? string.Empty, args ?? new object[0]);
		}

		/// <summary>
		/// Gets the formatted prefix for writing a line to the log.
		/// </summary>
		/// <returns>The appropriate prefix.</returns>
		private static string GetFormattedPrefix()
		{
			string prefix =
				PrefixTimeStamp ?
					DateTime.Now.ToString() + " " :
					string.Empty;

			prefix +=
				string.IsNullOrEmpty(Prefix) ?
					string.Empty :
					Prefix.IsWhitespace() ?
						Prefix :
						Prefix + " ";

			return prefix;
		}

		/// <summary>
		/// Gets a formatted error code like "-2147352571 (0x80020005)".
		/// </summary>
		/// <param name="errorCode">The error code to format.</param>
		/// <returns>A formatted error code.</returns>
		private static string FormatErrorCode(int errorCode)
		{
			return string.Format("{0} (0x{0:X8})", errorCode);
		}
		#endregion
	}
}
