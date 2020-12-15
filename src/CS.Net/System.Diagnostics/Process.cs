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
		/// <summary>
		/// Creates and runs a new process using the specified parameters.
		/// </summary>
		/// <param name="exe">
		/// The executable file to run.
		/// </param>
		/// <param name="args">
		/// The command line arguments to pass to the process, as a single string.
		/// </param>
		/// <returns>
		/// Returns the <see cref="Diagnostics.Process"/> object created to run the process.
		/// </returns>
		public static Process Run(string exe, string args)
		{
			return DoRun(exe, args, Directory.GetCurrentDirectory(), Default_WaitForExit, Default_RedirectOutputAction, Default_RedirectErrorAction);
		}

		/// <summary>
		/// Creates and runs a new process using the specified parameters.
		/// </summary>
		/// <param name="exe">
		/// The executable file to run.
		/// </param>
		/// <param name="args">
		/// The command line arguments to pass to the process, as a single string.
		/// </param>
		/// <param name="waitForExit">
		/// Indicates whether the method will wait for the process to exit before returning.
		/// </param>
		/// <returns>
		/// Returns the <see cref="Diagnostics.Process"/> object created to run the process.
		/// </returns>
		public static Process Run(string exe, string args, bool waitForExit)
		{
			return DoRun(exe, args, Directory.GetCurrentDirectory(), waitForExit, Default_RedirectOutputAction, Default_RedirectErrorAction);
		}

		/// <summary>
		/// Creates and runs a new process using the specified parameters.
		/// </summary>
		/// <param name="exe">
		/// The executable file to run.
		/// </param>
		/// <param name="args">
		/// The command line arguments to pass to the process, as a single string.
		/// </param>
		/// <param name="waitForExit">
		/// Indicates whether the method will wait for the process to exit before returning.
		/// </param>
		/// <param name="redirectOutputToLog">
		/// Indicates whether to redirect stdout output to the <see cref="System.Log.WriteLine(string)"/> method.
		/// </param>
		/// <returns>
		/// Returns the <see cref="Diagnostics.Process"/> object created to run the process.
		/// </returns>
		public static Process Run(string exe, string args, bool waitForExit, bool redirectOutputToLog)
		{
			return DoRun(exe, args, Directory.GetCurrentDirectory(), waitForExit, redirectOutputToLog, Default_RedirectErrorToLog);
		}

		/// <summary>
		/// Creates and runs a new process using the specified parameters.
		/// </summary>
		/// <param name="exe">
		/// The executable file to run.
		/// </param>
		/// <param name="args">
		/// The command line arguments to pass to the process, as a single string.
		/// </param>
		/// <param name="waitForExit">
		/// Indicates whether the method will wait for the process to exit before returning.
		/// </param>
		/// <param name="redirectOutputToLog">
		/// Indicates whether to redirect stdout output to the <see cref="System.Log.WriteLine(string)"/> method.
		/// </param>
		/// <param name="redirectErrorToLog">
		/// Indicates whether to redirect stderr output to the <see cref="System.Log.WriteException(string)"/> method.
		/// </param>
		/// <returns>
		/// Returns the <see cref="Diagnostics.Process"/> object created to run the process.
		/// </returns>
		public static Process Run(string exe, string args, bool waitForExit, bool redirectOutputToLog, bool redirectErrorToLog)
		{
			return DoRun(exe, args, Directory.GetCurrentDirectory(), waitForExit, redirectOutputToLog, redirectErrorToLog);
		}

		/// <summary>
		/// Creates and runs a new process using the specified parameters.
		/// </summary>
		/// <param name="exe">
		/// The executable file to run.
		/// </param>
		/// <param name="args">
		/// The command line arguments to pass to the process, as a single string.
		/// </param>
		/// <param name="waitForExit">
		/// Indicates whether the method will wait for the process to exit before returning.
		/// </param>
		/// <param name="outputDataReceived">
		/// The action to perform when stdout output is received from the running process.
		/// </param>
		/// <returns>
		/// Returns the <see cref="Diagnostics.Process"/> object created to run the process.
		/// </returns>
		public static Process Run(string exe, string args, bool waitForExit, Action<string> outputDataReceived)
		{
			return DoRun(exe, args, Directory.GetCurrentDirectory(), waitForExit, outputDataReceived, Default_RedirectErrorAction);
		}

		/// <summary>
		/// Creates and runs a new process using the specified parameters.
		/// </summary>
		/// <param name="exe">
		/// The executable file to run.
		/// </param>
		/// <param name="args">
		/// The command line arguments to pass to the process, as a single string.
		/// </param>
		/// <param name="waitForExit">
		/// Indicates whether the method will wait for the process to exit before returning.
		/// </param>
		/// <param name="outputDataReceived">
		/// The action to perform when stdout output is received from the running process.
		/// </param>
		/// <param name="errorDataReceived">
		/// The action to perform when stderr output is received from the running process.
		/// </param>
		/// <returns>
		/// Returns the <see cref="Diagnostics.Process"/> object created to run the process.
		/// </returns>
		public static Process Run(string exe, string args, bool waitForExit, Action<string> outputDataReceived, Action<string> errorDataReceived)
		{
			return DoRun(exe, args, Directory.GetCurrentDirectory(), waitForExit, outputDataReceived, errorDataReceived);
		}

		/// <summary>
		/// Creates and runs a new process using the specified parameters.
		/// </summary>
		/// <param name="exe">
		/// The executable file to run.
		/// </param>
		/// <param name="args">
		/// The command line arguments to pass to the process, as a single string.
		/// </param>
		/// <param name="workingDirectory">
		/// The working directory to use for the process.
		/// If the directory does not exist, it will be created.
		/// </param>
		/// <returns>
		/// Returns the <see cref="Diagnostics.Process"/> object created to run the process.
		/// </returns>
		public static Process Run(string exe, string args, string workingDirectory)
		{
			return DoRun(exe, args, GetWorkingDirectory(workingDirectory), Default_WaitForExit, Default_RedirectOutputAction, Default_RedirectErrorAction);
		}

		/// <summary>
		/// Creates and runs a new process using the specified parameters.
		/// </summary>
		/// <param name="exe">
		/// The executable file to run.
		/// </param>
		/// <param name="args">
		/// The command line arguments to pass to the process, as a single string.
		/// </param>
		/// <param name="workingDirectory">
		/// The working directory to use for the process.
		/// If the directory does not exist, it will be created.
		/// </param>
		/// <param name="waitForExit">
		/// Indicates whether the method will wait for the process to exit before returning.
		/// </param>
		/// <returns>
		/// Returns the <see cref="Diagnostics.Process"/> object created to run the process.
		/// </returns>
		public static Process Run(string exe, string args, string workingDirectory, bool waitForExit)
		{
			return DoRun(exe, args, GetWorkingDirectory(workingDirectory), waitForExit, Default_RedirectOutputAction, Default_RedirectErrorAction);
		}

		/// <summary>
		/// Creates and runs a new process using the specified parameters.
		/// </summary>
		/// <param name="exe">
		/// The executable file to run.
		/// </param>
		/// <param name="args">
		/// The command line arguments to pass to the process, as a single string.
		/// </param>
		/// <param name="workingDirectory">
		/// The working directory to use for the process.
		/// If the directory does not exist, it will be created.
		/// </param>
		/// <param name="waitForExit">
		/// Indicates whether the method will wait for the process to exit before returning.
		/// </param>
		/// <param name="redirectOutputToLog">
		/// Indicates whether to redirect stdout output to the <see cref="System.Log.WriteLine(string)"/> method.
		/// </param>
		/// <returns>
		/// Returns the <see cref="Diagnostics.Process"/> object created to run the process.
		/// </returns>
		public static Process Run(string exe, string args, string workingDirectory, bool waitForExit, bool redirectOutputToLog)
		{
			return DoRun(exe, args, GetWorkingDirectory(workingDirectory), waitForExit, redirectOutputToLog, Default_RedirectErrorToLog);
		}

		/// <summary>
		/// Creates and runs a new process using the specified parameters.
		/// </summary>
		/// <param name="exe">
		/// The executable file to run.
		/// </param>
		/// <param name="args">
		/// The command line arguments to pass to the process, as a single string.
		/// </param>
		/// <param name="workingDirectory">
		/// The working directory to use for the process.
		/// If the directory does not exist, it will be created.
		/// </param>
		/// <param name="waitForExit">
		/// Indicates whether the method will wait for the process to exit before returning.
		/// </param>
		/// <param name="redirectOutputToLog">
		/// Indicates whether to redirect stdout output to the <see cref="System.Log.WriteLine(string)"/> method.
		/// </param>
		/// <param name="redirectErrorToLog">
		/// Indicates whether to redirect stderr output to the <see cref="System.Log.WriteException(string)"/> method.
		/// </param>
		/// <returns>
		/// Returns the <see cref="Diagnostics.Process"/> object created to run the process.
		/// </returns>
		public static Process Run(string exe, string args, string workingDirectory, bool waitForExit, bool redirectOutputToLog, bool redirectErrorToLog)
		{
			return DoRun(exe, args, GetWorkingDirectory(workingDirectory), waitForExit, redirectOutputToLog, redirectErrorToLog);
		}

		/// <summary>
		/// Creates and runs a new process using the specified parameters.
		/// </summary>
		/// <param name="exe">
		/// The executable file to run.
		/// </param>
		/// <param name="args">
		/// The command line arguments to pass to the process, as a single string.
		/// </param>
		/// <param name="workingDirectory">
		/// The working directory to use for the process.
		/// If the directory does not exist, it will be created.
		/// </param>
		/// <param name="waitForExit">
		/// Indicates whether the method will wait for the process to exit before returning.
		/// </param>
		/// <param name="outputDataReceived">
		/// The action to perform when stdout output is received from the running process.
		/// </param>
		/// <returns>
		/// Returns the <see cref="Diagnostics.Process"/> object created to run the process.
		/// </returns>
		public static Process Run(string exe, string args, string workingDirectory, bool waitForExit, Action<string> outputDataReceived)
		{
			return DoRun(exe, args, GetWorkingDirectory(workingDirectory), waitForExit, outputDataReceived, Default_RedirectErrorAction);
		}

		/// <summary>
		/// Creates and runs a new process using the specified parameters.
		/// </summary>
		/// <param name="exe">
		/// The executable file to run.
		/// </param>
		/// <param name="args">
		/// The command line arguments to pass to the process, as a single string.
		/// </param>
		/// <param name="workingDirectory">
		/// The working directory to use for the process.
		/// If the directory does not exist, it will be created.
		/// </param>
		/// <param name="waitForExit">
		/// Indicates whether the method will wait for the process to exit before returning.
		/// </param>
		/// <param name="outputDataReceived">
		/// The action to perform when stdout output is received from the running process.
		/// </param>
		/// <param name="errorDataReceived">
		/// The action to perform when stderr output is received from the running process.
		/// </param>
		/// <returns>
		/// Returns the <see cref="Diagnostics.Process"/> object created to run the process.
		/// </returns>
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
		/// <summary>
		/// Creates a new process using the specified parameters.
		/// </summary>
		/// <param name="exe">
		/// The executable file to run.
		/// </param>
		/// <param name="args">
		/// The command line arguments to pass to the process, as a single string.
		/// </param>
		/// <returns>
		/// Returns the <see cref="_Process"/> object created to run the process.
		/// </returns>
		public static _Process Create(string exe, string args)
		{
			return DoCreate(exe, args, Directory.GetCurrentDirectory(), Default_RedirectOutputAction, Default_RedirectErrorAction);
		}

		/// <summary>
		/// Creates a new process using the specified parameters.
		/// </summary>
		/// <param name="exe">
		/// The executable file to run.
		/// </param>
		/// <param name="args">
		/// The command line arguments to pass to the process, as a single string.
		/// </param>
		/// <param name="redirectOutputToLog">
		/// Indicates whether to redirect stdout output to the <see cref="System.Log.WriteLine(string)"/> method.
		/// </param>
		/// <returns>
		/// Returns the <see cref="_Process"/> object created to run the process.
		/// </returns>
		public static _Process Create(string exe, string args, bool redirectOutputToLog)
		{
			return DoCreate(exe, args, Directory.GetCurrentDirectory(), redirectOutputToLog, Default_RedirectErrorToLog);
		}

		/// <summary>
		/// Creates a new process using the specified parameters.
		/// </summary>
		/// <param name="exe">
		/// The executable file to run.
		/// </param>
		/// <param name="args">
		/// The command line arguments to pass to the process, as a single string.
		/// </param>
		/// <param name="redirectOutputToLog">
		/// Indicates whether to redirect stdout output to the <see cref="System.Log.WriteLine(string)"/> method.
		/// </param>
		/// <param name="redirectErrorToLog">
		/// Indicates whether to redirect stderr output to the <see cref="System.Log.WriteException(string)"/> method.
		/// </param>
		/// <returns>
		/// Returns the <see cref="_Process"/> object created to run the process.
		/// </returns>
		public static _Process Create(string exe, string args, bool redirectOutputToLog, bool redirectErrorToLog)
		{
			return DoCreate(exe, args, Directory.GetCurrentDirectory(), redirectOutputToLog, redirectErrorToLog);
		}

		/// <summary>
		/// Creates a new process using the specified parameters.
		/// </summary>
		/// <param name="exe">
		/// The executable file to run.
		/// </param>
		/// <param name="args">
		/// The command line arguments to pass to the process, as a single string.
		/// </param>
		/// <param name="outputDataReceived">
		/// The action to perform when stdout output is received from the running process.
		/// </param>
		/// <returns>
		/// Returns the <see cref="_Process"/> object created to run the process.
		/// </returns>
		public static _Process Create(string exe, string args, Action<string> outputDataReceived)
		{
			return DoCreate(exe, args, Directory.GetCurrentDirectory(), outputDataReceived, Default_RedirectErrorAction);
		}

		/// <summary>
		/// Creates a new process using the specified parameters.
		/// </summary>
		/// <param name="exe">
		/// The executable file to run.
		/// </param>
		/// <param name="args">
		/// The command line arguments to pass to the process, as a single string.
		/// </param>
		/// <param name="outputDataReceived">
		/// The action to perform when stdout output is received from the running process.
		/// </param>
		/// <param name="errorDataReceived">
		/// The action to perform when stderr output is received from the running process.
		/// </param>
		/// <returns>
		/// Returns the <see cref="_Process"/> object created to run the process.
		/// </returns>
		public static _Process Create(string exe, string args, Action<string> outputDataReceived, Action<string> errorDataReceived)
		{
			return DoCreate(exe, args, Directory.GetCurrentDirectory(), outputDataReceived, errorDataReceived);
		}

		/// <summary>
		/// Creates a new process using the specified parameters.
		/// </summary>
		/// <param name="exe">
		/// The executable file to run.
		/// </param>
		/// <param name="args">
		/// The command line arguments to pass to the process, as a single string.
		/// </param>
		/// <param name="workingDirectory">
		/// The working directory to use for the process.
		/// If the directory does not exist, it will be created.
		/// </param>
		/// <returns>
		/// Returns the <see cref="_Process"/> object created to run the process.
		/// </returns>
		public static _Process Create(string exe, string args, string workingDirectory)
		{
			return DoCreate(exe, args, GetWorkingDirectory(workingDirectory), Default_RedirectOutputAction, Default_RedirectErrorAction);
		}

		/// <summary>
		/// Creates a new process using the specified parameters.
		/// </summary>
		/// <param name="exe">
		/// The executable file to run.
		/// </param>
		/// <param name="args">
		/// The command line arguments to pass to the process, as a single string.
		/// </param>
		/// <param name="workingDirectory">
		/// The working directory to use for the process.
		/// If the directory does not exist, it will be created.
		/// </param>
		/// <param name="redirectOutputToLog">
		/// Indicates whether to redirect stdout output to the <see cref="System.Log.WriteLine(string)"/> method.
		/// </param>
		/// <returns>
		/// Returns the <see cref="_Process"/> object created to run the process.
		/// </returns>
		public static _Process Create(string exe, string args, string workingDirectory, bool redirectOutputToLog)
		{
			return DoCreate(exe, args, GetWorkingDirectory(workingDirectory), redirectOutputToLog, Default_RedirectErrorToLog);
		}

		/// <summary>
		/// Creates a new process using the specified parameters.
		/// </summary>
		/// <param name="exe">
		/// The executable file to run.
		/// </param>
		/// <param name="args">
		/// The command line arguments to pass to the process, as a single string.
		/// </param>
		/// <param name="workingDirectory">
		/// The working directory to use for the process.
		/// If the directory does not exist, it will be created.
		/// </param>
		/// <param name="redirectOutputToLog">
		/// Indicates whether to redirect stdout output to the <see cref="System.Log.WriteLine(string)"/> method.
		/// </param>
		/// <param name="redirectErrorToLog">
		/// Indicates whether to redirect stderr output to the <see cref="System.Log.WriteException(string)"/> method.
		/// </param>
		/// <returns>
		/// Returns the <see cref="_Process"/> object created to run the process.
		/// </returns>
		public static _Process Create(string exe, string args, string workingDirectory, bool redirectOutputToLog, bool redirectErrorToLog)
		{
			return DoCreate(exe, args, GetWorkingDirectory(workingDirectory), redirectOutputToLog, redirectErrorToLog);
		}

		/// <summary>
		/// Creates a new process using the specified parameters.
		/// </summary>
		/// <param name="exe">
		/// The executable file to run.
		/// </param>
		/// <param name="args">
		/// The command line arguments to pass to the process, as a single string.
		/// </param>
		/// <param name="workingDirectory">
		/// The working directory to use for the process.
		/// If the directory does not exist, it will be created.
		/// </param>
		/// <param name="outputDataReceived">
		/// The action to perform when stdout output is received from the running process.
		/// </param>
		/// <returns>
		/// Returns the <see cref="_Process"/> object created to run the process.
		/// </returns>
		public static _Process Create(string exe, string args, string workingDirectory, Action<string> outputDataReceived)
		{
			return DoCreate(exe, args, GetWorkingDirectory(workingDirectory), outputDataReceived, Default_RedirectErrorAction);
		}

		/// <summary>
		/// Creates a new process using the specified parameters.
		/// </summary>
		/// <param name="exe">
		/// The executable file to run.
		/// </param>
		/// <param name="args">
		/// The command line arguments to pass to the process, as a single string.
		/// </param>
		/// <param name="workingDirectory">
		/// The working directory to use for the process.
		/// If the directory does not exist, it will be created.
		/// </param>
		/// <param name="outputDataReceived">
		/// The action to perform when stdout output is received from the running process.
		/// </param>
		/// <param name="errorDataReceived">
		/// The action to perform when stderr output is received from the running process.
		/// </param>
		/// <returns>
		/// Returns the <see cref="_Process"/> object created to run the process.
		/// </returns>
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
		/// <summary>
		/// Runs the process associated with this instance.
		/// </summary>
		/// <returns>
		/// Returns the <see cref="Diagnostics.Process"/> object created to run the process.
		/// </returns>
		public Process Run()
		{
			return this.DoRun(Default_WaitForExit, false, false);
		}

		/// <summary>
		/// Runs the process associated with this instance.
		/// </summary>
		/// <param name="waitForExit">
		/// Indicates whether the method will wait for the process to exit before returning.
		/// </param>
		/// <returns>
		/// Returns the <see cref="Diagnostics.Process"/> object created to run the process.
		/// </returns>
		public Process Run(bool waitForExit)
		{
			return this.DoRun(waitForExit, false, false);
		}

		/// <summary>
		/// Runs the process associated with this instance.
		/// </summary>
		/// <param name="waitForExit">
		/// Indicates whether the method will wait for the process to exit before returning.
		/// </param>
		/// <param name="redirectOutput">
		/// Indicates whether to redirect stdout output.
		/// </param>
		/// <returns>
		/// Returns the <see cref="Diagnostics.Process"/> object created to run the process.
		/// </returns>
		public Process Run(bool waitForExit, bool redirectOutput)
		{
			return this.DoRun(waitForExit, redirectOutput, false);
		}

		/// <summary>
		/// Runs the process associated with this instance.
		/// </summary>
		/// <param name="waitForExit">
		/// Indicates whether the method will wait for the process to exit before returning.
		/// </param>
		/// <param name="redirectOutput">
		/// Indicates whether to redirect stdout output.
		/// </param>
		/// <param name="redirectError">
		/// Indicates whether to redirect stderr output.
		/// </param>
		/// <returns>
		/// Returns the <see cref="Diagnostics.Process"/> object created to run the process.
		/// </returns>
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
