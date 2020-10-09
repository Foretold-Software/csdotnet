// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace CS.Net.Sample
{
	class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			//_Debugger.Break();

			TestPath();

			TestLogging();

			TestWPFUI();

			Console.WriteLine("Press any key to continue...");
			Console.ReadKey();
		}

		#region Test Path
		static void TestPath()
		{
			TestPath_GetDirectoryName();

			TestPathNormalize();
		}

		static void TestPath_GetDirectoryName()
		{
			var path = @"C:\some\path\to\some\other\path\and\this\one\other\path\also";

			path = Path.GetDirectoryName(path);
			path = Path.GetDirectoryName(path);
			path = Path.GetDirectoryName(path);
			path = Path.GetDirectoryName(path);
			path = Path.GetDirectoryName(path);
			path = Path.GetDirectoryName(path);

		}

		static void TestPathNormalize()
		{
			var root1 = Path.GetPathRoot("C:\\folder1\\folder2");
			var root2 = Path.GetPathRoot("C:\\");
			var root3 = Path.GetPathRoot("C:");
			var root4 = Path.GetPathRoot("C:folder1");
			//var path1 = Path.GetFullPath("\\");

			_Path.Normalize("C:\\.");
		}
		#endregion

		#region WPF UI
		static void TestWPFUI()
		{
			TestDraggableWindow();
		}

		static void TestDraggableWindow()
		{
			new Views.MainWindow().ShowAndWait();
		}
		#endregion

		#region Logging
		static void TestLogging()
		{
			TestLoggingToFile();
			TestLoggingToConsole();
		}

		static void TestLoggingToConsole()
		{
			TestLogging(Console.WriteLine);
		}

		static void TestLoggingToFile()
		{
			try
			{
				using (var file = new StreamWriter("logFile.log"))
				{
					TestLogging(file.WriteLine);
				}
			}
			catch (Exception exc)
			{
				Console.WriteLine("An error occurred: " + exc.GetType());
				Console.WriteLine(exc.Message);
			}
		}

		static void TestLogging(LogCallback logCallback)
		{
			Log.Initialize(logCallback, true);

			try
			{
				Log.WriteLine("Hello World!!");
				Log.WriteLine("This is the next line.");
				Log.WriteLine("And one more line after that.");
				Log.WriteLine("Empty string:");
				Log.WriteLine("");
				Log.WriteLine("Null:");
				Log.WriteLine(null);

				throw new Exception("Ouch!! That exception hurt, stop throwing them at me!!!");
			}
			catch (Exception exc)
			{
				//Log and continue...
				Log.WriteException("An error occurred while testing the Log class.");
				Log.WriteException(exc);
			}
		}
		#endregion
	}
}
