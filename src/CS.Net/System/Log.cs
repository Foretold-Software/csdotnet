// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;

namespace System
{
	public delegate void LogCallback(string message);
	public delegate void LogCallbackLeveled(string message, LogLevel logLevel);

	public static class Log
	{
		#region Fields
		private static string _Prefix = string.Empty;
		private static bool _PrefixDateTime = true;
		#endregion

		#region Properties
		public static bool IsInitialized
		{
			get { return LogCallback != null || LogCallbackLeveled != null; }
		}

		public static string Prefix
		{
			get { return _Prefix; }
			set { _Prefix = value ?? string.Empty; }
		}
		public static bool PrefixTimeStamp
		{
			get { return _PrefixDateTime; }
			set { _PrefixDateTime = value; }
		}
		public static LogCallback LogCallback { get; set; }
		public static LogCallbackLeveled LogCallbackLeveled { get; set; }
		#endregion

		#region Methods - Initialize
		public static void Initialize(LogCallback logMethod)
		{
			LogCallback = logMethod;
		}
		public static void Initialize(LogCallback logMethod, string prefix)
		{
			LogCallback = logMethod;
			Prefix = prefix;
		}
		public static void Initialize(LogCallback logMethod, bool prefixTimestamp)
		{
			LogCallback = logMethod;
			PrefixTimeStamp = prefixTimestamp;
		}
		public static void Initialize(LogCallback logMethod, string prefix, bool prefixTimestamp)
		{
			LogCallback = logMethod;
			Prefix = prefix;
			PrefixTimeStamp = prefixTimestamp;
		}
		public static void Initialize(LogCallbackLeveled logMethod)
		{
			LogCallbackLeveled = logMethod;
		}
		public static void Initialize(LogCallbackLeveled logMethod, string prefix)
		{
			LogCallbackLeveled = logMethod;
			Prefix = prefix;
		}
		public static void Initialize(LogCallbackLeveled logMethod, bool prefixTimestamp)
		{
			LogCallbackLeveled = logMethod;
			PrefixTimeStamp = prefixTimestamp;
		}
		public static void Initialize(LogCallbackLeveled logMethod, string prefix, bool prefixTimestamp)
		{
			LogCallbackLeveled = logMethod;
			Prefix = prefix;
			PrefixTimeStamp = prefixTimestamp;
		}
		#endregion

		#region Methods - Log Message Relays
		public static void WriteLine(string message)
		{
			RelayMessage(GetFormattedString(message));
		}

		public static void WriteLine(string format, params object[] args)
		{
			RelayMessage(GetFormattedString(format, args));
		}

		public static void WriteLine(LogLevel logLevel, string message)
		{
			RelayMessage(GetFormattedString(message), logLevel);
		}

		public static void WriteLine(LogLevel logLevel, string format, params object[] args)
		{
			RelayMessage(GetFormattedString(format, args), logLevel);
		}

		public static void WriteException(string message)
		{
			RelayMessage(GetFormattedString(message), LogLevel.Error);
		}

		public static void WriteException(string format, params object[] args)
		{
			RelayMessage(GetFormattedString(format, args), LogLevel.Error);
		}

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
