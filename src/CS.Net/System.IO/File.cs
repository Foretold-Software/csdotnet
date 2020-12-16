// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

namespace System.IO
{
	/// <summary>
	/// A static class with helper methods to simplify file operations.
	/// </summary>
	public static class _File
	{
		/// <summary>
		/// Copies a file to the specified destination, optionally
		/// creating the destination directory if it does not already exist.
		/// </summary>
		/// <param name="sourceFileName">
		/// The file to be copied.
		/// </param>
		/// <param name="destFileName">
		/// The path to the destination file that is the target of the copy.
		/// </param>
		/// <param name="overwrite">
		/// Indicates whether to overwrite the target file, if it already exists.
		/// </param>
		/// <param name="createDirectory">
		/// Indicates whether to create the target file's parent directory, if it does not already exist.
		/// </param>
		public static void Copy(string sourceFileName, string destFileName, bool overwrite = false, bool createDirectory = false)
		{
			string directory;

			if (createDirectory)
			{
				directory = Path.GetDirectoryName(destFileName);

				if (!Directory.Exists(directory))
				{
					Directory.CreateDirectory(directory);
				}
			}

			File.Copy(sourceFileName, destFileName, overwrite);
		}
	}
}
