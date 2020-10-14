// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace System.IO
{
	public static class _Path
	{
		#region Fields
		public static readonly string DirectorySeparatorString = Path.DirectorySeparatorChar.ToString();
		#endregion

		#region Methods
		/// <summary>
		/// Returns distinct elements from a sequence by using the <see cref="PathComparer"/> equality comparer to compare values.
		/// </summary>
		/// <param name="paths">The sequence to remove duplicate elements from.</param>
		/// <returns>An <see cref="IEnumerable{T}"/> that contains distinct elements from the source sequence.</returns>
		public static IEnumerable<string> DistinctPaths(this IEnumerable<string> paths)
		{
			return paths.Distinct(new PathComparer());
		}

		/// <summary>
		/// Gets the root directory information for the current directory.
		/// </summary>
		/// <returns>
		/// A string containing the root directory information for
		/// the current directory, such as "C:\"
		/// </returns>
		public static string GetPathRoot()
		{
			return Path.GetPathRoot(Path.GetFullPath(Directory.GetCurrentDirectory()));
		}

		/// <summary>
		/// Combines an array of strings into a path.
		/// </summary>
		/// <param name="paths">An array of parts of the path.</param>
		/// <returns>The combined paths.</returns>
		/// <exception cref="ArgumentException">
		/// One of the strings in the array contains one or more of the
		/// invalid characters defined in <see cref="Path.GetInvalidPathChars"/>.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// One of the strings in the array is null.
		/// </exception>
		public static string Combine(params string[] paths)
		{
#if NET35
			var path = string.Empty;

			for (int i = 0; i < paths.Length; i++)
			{
				if (path.IsBlank())
				{
					path = paths[i];
				}
				else
				{
					path = Path.Combine(path, paths[i]);
				}
			}

			return path;
#else
			return Path.Combine(paths);
#endif
		}

		/// <summary>
		/// Gets the path to windir from the system environment variable.
		/// </summary>
		/// <returns>The path to windir.</returns>
		public static string GetWindowsDirectory()
		{
#if NET35
			// .Net 3.5
			return Environment.GetEnvironmentVariable("windir", EnvironmentVariableTarget.Machine);
#else
			// .Net 4+
			return Environment.GetFolderPath(Environment.SpecialFolder.Windows);
#endif
		}

		/// <summary>
		/// Finds the lowest common folder path shared between the two given paths.
		/// </summary>
		/// <returns>Returns the lowest common folder path, or null if none is found.</returns>
		public static string GetCommonDirectory(string path, string otherPath)
		{
			path = Path.GetDirectoryName(_Path.Normalize(path));
			otherPath = Path.GetDirectoryName(_Path.Normalize(otherPath));

			if (path == null || otherPath == null)
			{
				return null;
			}
			else
			{
				while (path != null && !otherPath.StartsWith(path, StringComparison.OrdinalIgnoreCase))//    PathComparer.IsEquivalent(path, otherPath))
				{
					path = Path.GetDirectoryName(path);
				}

				return path;
			}
		}
		#endregion

		#region Methods - Normalize
		/// <summary>
		/// Normalizes a file path, setting the correct case and removing invalid characters.
		/// This method also removes any leading/trailing whitespace from
		/// each file or directory in the path.
		/// Does not currently support UNC, URI, or non-Windows paths.
		/// </summary>
		/// <param name="path">The path to normalize.</param>
		/// <returns>
		/// A normalized and fully qualified path based on the input path.
		/// Returns null if the input path is null.
		/// </returns>
		/// <remarks>
		/// Useful information:
		/// https://msdn.microsoft.com/en-us/library/windows/desktop/aa365247.aspx
		/// </remarks>
		public static string Normalize(string path)
		{
			//If the path is null, there is nothing to normalize...
			if (path == null)
			{
				//...so return it as-is.
				return path;
			}
			else
			{
				//TODO: Implement network folder path notation.
				//if (_Path.IsUNCPath(path))
				//{
				//	throw new NotImplementedException();
				//}
				//else
				{
					var pathNodes = path.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).ToList();

					//Trim the whitespace from each of the nodes.
					Normalize_TrimNodes(pathNodes);

					//If the path is not already rooted, then root it.
					var rootNodes = Normalize_ExtractRootNodes(pathNodes);

					//Remove the empty nodes before removing the dot nodes, so that the
					// removal of a double dot node does not mistakenly remove empty nodes.
					Normalize_RemoveEmptyNodes(pathNodes);

					//Remove any dot nodes as well as double dot nodes and the node immediately preceeding it.
					// Do this BEFORE removing invalid characters. Otherwise, a filename like "|?*.@*|.?" might be
					// mistaken for a double dot node ("..") once the invalid characters are removed.
					Normalize_RemoveDotNodes(pathNodes);

					//Remove any invalid characters from the path.
					Normalize_RemoveInvalidCharacters(pathNodes);

					//Once all invalid characters and dot nodes have been
					// removed, then the empty nodes need to be removed.
					// (Yes, this method gets called twice.)
					Normalize_RemoveEmptyNodes(pathNodes);

					return Normalize_GetCorrectCase(pathNodes);
				}
			}
		}

		private static void Normalize_TrimNodes(List<string> pathNodes)
		{
			for (int i = 0; i < pathNodes.Count; i++)
			{
				//Trim any leading or trailing whitespace from the node.
				pathNodes[i] = pathNodes[i].Trim();
			}
		}

		private static List<string> Normalize_ExtractRootNodes(List<string> pathNodes)
		{
			//TODO: Extract the root node(s) in order to support formats like:
			//      \\pcname\sharename\some\path
			//      http://serverAddress/some/path
			//      file://pcname/C:/some/path
			//      file:///C:/some/path
			//      Reference: https://en.wikipedia.org/wiki/File_URI_scheme
			var rootNodes = new List<string>();


			//Note: The pathNodes list should NEVER contain less than 1 item,
			//      because it comes from a String.Split method call. This if-statement
			//      is present only for unit testing robustness.
			if (pathNodes.Count > 0)
			{
				if (pathNodes[0] == string.Empty)
				{
					if (pathNodes.Count == 1)
					{
						//If the empty node is the only node, then there was not a leading backslash.
						// Replace the empty string node with the current directory, split into nodes.
						// Result: "" becomes "C:\path\to\current\directory"
						pathNodes.RemoveAt(0);
						pathNodes.InsertRange(0, Directory.GetCurrentDirectory().Split(Path.DirectorySeparatorChar));
					}
					else
					{
						//If there is more than one node, then the original path contained a leading backslash.
						// Replace the empty string node with the current directory's root
						// folder, removing its trailing backslash. E.g. "C:"
						// Result: "\" becomes "C:\"
						// Result: "\some\path" becomes "C:\some\path"
						pathNodes[0] = Path.GetPathRoot(Directory.GetCurrentDirectory()).TrimEnd(Path.DirectorySeparatorChar);
					}
				}
				else
				{
					var firstNodeNoWhitespace = Regex.Replace(pathNodes[0], @"\s+", string.Empty);

					if (firstNodeNoWhitespace.Length > 1 && firstNodeNoWhitespace[1] == Path.VolumeSeparatorChar)
					{
						//Note: Windows allows non-letter drives, like "1:" or "2:" or "!:",
						//      so we do not test the first character.

						if (firstNodeNoWhitespace.Length == 2)
						{
							//Replace the first node with the whitespace-removed string.
							// Result: "C  :\some\path" becomes "C:\some\path"
							pathNodes[0] = firstNodeNoWhitespace;

							if (pathNodes.Count == 1)
							{
								//Insert the current directory path, with its root folder drive letter removed, after the driver letter.
								// Result: "C:" becomes "C:\path\to\current\directory"
								pathNodes.InsertRange(1, Directory.GetCurrentDirectory().Split(Path.DirectorySeparatorChar).Skip(1));
							}
						}
						else
						{
							//The first node implements the C:FirstNodeFolder path notation.

							//Parse the first node folder and insert it after the drive letter.
							pathNodes.Insert(1, pathNodes[0].Substring(pathNodes[0].IndexOf(':') + 1).Trim());

							//Insert the current directory path (with its root
							// folder drive letter removed) to lead to the first-node folder.
							pathNodes.InsertRange(1, Directory.GetCurrentDirectory().Split(Path.DirectorySeparatorChar).Skip(1));

							//Replace the first node with the whitespace-removed string.
							pathNodes[0] = firstNodeNoWhitespace.Substring(0, 2);
						}
					}
					else
					{
						//Insert the current directory (split into nodes) before the first
						// node, even if the first node is a dot node or a double dot node.
						pathNodes.InsertRange(0, Directory.GetCurrentDirectory().Split(Path.DirectorySeparatorChar));
					}
				}

				//By this point, the first node is guaranteed to contain the drive letter (root folder). E.g. "C:"
				pathNodes[0] = pathNodes[0].ToUpper();
			}

			return rootNodes;
		}

		private static void Normalize_RemoveEmptyNodes(List<string> pathNodes)
		{
			//Leave the first node alone.
			// By this point, it should have a valid root folder string.

			for (int i = 1; i < pathNodes.Count; i++)
			{
				if (pathNodes[i] == string.Empty)
				{
					//Remove the empty node.
					pathNodes.RemoveAt(i);

					//Decrement the counter, since we removed an item.
					i--;
				}
			}
		}

		private static void Normalize_RemoveDotNodes(List<string> pathNodes)
		{
			//Leave the first node alone.
			// By this point, it should have a valid root folder string.

			for (int i = 1; i < pathNodes.Count; i++)
			{
				if (pathNodes[i] == ".")
				{
					//Remove the dot node.
					pathNodes.RemoveAt(i);

					//Decrement the counter, since we removed an item.
					i--;
				}
				else if (pathNodes[i] == "..")
				{
					//Remove the double dot node.
					pathNodes.RemoveAt(i);

					//Decrement the counter, since we removed an item.
					i--;

					//If we have not double-dotted back all the way
					// back to the root folder, then remove one more node.
					if (i > 0)
					{
						pathNodes.RemoveAt(i);
						i--;
					}
				}
			}
		}

		private static void Normalize_RemoveInvalidCharacters(List<string> pathNodes)
		{
			//Leave the first node alone.
			// By this point, it should have a valid root folder string.

			for (int i = 1; i < pathNodes.Count; i++)
			{
				//Remove invalid path characters.
				foreach (var invalidCharacter in Path.GetInvalidPathChars())
				{
					pathNodes[i] = pathNodes[i].Replace(invalidCharacter.ToString(), string.Empty);
				}

				//Remove the volume separator character ":"
				pathNodes[i] = pathNodes[i].Replace(Path.VolumeSeparatorChar.ToString(), string.Empty);

				//Trim remaining whitespace from the node.
				pathNodes[i] = pathNodes[i].Trim();

				//Trim any trailing dots and whitespace buffering them.
				while (pathNodes[i].EndsWith("."))
				{
					pathNodes[i] = pathNodes[i].TrimEnd('.').TrimEnd();
				}
			}
		}

		private static string Normalize_GetCorrectCase(List<string> pathNodes, int index = -1)
		{
			string parentPath;
			string currentNode;
			string currentPath;

			if (index == -1)
			{
				index = pathNodes.Count - 1;
			}

			if (index == 0)
			{
				return pathNodes[0] + Path.DirectorySeparatorChar;
			}
			else
			{
				parentPath = Normalize_GetCorrectCase(pathNodes, index - 1);
				currentNode = pathNodes[index];
				currentPath = Path.Combine(parentPath, currentNode);

				if (File.Exists(currentPath) || Directory.Exists(currentPath))
				{
					//Return the full path to the current file/directory using
					// the FileSystemInfo class to ensure the correct case is used
					// for the current file/directory's name. The case used for the
					// parent directory's path is determined by what is passed into
					// the DirectoryInfo constructor.
					return
						new DirectoryInfo(parentPath)
							.GetFileSystemInfos(currentNode)[0]
							.FullName;
				}
				else
				{
					//If the file/directory does not exist, then return the normalized
					// parent path combined with the name of the non-existent file/directory.
					return currentPath;
				}
			}
		}
		#endregion

		#region TODO: UNC Path
		//// \\ -> error
		//// \\pcname -> ???
		//// \\pcname\sharename -> ???
		//// \\pcname\sharename\some\path -> ???
		//private static List<string> Normalize_ExtractUNCRootNodes(List<string> pathNodes)
		//{
		//	if (pathNodes.Count > 2 && pathNodes.Take(2).All(node => node.IsBlank()))
		//	{

		//	}
		//}

		//private static readonly string UNCPathIndicator = _Path.DirectorySeparatorString + _Path.DirectorySeparatorString;

		//private static bool IsUNCPath(string path)
		//{
		//	return path != null && path.StartsWith(_Path.UNCPathIndicator);
		//}

		//TODO: This.
		//public static string Normalize(string path, bool useNetworkMappedDrive)
		//{
		//	return Normalize(path, false);
		//}
		//public static string Normalize(string path, bool makePathAbsolute)
		//{
		//	return Normalize(path, false);
		//}

		#endregion
	}
}
/*


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 
namespace RosettaCodeTasks
{
 
	class Program
	{
		static void Main ( string[ ] args )
		{
			FindCommonDirectoryPath.Test ( );
		}
 
	}
 
	class FindCommonDirectoryPath
	{
		public static void Test ( )
		{
			Console.WriteLine ( "Find Common Directory Path" );
			Console.WriteLine ( );
			List<string> PathSet1 = new List<string> ( );
			PathSet1.Add ( "/home/user1/tmp/coverage/test" );
			PathSet1.Add ( "/home/user1/tmp/covert/operator" );
			PathSet1.Add ( "/home/user1/tmp/coven/members" );
			Console.WriteLine("Path Set 1 (All Absolute Paths):");
			foreach ( string path in PathSet1 )
			{
				Console.WriteLine ( path );
			}
			Console.WriteLine ( "Path Set 1 Common Path: {0}", FindCommonPath ( "/", PathSet1 ) );
		}
		public static string FindCommonPath ( string Separator, List<string> Paths )
		{
			string CommonPath = String.Empty;
			List<string> SeparatedPath = Paths
				.First ( str => str.Length == Paths.Max ( st2 => st2.Length ) )
				.Split ( new string[ ] { Separator }, StringSplitOptions.RemoveEmptyEntries )
				.ToList ( );
 
			foreach ( string PathSegment in SeparatedPath.AsEnumerable ( ) )
			{
				if ( CommonPath.Length == 0 && Paths.All ( str => str.StartsWith ( PathSegment ) ) )
				{
					CommonPath = PathSegment;
				}
				else if ( Paths.All ( str => str.StartsWith ( CommonPath + Separator + PathSegment ) ) )
				{
					CommonPath += Separator + PathSegment;
				}
				else
				{
					break;
				}
			}
 
			return CommonPath;
		}
	}
}


*/
