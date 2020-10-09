// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.IO;

namespace System.Diagnostics
{
	/// <summary>
	/// Helper class for running external processes.
	/// </summary>
	public class _Process
	{
		#region Constructor
		private _Process(Process process)
		{
			this.Process = process;
		}
		#endregion

		#region Fields
		private readonly Process Process;
		private const bool Default_WaitForExit = true;
		private const bool Default_RedirectOutputToLog = false;
		private const bool Default_RedirectErrorToLog = false;
		private const Action<string> Default_RedirectOutputAction = null;
		private const Action<string> Default_RedirectErrorAction = null;
		#endregion

		#region Methods - Static Run
		public static Process Run(string exe, string args)
		{
			return DoRun(exe, args, Directory.GetCurrentDirectory(), Default_WaitForExit, Default_RedirectOutputAction, Default_RedirectErrorAction);
		}

		public static Process Run(string exe, string args, bool waitForExit)
		{
			return DoRun(exe, args, Directory.GetCurrentDirectory(), waitForExit, Default_RedirectOutputAction, Default_RedirectErrorAction);
		}

		public static Process Run(string exe, string args, bool waitForExit, bool redirectOutputToLog)
		{
			return DoRun(exe, args, Directory.GetCurrentDirectory(), waitForExit, redirectOutputToLog, Default_RedirectErrorToLog);
		}

		public static Process Run(string exe, string args, bool waitForExit, bool redirectOutputToLog, bool redirectErrorToLog)
		{
			return DoRun(exe, args, Directory.GetCurrentDirectory(), waitForExit, redirectOutputToLog, redirectErrorToLog);
		}

		public static Process Run(string exe, string args, bool waitForExit, Action<string> outputDataReceived)
		{
			return DoRun(exe, args, Directory.GetCurrentDirectory(), waitForExit, outputDataReceived, Default_RedirectErrorAction);
		}

		public static Process Run(string exe, string args, bool waitForExit, Action<string> outputDataReceived, Action<string> errorDataReceived)
		{
			return DoRun(exe, args, Directory.GetCurrentDirectory(), waitForExit, outputDataReceived, errorDataReceived);
		}

		public static Process Run(string exe, string args, string workingDirectory)
		{
			return DoRun(exe, args, GetWorkingDirectory(workingDirectory), Default_WaitForExit, Default_RedirectOutputAction, Default_RedirectErrorAction);
		}

		public static Process Run(string exe, string args, string workingDirectory, bool waitForExit)
		{
			return DoRun(exe, args, GetWorkingDirectory(workingDirectory), waitForExit, Default_RedirectOutputAction, Default_RedirectErrorAction);
		}

		public static Process Run(string exe, string args, string workingDirectory, bool waitForExit, bool redirectOutputToLog)
		{
			return DoRun(exe, args, GetWorkingDirectory(workingDirectory), waitForExit, redirectOutputToLog, Default_RedirectErrorToLog);
		}

		public static Process Run(string exe, string args, string workingDirectory, bool waitForExit, bool redirectOutputToLog, bool redirectErrorToLog)
		{
			return DoRun(exe, args, GetWorkingDirectory(workingDirectory), waitForExit, redirectOutputToLog, redirectErrorToLog);
		}

		public static Process Run(string exe, string args, string workingDirectory, bool waitForExit, Action<string> outputDataReceived)
		{
			return DoRun(exe, args, GetWorkingDirectory(workingDirectory), waitForExit, outputDataReceived, Default_RedirectErrorAction);
		}

		public static Process Run(string exe, string args, string workingDirectory, bool waitForExit, Action<string> outputDataReceived, Action<string> errorDataReceived)
		{
			return DoRun(exe, args, GetWorkingDirectory(workingDirectory), waitForExit, outputDataReceived, errorDataReceived);
		}

		private static Process DoRun(string exe, string args, string workingDirectory, bool waitForExit, bool redirectOutputToLog, bool redirectErrorToLog)
		{
			return DoRun(
				exe,
				args,
				GetWorkingDirectory(workingDirectory),
				waitForExit,
				redirectOutputToLog ? Log.WriteLine      : null as Action<string>,
				redirectErrorToLog  ? Log.WriteException : null as Action<string>);
		}

		private static Process DoRun(string exe, string args, string workingDirectory, bool waitForExit, Action<string> outputDataReceived, Action<string> errorDataReceived)
		{
			try
			{
				var info = new ProcessStartInfo(exe, args);
				info.RedirectStandardOutput = outputDataReceived != null;
				info.RedirectStandardError = errorDataReceived != null;
				info.UseShellExecute = false;
				info.WorkingDirectory = workingDirectory;

				var process = new Process();
				process.StartInfo = info;

				if (outputDataReceived != null)
				{
					process.OutputDataReceived += (s, e) => outputDataReceived(e.Data);
				}
				if (errorDataReceived != null)
				{
					process.ErrorDataReceived += (s, e) => errorDataReceived(e.Data);
				}

				process.Start();

				if (outputDataReceived != null)
				{
					process.BeginOutputReadLine();
				}
				if (errorDataReceived != null)
				{
					process.BeginErrorReadLine();
				}

				if (waitForExit)
				{
					process.WaitForExit();
				}

				return process;
			}
			catch (Exception exc)
			{
				throw new Exception(string.Format("Process failed: '{0}' '{1}'", exe, args), exc);
			}
		}
		#endregion

		#region Methods - Create
		public static _Process Create(string exe, string args)
		{
			return DoCreate(exe, args, Directory.GetCurrentDirectory(), Default_RedirectOutputAction, Default_RedirectErrorAction);
		}

		public static _Process Create(string exe, string args, bool redirectOutputToLog)
		{
			return DoCreate(exe, args, Directory.GetCurrentDirectory(), redirectOutputToLog, Default_RedirectErrorToLog);
		}

		public static _Process Create(string exe, string args, bool redirectOutputToLog, bool redirectErrorToLog)
		{
			return DoCreate(exe, args, Directory.GetCurrentDirectory(), redirectOutputToLog, redirectErrorToLog);
		}

		public static _Process Create(string exe, string args, Action<string> outputDataReceived)
		{
			return DoCreate(exe, args, Directory.GetCurrentDirectory(), outputDataReceived, Default_RedirectErrorAction);
		}

		public static _Process Create(string exe, string args, Action<string> outputDataReceived, Action<string> errorDataReceived)
		{
			return DoCreate(exe, args, Directory.GetCurrentDirectory(), outputDataReceived, errorDataReceived);
		}

		public static _Process Create(string exe, string args, string workingDirectory)
		{
			return DoCreate(exe, args, GetWorkingDirectory(workingDirectory), Default_RedirectOutputAction, Default_RedirectErrorAction);
		}

		public static _Process Create(string exe, string args, string workingDirectory, bool redirectOutputToLog)
		{
			return DoCreate(exe, args, GetWorkingDirectory(workingDirectory), redirectOutputToLog, Default_RedirectErrorToLog);
		}

		public static _Process Create(string exe, string args, string workingDirectory, bool redirectOutputToLog, bool redirectErrorToLog)
		{
			return DoCreate(exe, args, GetWorkingDirectory(workingDirectory), redirectOutputToLog, redirectErrorToLog);
		}

		public static _Process Create(string exe, string args, string workingDirectory, Action<string> outputDataReceived)
		{
			return DoCreate(exe, args, GetWorkingDirectory(workingDirectory), outputDataReceived, Default_RedirectErrorAction);
		}

		public static _Process Create(string exe, string args, string workingDirectory, Action<string> outputDataReceived, Action<string> errorDataReceived)
		{
			return DoCreate(exe, args, GetWorkingDirectory(workingDirectory), outputDataReceived, errorDataReceived);
		}

		private static _Process DoCreate(string exe, string args, string workingDirectory, bool redirectOutputToLog, bool redirectErrorToLog)
		{
			return DoCreate(
				exe,
				args,
				GetWorkingDirectory(workingDirectory),
				redirectOutputToLog ? Log.WriteLine      : null as Action<string>,
				redirectErrorToLog  ? Log.WriteException : null as Action<string>);
		}

		private static _Process DoCreate(string exe, string args, string workingDirectory, Action<string> outputDataReceived, Action<string> errorDataReceived)
		{
			try
			{
				var info = new ProcessStartInfo(exe, args);
				info.RedirectStandardOutput = outputDataReceived != null;
				info.RedirectStandardError = errorDataReceived != null;
				info.UseShellExecute = false;
				info.WorkingDirectory = workingDirectory;

				var process = new Process();
				process.StartInfo = info;

				if (outputDataReceived != null)
				{
					process.OutputDataReceived += (s, e) => outputDataReceived(e.Data);
				}
				if (errorDataReceived != null)
				{
					process.ErrorDataReceived += (s, e) => errorDataReceived(e.Data);
				}

				return new _Process(process);
			}
			catch (Exception exc)
			{
				throw new Exception(string.Format("Process failed: '{0}' '{1}'", exe, args), exc);
			}
		}
		#endregion

		#region Methods - Instance Run
		public Process Run()
		{
			return this.DoRun(Default_WaitForExit, false, false);
		}

		public Process Run(bool waitForExit)
		{
			return this.DoRun(waitForExit, false, false);
		}

		public Process Run(bool waitForExit, bool redirectOutput)
		{
			return this.DoRun(waitForExit, redirectOutput, false);
		}

		public Process Run(bool waitForExit, bool redirectOutput, bool redirectError)
		{
			return this.DoRun(waitForExit, redirectOutput, redirectError);
		}

		private Process DoRun(bool waitForExit, bool redirectOutput, bool redirectError)
		{
			Process process = this.Process;

			try
			{
				process.Start();

				if (redirectOutput)
				{
					process.BeginOutputReadLine();
				}
				if (redirectError)
				{
					process.BeginErrorReadLine();
				}

				if (waitForExit)
				{
					process.WaitForExit();
				}

				return process;
			}
			catch (Exception exc)
			{
				throw new Exception(string.Format("Process failed: '{0}' '{1}'", process.StartInfo.FileName, process.StartInfo.Arguments), exc);
			}
		}
		#endregion

		#region Methods - Helpers
		/// <summary>
		/// Gets a valid value for a working directory.
		/// </summary>
		/// <param name="workingDirectory">The working directory to initially attempt to use.</param>
		/// <returns>A valid working directory, or the current directory is the specified one is not a valid path.</returns>
		private static string GetWorkingDirectory(string workingDirectory)
		{
			if (workingDirectory.IsBlank())
			{
				return Directory.GetCurrentDirectory();
			}
			else
			{
				try
				{
					return new Folder(workingDirectory).FullName;
				}
				catch
				{
					return Directory.GetCurrentDirectory();
				}
			}
		}
		#endregion
	}
}
