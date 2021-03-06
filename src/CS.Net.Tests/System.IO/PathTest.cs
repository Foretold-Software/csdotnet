// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.IO.Tests
{
	[TestClass]
	public partial class PathTest
	{
		[TestMethod]
		public void GetPathRootTest()
		{
			string expected = Path.GetPathRoot(Path.GetFullPath(Directory.GetCurrentDirectory()));
			string actual = _Path.GetPathRoot();

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetCommonDirectoryTest()
		{
			string path1, path2, expected, actual;

			path1 = @"C:\common\path\specific1\path1";
			path2 = @"C:\common\path\specific2\path2";
			expected = @"C:\common\path";
			actual = _Path.GetCommonDirectory(path1, path2);
			Assert.IsTrue(PathComparer.IsEquivalent(expected, actual));

			path1 = @"C:\common\path\specific1";
			path2 = @"C:\common\path\specific2";
			expected = @"C:\common\path";
			actual = _Path.GetCommonDirectory(path1, path2);
			Assert.IsTrue(PathComparer.IsEquivalent(expected, actual));

			path1 = @"C:\common\path\specific";
			path2 = @"C:\common\path\specific";
			expected = @"C:\common\path";
			actual = _Path.GetCommonDirectory(path1, path2);
			Assert.IsTrue(PathComparer.IsEquivalent(expected, actual));

			path1 = @"C:\no\common\path";
			path2 = @"C:\cuz\im\different";
			expected = @"C:\";
			actual = _Path.GetCommonDirectory(path1, path2);
			Assert.IsTrue(PathComparer.IsEquivalent(expected, actual));

			path1 = @"C:\no\common\path\specific";
			path2 = @"D:\common\path\specific";
			actual = _Path.GetCommonDirectory(path1, path2);
			Assert.IsNull(actual);
		}

		[TestMethod]
		public void NormalizeTest_Null()
		{
			string expected = null;
			string actual = _Path.Normalize(null);

			Assert.AreEqual(expected, actual, "The null path did not normalize to null.");
		}

		[TestMethod]
		public void NormalizeTest()
		{
			string actual;
			string expected;
			string inputPath;

			//Store the previous current directory, to restore it when finished.
			string previousCurrentDirectory = Directory.GetCurrentDirectory();

			//Get a path like: "C:\TempFolder-{SOME-GUID-VALUE}"
			string tempFolder = Path.Combine(_Path.GetPathRoot(), "TempFolder-" + Guid.NewGuid().ToString("N").ToUpper());

			try
			{
				//Get the paths to test.
				var paths = GetNormalizeTestPaths(tempFolder, true);

				foreach (var kvp in paths)
				{
					inputPath = kvp.Key;
					expected = kvp.Value;
					actual = _Path.Normalize(inputPath);

					Assert.AreEqual(expected, actual, "\nThe path was not correctly normalized.\n-Input: '{0}'\n-Expected: '{1}'\n-Actual: '{2}'", inputPath, expected, actual);
				}
			}
			finally
			{
				Directory.SetCurrentDirectory(previousCurrentDirectory);

				if (Directory.Exists(tempFolder))
				{
					Directory.Delete(tempFolder, true);
				}
			}
		}

		//TODO: This.
		/*
add invalid characters to the test paths
add C:somefolder to the test paths
A folder node like ".. .." would probably throw an exception. Allow this?

Add a path: "C:\my\path"
Add a path: "5:\my\path"

Add a path: "C:\path\to\a\realFile.txt"
		 */

		private Dictionary<string, string> GetNormalizeTestPaths(string tempFolder, bool createDirectories)
		{
			string tempFolderName = Path.GetFileName(tempFolder);

			string realFolder1a = Path.Combine(tempFolder, "RealFolder_1a");
			string realFolder1b = Path.Combine(tempFolder, "RealFolder_1b");
			string realFolder2 = Path.Combine(realFolder1a, "RealFolder_2");
			string realFolder3 = Path.Combine(realFolder2, "RealFolder_3");

			string fakeFolder1 = Path.Combine(tempFolder, "FakeFolder_1");
			string fakeFolder2 = Path.Combine(fakeFolder1, "FakeFolder_2");
			string fakeFolder3 = Path.Combine(fakeFolder2, "FakeFolder_3");


			string realDrive = Path.GetPathRoot(tempFolder);
			string fakeDrive = GetNonExistentDrive();

			if (fakeDrive == null)
			{
				Assert.Inconclusive("Cannot complete NormalizeTest on a system without an available drive letter.");
			}


			Directory.CreateDirectory(tempFolder);
			Directory.CreateDirectory(realFolder1a);
			Directory.CreateDirectory(realFolder1b);
			Directory.CreateDirectory(realFolder2);
			Directory.CreateDirectory(realFolder3);

			Directory.SetCurrentDirectory(tempFolder);
			string currentDirectory = Directory.GetCurrentDirectory();


			//Key = test input
			//Value = expected output
			var paths = new Dictionary<string, string>
			{
				{ realDrive,                              realDrive },
				{ realDrive.ToLower(),                    realDrive },
				{ realDrive.Remove(realDrive.Length - 1), currentDirectory },

				{ fakeDrive,                              fakeDrive },
				{ fakeDrive.ToLower(),                    fakeDrive },
				{ fakeDrive.Remove(fakeDrive.Length - 1), fakeDrive + currentDirectory.Substring(fakeDrive.Length) },

				{ "\\",         realDrive },
				{ "\\\\",       realDrive },
				{ "\\\\\\\\\\", realDrive },

				{ realDrive + ".",                          realDrive },
				{ realDrive + ".\\" + tempFolderName,       tempFolder },
				{ realDrive + ".\\.\\.\\" + tempFolderName, tempFolder },

				{ fakeDrive + ".",                          fakeDrive },
				{ fakeDrive + ".\\" + tempFolderName,       Path.Combine(fakeDrive + tempFolderName) },
				{ fakeDrive + ".\\.\\.\\" + tempFolderName, Path.Combine(fakeDrive + tempFolderName) },

				{ realDrive + "..",                                        realDrive },
				{ realDrive + "..\\",                                      realDrive },
				{ realDrive + "\\..",                                      realDrive },
				{ realDrive + "\\..\\",                                    realDrive },
				{ realDrive + "\\..\\..",                                  realDrive },
				{ realDrive + "\\..\\..\\..",                              realDrive },
				{ realDrive + "\\..\\..\\..\\",                            realDrive },
				{ realDrive + "\\..\\..\\..\\" + tempFolderName,           tempFolder },
				{ realDrive + "..\\" + tempFolderName + "\\FakeFolder_1",  fakeFolder1 },
				{ realDrive + "..\\" + tempFolderName + "\\RealFolder_1a", realFolder1a },
				{ realDrive + "..\\" + tempFolderName + "\\REALFOLDER_1A", realFolder1a },

				{ fakeDrive + "..",                              fakeDrive },
				{ fakeDrive + "..\\",                            fakeDrive },
				{ fakeDrive + "\\..",                            fakeDrive },
				{ fakeDrive + "\\..\\",                          fakeDrive },
				{ fakeDrive + "\\..\\..",                        fakeDrive },
				{ fakeDrive + "\\..\\..\\..",                    fakeDrive },
				{ fakeDrive + "\\..\\..\\..\\",                  fakeDrive },
				{ fakeDrive + "\\..\\..\\..\\" + tempFolderName, fakeDrive + tempFolderName },

				{ tempFolder,                              tempFolder },
				{ tempFolder + "\\FakeFolder_1",           fakeFolder1 },
				{ tempFolder + "\\\\\\FakeFolder_1",       fakeFolder1 },
				{ tempFolder + "\\FakeFolder_1\\",         fakeFolder1 },
				{ tempFolder + "\\\\\\FakeFolder_1\\\\\\", fakeFolder1 },

				{ tempFolder.ToUpper(),                              tempFolder },
				{ tempFolder.ToUpper() + "\\FakeFolder_1",           fakeFolder1 },
				{ tempFolder.ToUpper() + "\\\\\\FakeFolder_1",       fakeFolder1 },
				{ tempFolder.ToUpper() + "\\FakeFolder_1\\",         fakeFolder1 },
				{ tempFolder.ToUpper() + "\\\\\\FakeFolder_1\\\\\\", fakeFolder1 },

				{ realFolder1a + "\\..\\FakeFolder_1",  fakeFolder1 },
				{ realFolder1a + "\\..\\RealFolder_1a", realFolder1a },
				{ realFolder1a + "\\..\\RealFolder_1b", realFolder1b },

				{ fakeFolder1 + "\\..\\FakeFolder_1",   fakeFolder1 },
				{ fakeFolder1 + "\\..\\RealFolder_1a",  realFolder1a },
				{ fakeFolder1 + "\\..\\RealFolder_1b",  realFolder1b },

				{ fakeFolder1.ToUpper() + "\\..\\FakeFolder_1",   fakeFolder1 },
				{ fakeFolder1.ToUpper() + "\\..\\REALFOLDER_1A",  realFolder1a },
				{ fakeFolder1.ToUpper() + "\\..\\REALFOLDER_1B",  realFolder1b },
				{ realFolder1a.ToUpper() + "\\..\\FakeFolder_1",  fakeFolder1 },
				{ realFolder1a.ToUpper() + "\\..\\REALFOLDER_1A", realFolder1a },
				{ realFolder1a.ToUpper() + "\\..\\REALFOLDER_1B", realFolder1b },

				{ realDrive + "FakeFolder_1",                           Path.Combine(realDrive, "FakeFolder_1") },
				{ realDrive + "FakeFolder_1\\",                         Path.Combine(realDrive, "FakeFolder_1") },
				{ realDrive + "\\\\\\FakeFolder_1\\\\\\FakeFolder_2\\", Path.Combine(realDrive, "FakeFolder_1", "FakeFolder_2") },

				{ fakeDrive + "FakeFolder_1",                           Path.Combine(fakeDrive, "FakeFolder_1") },
				{ fakeDrive + "FakeFolder_1\\",                         Path.Combine(fakeDrive, "FakeFolder_1") },
				{ fakeDrive + "\\\\\\FakeFolder_1\\\\\\FakeFolder_2\\", Path.Combine(fakeDrive, "FakeFolder_1", "FakeFolder_2") },

				{ tempFolder + "\\ RealFolder_1a \t \\ \t ",          realFolder1a },
				{ tempFolder + "\\ RealFolder_1a \t \\ RealFolder_2", realFolder2 },
				{ tempFolder + "\\ FakeFolder_1 \t \\ FakeFolder_2",  fakeFolder2 },

				{ ".",                      currentDirectory },
				{ ".\\FakeFolder_1",        fakeFolder1 },
				{ ".\\\\\\FakeFolder_1",    fakeFolder1 },
				{ ".\\.\\.\\FakeFolder_1",  fakeFolder1 },
				{ "FakeFolder_1",           fakeFolder1 },
				{ "FakeFolder_1\\",         fakeFolder1 },
				{ "FakeFolder_1\\\\\\",     fakeFolder1 },
				{ "\\FakeFolder_1",         realDrive + "FakeFolder_1" },
				{ "\\\\\\FakeFolder_1",     realDrive + "FakeFolder_1" },
				{ "\\.\\.\\FakeFolder_1",   realDrive + "FakeFolder_1" },

				{ "",           currentDirectory },
				{ " ",          currentDirectory },
				{ "  ",         currentDirectory },
				{ "     ",      currentDirectory },

				{ "\t",         currentDirectory },
				{ "\t\t",       currentDirectory },
				{ "\t\t\t\t\t", currentDirectory },

				{ " \t ",                   currentDirectory },
				{ "  \t  ",                 currentDirectory },
				{ "     \t     ",           currentDirectory },
				{ " \t\t ",                 currentDirectory },
				{ "  \t\t  ",               currentDirectory },
				{ "     \t\t     ",         currentDirectory },
				{ " \t\t\t\t\t ",           currentDirectory },
				{ "  \t\t\t\t\t  ",         currentDirectory },
				{ "     \t\t\t\t\t     ",   currentDirectory },

				{ "\t \t",                      currentDirectory },
				{ "\t\t \t\t",                  currentDirectory },
				{ "\t\t\t\t\t \t\t\t\t\t",      currentDirectory },
				{ "\t  \t",                     currentDirectory },
				{ "\t\t  \t\t",                 currentDirectory },
				{ "\t\t\t\t\t  \t\t\t\t\t",     currentDirectory },
				{ "\t     \t",                  currentDirectory },
				{ "\t\t     \t\t",              currentDirectory },
				{ "\t\t\t\t\t     \t\t\t\t\t",  currentDirectory },

				{ " \t",                currentDirectory },
				{ "  \t",               currentDirectory },
				{ "     \t",            currentDirectory },
				{ " \t\t",              currentDirectory },
				{ "  \t\t",             currentDirectory },
				{ "     \t\t",          currentDirectory },
				{ " \t\t\t\t\t",        currentDirectory },
				{ "  \t\t\t\t\t",       currentDirectory },
				{ "     \t\t\t\t\t",    currentDirectory },

				{ "\t ",                currentDirectory },
				{ "\t\t ",              currentDirectory },
				{ "\t\t\t\t\t ",        currentDirectory },
				{ "\t  ",               currentDirectory },
				{ "\t\t  ",             currentDirectory },
				{ "\t\t\t\t\t  ",       currentDirectory },
				{ "\t     ",            currentDirectory },
				{ "\t\t     ",          currentDirectory },
				{ "\t\t\t\t\t     ",    currentDirectory }
			};

			return paths;
		}

		private string GetNonExistentDrive()
		{
			var existingDrives = Environment.GetLogicalDrives().Select(drive => drive[0]).ToList();

			for (char driveLetter = 'Z'; driveLetter >= 'A'; driveLetter--)
			{
				if (!existingDrives.Contains(driveLetter))
				{
					return driveLetter.ToString() + ":\\";
				}
			}

			return null;
		}
	}
}
