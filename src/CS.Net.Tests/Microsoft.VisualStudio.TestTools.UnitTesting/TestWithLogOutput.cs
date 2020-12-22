// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System;
using System.IO;

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
	/// <summary>
	/// A base class for unit test classes containing unit tests which require output to a log file.
	/// </summary>
	/// <remarks>
	/// The particular unit test methods which require output must use the <see cref="TestPropertyAttribute"/>
	/// with Name set to "log" and Value set to "true".
	/// </remarks>
	[TestClass]
	public class TestWithLogOutput
	{
		/// <summary>
		/// Used to store information that is passed to unit tests.
		/// </summary>
		public TestContext TestContext { get; set; }

		/// <summary>
		/// Indicates whether the current unit test requires log file output.
		/// </summary>
		protected bool LogOutputIndicated
		{
			get
			{
				//The logOutput variable will be assigned a 'true' value only if a
				// bool string can be parsed AND it is a 'true' value. Otherwise, it
				// will receive a 'false' value.
				bool.TryParse(TestContext.Properties["log"] as string, out var logOutput);

				return logOutput;
			}
		}

		/// <summary>
		/// Gets a path to a log file specific to the currently running unit test.
		/// </summary>
		protected string LogOutputFile
		{
			get
			{
				return Path.Combine(
					"FileOutput",
					TestContext.FullyQualifiedTestClassName + "-" + TestContext.TestName + "-out.txt");
			}
		}

		/// <summary>
		/// Used to initialize each unit test.
		/// </summary>
		[TestInitialize]
		public void TestInitialize()
		{
			if (LogOutputIndicated)
			{
				var outputFile = LogOutputFile;

				Directory.CreateDirectory(Directory.GetParent(outputFile).FullName);

				Log.Initialize(s =>
				{
					lock (this)
					{
						File.AppendAllLines(outputFile, new string[] { s });
					}
				});
			}

		}

		/// <summary>
		/// Used to clean up each unit test.
		/// </summary>
		[TestCleanup]
		public void TestCleanup()
		{
			if (LogOutputIndicated)
			{
				File.Delete(LogOutputFile);

				Log.LogCallback = null;
			}
		}
	}
}
