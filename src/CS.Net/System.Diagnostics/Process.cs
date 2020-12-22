// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.IO;

namespace System.Diagnostics
{
	/// <summary>
	/// Helper class for running external processes.
	/// </summary>
	public class _Process : Disposable
	{
		#region Constructor
		private _Process()
		{
			Process = new Process();
			Process.StartInfo.UseShellExecute = false;

			IsDisposed = false;
			Process.Disposed += ProcessDisposed;
		}
		#endregion

		#region Fields
		private readonly Process Process;

		private bool _WaitForExit = Default_WaitForExit;
		private Action<string> _RedirectOutputAction = Default_RedirectOutputAction;
		private Action<string> _RedirectErrorAction = Default_RedirectErrorAction;

		//Multiple run / multiple thread safety
		private volatile bool IsDisposing = false;
		private bool RedirectOutputStreamStarted = false;
		private bool RedirectErrorStreamStarted = false;

		//Default behaviors
		private const bool Default_WaitForExit = true;
		private const bool Default_RedirectOutputToLog = false;
		private const bool Default_RedirectErrorToLog = false;
		private const Action<string> Default_RedirectOutputAction = null;
		private const Action<string> Default_RedirectErrorAction = null;
		#endregion

		#region Properties
		/// <summary>
		/// The executable file to run.
		/// </summary>
		public string Executable
		{
			get { return Process.StartInfo.FileName; }
			set { Process.StartInfo.FileName = value; }
		}

		/// <summary>
		/// The command line arguments to pass to the process, as a single string.
		/// </summary>
		public string Arguments
		{
			get { return Process.StartInfo.Arguments; }
			set { Process.StartInfo.Arguments = value; }
		}

		/// <summary>
		/// The working directory from which to run the process.
		/// If the directory does not exist, it will be created.
		/// If the value is null or an invalid directory path, the return
		/// value of <see cref="Directory.GetCurrentDirectory"/> will be used.
		/// </summary>
		public string WorkingDirectory
		{ get; set; }

		/// <summary>
		/// Indicates whether to wait for the process to exit before returning when the process is run.
		/// </summary>
		public bool WaitForExit
		{
			get { return _WaitForExit; }
			set { _WaitForExit = value; }
		}

		/// <summary>
		/// The action to perform when stdout output is received from the running process.
		/// </summary>
		public Action<string> RedirectOutputAction
		{
			get { return _RedirectOutputAction; }
			set
			{
				//Assign the action and the value indicating whether to redirect output.
				Process.StartInfo.RedirectStandardOutput = null != (_RedirectOutputAction = value);

				//Remove the delegate if it was already assigned.
				Process.OutputDataReceived -= OutputDataReceived;

				//If redirecting output, then reassign the delegate.
				if (Process.StartInfo.RedirectStandardOutput)
				{
					Process.OutputDataReceived += OutputDataReceived;
				}
			}
		}

		/// <summary>
		/// The action to perform when stderr output is received from the running process.
		/// </summary>
		public Action<string> RedirectErrorAction
		{
			get { return _RedirectErrorAction; }
			//set { Process.StartInfo.RedirectStandardError = null != (_RedirectErrorAction = value); }
			set
			{
				//Assign the action and the value indicating whether to redirect output.
				Process.StartInfo.RedirectStandardError = null != (_RedirectErrorAction = value);

				//Remove the delegate if it was already assigned.
				Process.ErrorDataReceived -= ErrorDataReceived;

				//If redirecting output, then reassign the delegate.
				if (Process.StartInfo.RedirectStandardError)
				{
					Process.ErrorDataReceived += ErrorDataReceived;
				}
			}
		}

		/// <summary>
		/// Indicates whether the underlying <see cref="Diagnostics.Process"/> object has been disposed.
		/// </summary>
		public bool IsDisposed
		{ get; private set; }
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
			return DoCreate(exe, args, workingDirectory, waitForExit, outputDataReceived, errorDataReceived).Run();
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
			return DoCreate(exe, args, workingDirectory, Default_WaitForExit, outputDataReceived, errorDataReceived);
		}

		private static _Process DoCreate(string exe, string args, string workingDirectory, bool waitForExit, Action<string> outputDataReceived, Action<string> errorDataReceived)
		{
			try
			{
				return new _Process()
				{
					Executable = exe,
					Arguments = args,
					WorkingDirectory = workingDirectory,
					WaitForExit = waitForExit,
					RedirectOutputAction = outputDataReceived,
					RedirectErrorAction = errorDataReceived
				};
			}
			catch (Exception exc)
			{
				throw new Exception(string.Format("Failed to create process: '{0}' '{1}'", exe, args), exc);
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
			return DoRun(
				this,
				WaitForExit,
				!RedirectOutputStreamStarted && Process.StartInfo.RedirectStandardOutput && RedirectOutputAction != null,
				!RedirectErrorStreamStarted && Process.StartInfo.RedirectStandardError && RedirectErrorAction != null);
		}

		/// <summary>
		/// Runs the process associated with this instance.
		/// </summary>
		/// <param name="waitForExit">
		/// Indicates whether the method will wait for the process to exit before returning.
		/// This value overrides the <see cref="WaitForExit"/> property.
		/// </param>
		/// <returns>
		/// Returns the <see cref="Diagnostics.Process"/> object created to run the process.
		/// </returns>
		public Process Run(bool waitForExit)
		{
			return DoRun(
				this,
				waitForExit,
				!RedirectOutputStreamStarted && Process.StartInfo.RedirectStandardOutput && RedirectOutputAction != null,
				!RedirectErrorStreamStarted && Process.StartInfo.RedirectStandardError && RedirectErrorAction != null);
		}

		/// <summary>
		/// Runs the process associated with this instance.
		/// </summary>
		/// <param name="waitForExit">
		/// Indicates whether the method will wait for the process to exit before returning.
		/// This value overrides the <see cref="WaitForExit"/> property.
		/// </param>
		/// <param name="redirectOutput">
		/// Indicates whether to redirect stdout output.
		/// If false, this value overrides the <see cref="RedirectOutputAction"/> property.
		/// </param>
		/// <returns>
		/// Returns the <see cref="Diagnostics.Process"/> object created to run the process.
		/// </returns>
		public Process Run(bool waitForExit, bool redirectOutput)
		{
			return DoRun(
				this,
				waitForExit,
				!RedirectOutputStreamStarted && Process.StartInfo.RedirectStandardOutput && RedirectOutputAction != null && redirectOutput,
				!RedirectErrorStreamStarted && Process.StartInfo.RedirectStandardError && RedirectErrorAction != null);
		}

		/// <summary>
		/// Runs the process associated with this instance.
		/// </summary>
		/// <param name="waitForExit">
		/// Indicates whether the method will wait for the process to exit before returning.
		/// This value overrides the <see cref="WaitForExit"/> property.
		/// </param>
		/// <param name="redirectOutput">
		/// Indicates whether to redirect stdout output.
		/// If false, this value overrides the <see cref="RedirectOutputAction"/> property.
		/// </param>
		/// <param name="redirectError">
		/// Indicates whether to redirect stderr output.
		/// If false, this value overrides the <see cref="RedirectErrorAction"/> property.
		/// </param>
		/// <returns>
		/// Returns the <see cref="Diagnostics.Process"/> object created to run the process.
		/// </returns>
		public Process Run(bool waitForExit, bool redirectOutput, bool redirectError)
		{
			return DoRun(
				this,
				waitForExit,
				!RedirectOutputStreamStarted && Process.StartInfo.RedirectStandardOutput && RedirectOutputAction != null && redirectOutput,
				!RedirectErrorStreamStarted && Process.StartInfo.RedirectStandardError && RedirectErrorAction != null && redirectError);
		}

		private static Process DoRun(_Process process, bool waitForExit, bool beginOutputRedirect, bool beginErrorRedirect)
		{
			lock (process)
			{
				try
				{
					process.Process.Start();

					if (beginOutputRedirect)
					{
						process.Process.BeginOutputReadLine();
						process.RedirectOutputStreamStarted = true;
					}
					if (beginErrorRedirect)
					{
						process.Process.BeginErrorReadLine();
						process.RedirectErrorStreamStarted = true;
					}

					if (waitForExit)
					{
						process.Process.WaitForExit();
					}

					return process.Process;
				}
				catch (ObjectDisposedException exc)
				{
					throw new ObjectDisposedException(
						string.Format("{1}{0}Executable: {2}{0}Args: {3}",
							Environment.NewLine,
							exc.Message,
							process.Executable,
							process.Arguments),
						exc);
				}
				catch (Exception exc)
				{
					throw new Exception(string.Format("Process failed: '{0}' '{1}'", process.Executable, process.Arguments), exc);
				}
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

		/// <summary>
		/// Handles the <see cref="Process.OutputDataReceived"/> event.
		/// </summary>
		/// <param name="sender">
		/// The source of the event.
		/// </param>
		/// <param name="e">
		/// An instance of <see cref="DataReceivedEventArgs"/> containing the event data.
		/// </param>
		private void OutputDataReceived(object sender, DataReceivedEventArgs e)
		{
			RedirectOutputAction?.Invoke(e.Data);
		}

		/// <summary>
		/// Handles the <see cref="Process.ErrorDataReceived"/> event.
		/// </summary>
		/// <param name="sender">
		/// The source of the event.
		/// </param>
		/// <param name="e">
		/// An instance of <see cref="DataReceivedEventArgs"/> containing the event data.
		/// </param>
		private void ErrorDataReceived(object sender, DataReceivedEventArgs e)
		{
			RedirectErrorAction?.Invoke(e.Data);
		}

		/// <summary>
		/// Handles the process disposal event.
		/// </summary>
		/// <param name="sender">
		/// The source of the event.
		/// </param>
		/// <param name="e">
		/// An instance of <see cref="EventArgs"/> containing the event data.
		/// </param>
		private void ProcessDisposed(object sender, EventArgs e)
		{
			lock (this)
			{
				IsDisposing = true;
				IsDisposed = true;
			}
		}
		#endregion

		#region Methods - Disposable Overrides
		/// <summary>
		/// Frees resources used by this class's underlying <see cref="Diagnostics.Process"/> object.
		/// </summary>
		protected override void FreeManagedResources()
		{
			lock (this)
			{
				if (!IsDisposing)
				{
					IsDisposing = true;

					if (!IsDisposed)
					{
						if (Process.HasExited)
						{
							Process.Dispose();
						}
						else
						{
							new Threading.Thread(() =>
							{
								Process.WaitForExit();
								Process.Dispose();
							}).Start();
						}
					}
				}
			}
		}
		#endregion
	}
}
