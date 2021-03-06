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
			var paths = new List<string>();
			var workingDirectories = new List<WorkingDirectory>();
			var originalWorkingDirectory = Directory.GetCurrentDirectory();

			paths.Add(Path.Combine(originalWorkingDirectory, "dir1"));
			paths.Add(Path.Combine(originalWorkingDirectory, "dir1", "dir2"));
			paths.Add(Path.Combine(originalWorkingDirectory, "dir1", "dir2", "dir3a"));
			paths.Add(Path.Combine(originalWorkingDirectory, "dir1", "dir2", "dir3a", "dir4a"));
			paths.Add(Path.Combine(originalWorkingDirectory, "dir1", "dir2", "dir3b"));
			paths.Add(Path.Combine(originalWorkingDirectory, "dir1", "dir2", "dir3b", "dir4b"));
			paths.ForEach(path => Directory.CreateDirectory(path));

			//Push each path into the workingDirectories list, and set the current WorkingDirectory.
			foreach (var path in paths)
			{
				workingDirectories.Add(new WorkingDirectory(path));
				Assert.IsTrue(PathComparer.IsEquivalent(path, Directory.GetCurrentDirectory()));
			}

			//Pop each path from the workingDirectories list, and check that the current WorkingDirectory also pops.
			for (int i = workingDirectories.Count - 1; i >= 0; i--)
			{
				Assert.IsTrue(PathComparer.IsEquivalent(paths[i], Directory.GetCurrentDirectory()));
				workingDirectories[i].Dispose();
			}

			Assert.IsTrue(PathComparer.IsEquivalent(originalWorkingDirectory, Directory.GetCurrentDirectory()));

			try
			{
				Directory.Delete(paths[0], true);
			}
			catch
			{
				Assert.Fail("Failed to delete test directory: " + paths[0]);
			}
		}
	}
}
