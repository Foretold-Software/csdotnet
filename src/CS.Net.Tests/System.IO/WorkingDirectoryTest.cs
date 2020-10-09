// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.IO.Tests
{
	[TestClass]
	public partial class WorkingDirectoryTest
	{
		[TestMethod]
		public void SetWorkingDirectoryTest()
		{
			var originalWorkingDirectory = Directory.GetCurrentDirectory();
			var projectDirectory = _Path.Normalize(originalWorkingDirectory + @"\..\..");

			var paths = new List<string>();

			paths.Add(projectDirectory);
			paths.Add(projectDirectory + @"\bin");
			paths.Add(projectDirectory + @"\bin\Debug");

			var workingDirectories = new List<WorkingDirectory>();


			foreach (var path in paths)
			{
				workingDirectories.Add(new WorkingDirectory(path));
				Assert.IsTrue(PathComparer.IsEquivalent(path, Directory.GetCurrentDirectory()));
			}

			for (int i = workingDirectories.Count - 1; i >= 0; i--)
			{
				Assert.IsTrue(PathComparer.IsEquivalent(paths[i], Directory.GetCurrentDirectory()));
				workingDirectories[i].Dispose();
			}

			Assert.IsTrue(PathComparer.IsEquivalent(originalWorkingDirectory, Directory.GetCurrentDirectory()));
		}
	}
}
