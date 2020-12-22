// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.IO;

namespace System.Diagnostics.Tests
{
	[TestClass]
	public class ProcessTests : TestWithLogOutput
	{
		[TestMethod]
		public void RunTest()
		{
			Process process;
			bool disposed = false;

			using (process = _Process.Run(WaitCommandExe, WaitCommandArgs))
			{
				process.Disposed += (s, e) => disposed = true;
			}

			Assert.IsTrue(disposed);
		}

		[TestMethod]
		public void CreateTest()
		{
			_Process _process;

			using (_process = _Process.Create(WaitCommandExe, WaitCommandArgs))
			{
				Assert.IsFalse(_process.IsDisposed);

				_process.Run();
			}

			Assert.IsTrue(_process.IsDisposed);
		}

		[DataRow]
		[DataRow(2)]
		[TestProperty("log", "true")]
		[DataTestMethod]
		public void OutputTest(int waitTime = 0)
		{
			var outputFile = LogOutputFile;

			//The file should not exist. If it does, we cannot be sure that its text content was output from this test.
			Assert.That.FileExists(outputFile, false, true);

			_Process.Run(WaitCommandExe, GetWaitCommandArgs(waitTime), true, Log.WriteLine, Log.WriteException).Dispose();

			//Ensure the output file exists.
			Assert.That.FileExists(outputFile, true);

			//Check the output file for the expected text.
			CheckOutputFileContents(outputFile, waitTime + 1);
		}

		[DataRow]
		[DataRow(2)]
		[TestProperty("log", "true")]
		[DataTestMethod]
		public void OutputToLogTest(int waitTime = 0)
		{
			var outputFile = LogOutputFile;

			//The file should not exist. If it does, we cannot be sure that its text content was output from this test.
			Assert.That.FileExists(outputFile, false, true);

			_Process.Run(WaitCommandExe, GetWaitCommandArgs(waitTime), true, true).Dispose();

			//Ensure the output file exists.
			Assert.That.FileExists(outputFile, true);

			//Check the output file for the expected text.
			CheckOutputFileContents(outputFile, waitTime + 1);
		}

		[DataRow]
		[DataRow(2)]
		[DataTestMethod]
		public void DisposeTest(int waitTime = 0)
		{
			Process process;
			_Process _process;


			//Create the process object
			_process = _Process.Create(WaitCommandExe, GetWaitCommandArgs(waitTime));
			Assert.IsFalse(_process.IsDisposed);


			//Run the process
			process = _process.Run();
			Assert.IsFalse(_process.IsDisposed);

			process = _process.Run(false);
			Assert.IsFalse(_process.IsDisposed);

			process = _process.Run();
			Assert.IsFalse(_process.IsDisposed);


			//Dispose the process
			_process.Dispose();
			Assert.IsTrue(_process.IsDisposed);

			Threading.Thread.Sleep(1000 * (waitTime + 1));
			Assert.IsTrue(_process.IsDisposed);


			//Ensure subsequent runs throw an exception
			Assert.ThrowsException<ObjectDisposedException>(_process.Run);
			Assert.IsTrue(_process.IsDisposed);
		}

		[DataRow]
		[DataRow(2)]
		[DataTestMethod]
		public void DisposeIndirectlyTest(int waitTime = 0)
		{
			Process process;
			_Process _process;


			//Create the process object
			_process = _Process.Create(WaitCommandExe, GetWaitCommandArgs(waitTime));
			Assert.IsFalse(_process.IsDisposed);


			//Run the process
			process = _process.Run();
			Assert.IsFalse(_process.IsDisposed);

			process = _process.Run(false);
			Assert.IsFalse(_process.IsDisposed);

			process = _process.Run();
			Assert.IsFalse(_process.IsDisposed);


			//Dispose the process
			process.Dispose();
			Assert.IsTrue(_process.IsDisposed);

			Threading.Thread.Sleep(1000 * (waitTime + 1));
			Assert.IsTrue(_process.IsDisposed);


			//Ensure subsequent runs throw an exception
			Assert.ThrowsException<ObjectDisposedException>(_process.Run);
			Assert.IsTrue(_process.IsDisposed);
		}

		#region Helpers
		private string WaitCommandExe
		{ get => "ping"; }

		private string WaitCommandArgs
		{ get => GetWaitCommandArgs(); }

		private string GetWaitCommandArgs(int secondsToWait = 0)
		{
			return string.Format("127.0.0.1 -n {0}", secondsToWait + 1);
		}

		private void CheckOutputFileContents(string outputFile, int pingCount)
		{
			var allLines = File.ReadAllLines(outputFile);

			Assert.IsTrue(allLines.Any(line => line.Contains("Pinging")));
			Assert.IsTrue(allLines.Any(line => line.Contains("Ping statistics for")));
			Assert.IsTrue(allLines.Any(line => line.Contains("Approximate round trip")));
			Assert.IsTrue(allLines.Count(line => line.Contains("Reply from")) == pingCount);
		}
		#endregion
	}
}
