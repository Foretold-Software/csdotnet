// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.Linq;

namespace System.IO
{
	/// <summary>
	/// A class with helper methods to simplify folder operations.
	/// </summary>
	public class Folder
	{
		#region Constructor
		/// <summary>
		/// Creates a new instance of <see cref="Folder"/> and
		/// ensures a folder at the specified path exists.
		/// </summary>
		/// <param name="path">
		/// The path to the folder.
		/// </param>
		public Folder(string path)
		{
			this.FullName = Path.GetFullPath(path);
			Folder.CreateVerifyDirectory(this.FullName);
		}
		#endregion

		#region Fields
		private const bool OverwriteByDefault = true;
		private const bool RecursiveByDefault = true;
		#endregion

		#region Properties
		/// <summary>
		/// The fully qualified path to the folder.
		/// </summary>
		public string FullName
		{ get; private set; }

		/// <summary>
		/// The name of the folder, not including its full path.
		/// </summary>
		public string Name
		{ get { return Path.GetFileName(this.FullName); } }
		#endregion

		#region Methods - Copy
		/// <summary>
		/// Copies the entire folder, including the root folder itself, to the destination.
		/// </summary>
		/// <param name="destination">
		/// The folder to which this folder is copied.
		/// </param>
		/// <returns>
		/// Returns an instance of <see cref="Folder"/> representing the copy of this folder within the destination.
		/// </returns>
		public Folder CopyTo(string destination)
		{
			return this.CopyContentsTo(new Folder(Path.Combine(destination, this.Name)), RecursiveByDefault, OverwriteByDefault);
		}

		/// <summary>
		/// Copies the entire folder, including the root folder itself, to the destination.
		/// </summary>
		/// <param name="destination">
		/// The folder to which this folder is copied.
		/// </param>
		/// <param name="recursive">
		/// Indicates whether to copy this folder's contents recursively.
		/// </param>
		/// <returns>
		/// Returns an instance of <see cref="Folder"/> representing the copy of this folder within the destination.
		/// </returns>
		public Folder CopyTo(string destination, bool recursive)
		{
			return this.CopyContentsTo(new Folder(Path.Combine(destination, this.Name)), recursive, OverwriteByDefault);
		}

		/// <summary>
		/// Copies the entire folder, including the root folder itself, to the destination.
		/// </summary>
		/// <param name="destination">
		/// The folder to which this folder is copied.
		/// </param>
		/// <param name="recursive">
		/// Indicates whether to copy this folder's contents recursively.
		/// </param>
		/// <param name="overwrite">
		/// Indicates whether to overwrite existing files of the same name in the destination folder.
		/// </param>
		/// <returns>
		/// Returns an instance of <see cref="Folder"/> representing the copy of this folder within the destination.
		/// </returns>
		public Folder CopyTo(string destination, bool recursive, bool overwrite)
		{
			return this.CopyContentsTo(new Folder(Path.Combine(destination, this.Name)), recursive, overwrite);
		}

		/// <summary>
		/// Copies the entire folder, including the root folder itself, to the destination.
		/// </summary>
		/// <param name="destination">
		/// The folder to which this folder is copied.
		/// </param>
		/// <returns>
		/// Returns an instance of <see cref="Folder"/> representing the copy of this folder within the destination.
		/// </returns>
		public Folder CopyTo(Folder destination)
		{
			return this.CopyContentsTo(destination.GetSubfolder(this.Name), RecursiveByDefault, OverwriteByDefault);
		}

		/// <summary>
		/// Copies the entire folder, including the root folder itself, to the destination.
		/// </summary>
		/// <param name="destination">
		/// The folder to which this folder is copied.
		/// </param>
		/// <param name="recursive">
		/// Indicates whether to copy this folder's contents recursively.
		/// </param>
		/// <returns>
		/// Returns an instance of <see cref="Folder"/> representing the copy of this folder within the destination.
		/// </returns>
		public Folder CopyTo(Folder destination, bool recursive)
		{
			return this.CopyContentsTo(destination.GetSubfolder(this.Name), recursive, OverwriteByDefault);
		}

		/// <summary>
		/// Copies the entire folder, including the root folder itself, to the destination.
		/// </summary>
		/// <param name="destination">
		/// The folder to which this folder is copied.
		/// </param>
		/// <param name="recursive">
		/// Indicates whether to copy this folder's contents recursively.
		/// </param>
		/// <param name="overwrite">
		/// Indicates whether to overwrite existing files of the same name in the destination folder.
		/// </param>
		/// <returns>
		/// Returns an instance of <see cref="Folder"/> representing the copy of this folder within the destination.
		/// </returns>
		public Folder CopyTo(Folder destination, bool recursive, bool overwrite)
		{
			return this.CopyContentsTo(destination.GetSubfolder(this.Name), recursive, overwrite);
		}

		/// <summary>
		/// Copies the contents of the current folder to the destination, excluding the folder itself.
		/// </summary>
		/// <param name="destination">
		/// The folder to which all the contents of this folder are copied.
		/// </param>
		/// <returns>
		/// Returns an instance of <see cref="Folder"/> representing the destination.
		/// </returns>
		public Folder CopyContentsTo(string destination)
		{
			return this.CopyContentsTo(new Folder(destination), RecursiveByDefault, OverwriteByDefault);
		}

		/// <summary>
		/// Copies the contents of the current folder to the destination, excluding the folder itself.
		/// </summary>
		/// <param name="destination">
		/// The folder to which all the contents of this folder are copied.
		/// </param>
		/// <param name="recursive">
		/// Indicates whether to copy this folder's contents recursively.
		/// </param>
		/// <returns>
		/// Returns an instance of <see cref="Folder"/> representing the destination.
		/// </returns>
		public Folder CopyContentsTo(string destination, bool recursive)
		{
			return this.CopyContentsTo(new Folder(destination), recursive, OverwriteByDefault);
		}

		/// <summary>
		/// Copies the contents of the current folder to the destination, excluding the folder itself.
		/// </summary>
		/// <param name="destination">
		/// The folder to which all the contents of this folder are copied.
		/// </param>
		/// <param name="recursive">
		/// Indicates whether to copy this folder's contents recursively.
		/// </param>
		/// <param name="overwrite">
		/// Indicates whether to overwrite existing files of the same name in the destination folder.
		/// </param>
		/// <returns>
		/// Returns an instance of <see cref="Folder"/> representing the destination.
		/// </returns>
		public Folder CopyContentsTo(string destination, bool recursive, bool overwrite)
		{
			return this.CopyContentsTo(new Folder(destination), recursive, overwrite);
		}

		/// <summary>
		/// Copies the contents of the current folder to the destination, excluding the folder itself.
		/// </summary>
		/// <param name="destination">
		/// The folder to which all the contents of this folder are copied.
		/// </param>
		/// <returns>
		/// Returns an instance of <see cref="Folder"/> representing the destination.
		/// </returns>
		public Folder CopyContentsTo(Folder destination)
		{
			return this.CopyContentsTo(destination, RecursiveByDefault, OverwriteByDefault);
		}

		/// <summary>
		/// Copies the contents of the current folder to the destination, excluding the folder itself.
		/// </summary>
		/// <param name="destination">
		/// The folder to which all the contents of this folder are copied.
		/// </param>
		/// <param name="recursive">
		/// Indicates whether to copy this folder's contents recursively.
		/// </param>
		/// <returns>
		/// Returns an instance of <see cref="Folder"/> representing the destination.
		/// </returns>
		public Folder CopyContentsTo(Folder destination, bool recursive)
		{
			return this.CopyContentsTo(destination, recursive, OverwriteByDefault);
		}

		/// <summary>
		/// Copies the contents of the current folder to the destination, excluding the folder itself.
		/// </summary>
		/// <param name="destination">
		/// The folder to which all the contents of this folder are copied.
		/// </param>
		/// <param name="recursive">
		/// Indicates whether to copy this folder's contents recursively.
		/// </param>
		/// <param name="overwrite">
		/// Indicates whether to overwrite existing files of the same name in the destination folder.
		/// </param>
		/// <returns>
		/// Returns an instance of <see cref="Folder"/> representing the destination.
		/// </returns>
		public Folder CopyContentsTo(Folder destination, bool recursive, bool overwrite)
		{
			Folder subFolder;
			var searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

			//Gets an array of the relative paths of the subdirectories within this directory.
			var dirs = Directory
						.GetDirectories(this.FullName, "*", searchOption)
						.Select(dirPath => dirPath.Substring(this.FullName.Length + 1));

			//Gets an array of the relative paths of the files within this directory.
			var files = Directory
						.GetFiles(this.FullName, "*", searchOption)
						.Select(filePath => filePath.Substring(this.FullName.Length + 1));

			//Create the new subdirectories.
			foreach (var dir in dirs)
			{
				subFolder = new Folder(Path.Combine(destination.FullName, dir));
			}

			//Create the new files.
			foreach (var file in files)
			{
				File.Copy(Path.Combine(this.FullName, file), Path.Combine(destination.FullName, file), overwrite);
			}

			return destination;
		}
		#endregion

		#region Methods - Move
		//TODO: Should I delete MoveTo altogether?

		/// <summary>
		/// Moves the entire folder, including the root folder itself, to the destination.
		/// </summary>
		/// <param name="destination">
		/// The folder to which this folder is moved.
		/// </param>
		/// <returns>
		/// Returns an instance of <see cref="Folder"/> representing the new folder within the destination to which this folder was moved.
		/// </returns>
		[Obsolete("If you use this method, then the folder that this object represents will be gone and an error might occur.", true)]
		public Folder MoveTo(string destination)
		{
			Folder folder = this.MoveContentsTo(new Folder(Path.Combine(destination, this.Name)), RecursiveByDefault, OverwriteByDefault);

			Directory.Delete(this.FullName, true);
			return folder;
		}

		/// <summary>
		/// Moves the entire folder, including the root folder itself, to the destination.
		/// </summary>
		/// <param name="destination">
		/// The folder to which this folder is moved.
		/// </param>
		/// <param name="recursive">
		/// Indicates whether to move this folder's contents recursively.
		/// </param>
		/// <returns>
		/// Returns an instance of <see cref="Folder"/> representing the new folder within the destination to which this folder was moved.
		/// </returns>
		[Obsolete("If you use this method, then the folder that this object represents will be gone and an error might occur.", true)]
		public Folder MoveTo(string destination, bool recursive)
		{
			Folder folder = this.MoveContentsTo(new Folder(Path.Combine(destination, this.Name)), recursive, OverwriteByDefault);

			Directory.Delete(this.FullName, true);
			return folder;
		}

		/// <summary>
		/// Moves the entire folder, including the root folder itself, to the destination.
		/// </summary>
		/// <param name="destination">
		/// The folder to which this folder is moved.
		/// </param>
		/// <param name="recursive">
		/// Indicates whether to move this folder's contents recursively.
		/// </param>
		/// <param name="overwrite">
		/// Indicates whether to overwrite existing files of the same name in the destination folder.
		/// </param>
		/// <returns>
		/// Returns an instance of <see cref="Folder"/> representing the new folder within the destination to which this folder was moved.
		/// </returns>
		[Obsolete("If you use this method, then the folder that this object represents will be gone and an error might occur.", true)]
		public Folder MoveTo(string destination, bool recursive, bool overwrite)
		{
			Folder folder = this.MoveContentsTo(new Folder(Path.Combine(destination, this.Name)), recursive, overwrite);

			Directory.Delete(this.FullName, true);
			return folder;
		}

		/// <summary>
		/// Moves the entire folder, including the root folder itself, to the destination.
		/// </summary>
		/// <param name="destination">
		/// The folder to which this folder is moved.
		/// </param>
		/// <returns>
		/// Returns an instance of <see cref="Folder"/> representing the new folder within the destination to which this folder was moved.
		/// </returns>
		[Obsolete("If you use this method, then the folder that this object represents will be gone and an error might occur.", true)]
		public Folder MoveTo(Folder destination)
		{
			Folder folder = this.MoveContentsTo(destination.GetSubfolder(this.Name), RecursiveByDefault, OverwriteByDefault);

			Directory.Delete(this.FullName, true);
			return folder;
		}

		/// <summary>
		/// Moves the entire folder, including the root folder itself, to the destination.
		/// </summary>
		/// <param name="destination">
		/// The folder to which this folder is moved.
		/// </param>
		/// <param name="recursive">
		/// Indicates whether to move this folder's contents recursively.
		/// </param>
		/// <returns>
		/// Returns an instance of <see cref="Folder"/> representing the new folder within the destination to which this folder was moved.
		/// </returns>
		[Obsolete("If you use this method, then the folder that this object represents will be gone and an error might occur.", true)]
		public Folder MoveTo(Folder destination, bool recursive)
		{
			Folder folder = this.MoveContentsTo(destination.GetSubfolder(this.Name), recursive, OverwriteByDefault);

			Directory.Delete(this.FullName, true);
			return folder;
		}

		/// <summary>
		/// Moves the entire folder, including the root folder itself, to the destination.
		/// </summary>
		/// <param name="destination">
		/// The folder to which this folder is moved.
		/// </param>
		/// <param name="recursive">
		/// Indicates whether to move this folder's contents recursively.
		/// </param>
		/// <param name="overwrite">
		/// Indicates whether to overwrite existing files of the same name in the destination folder.
		/// </param>
		/// <returns>
		/// Returns an instance of <see cref="Folder"/> representing the new folder within the destination to which this folder was moved.
		/// </returns>
		[Obsolete("If you use this method, then the folder that this object represents will be gone and an error might occur.", true)]
		public Folder MoveTo(Folder destination, bool recursive, bool overwrite)
		{
			Folder folder = this.MoveContentsTo(destination.GetSubfolder(this.Name), recursive, overwrite);

			Directory.Delete(this.FullName, true);
			return folder;
		}

		/// <summary>
		/// Moves the contents of the current folder to the destination, excluding the folder itself.
		/// </summary>
		/// <param name="destination">
		/// The folder to which all the contents of this folder are moved.
		/// </param>
		/// <returns>
		/// Returns an instance of <see cref="Folder"/> representing the destination.
		/// </returns>
		public Folder MoveContentsTo(string destination)
		{
			return this.MoveContentsTo(new Folder(destination), RecursiveByDefault, OverwriteByDefault);
		}

		/// <summary>
		/// Moves the contents of the current folder to the destination, excluding the folder itself.
		/// </summary>
		/// <param name="destination">
		/// The folder to which all the contents of this folder are moved.
		/// </param>
		/// <param name="recursive">
		/// Indicates whether to move this folder's contents recursively.
		/// </param>
		/// <returns>
		/// Returns an instance of <see cref="Folder"/> representing the destination.
		/// </returns>
		public Folder MoveContentsTo(string destination, bool recursive)
		{
			return this.MoveContentsTo(new Folder(destination), recursive, OverwriteByDefault);
		}

		/// <summary>
		/// Moves the contents of the current folder to the destination, excluding the folder itself.
		/// </summary>
		/// <param name="destination">
		/// The folder to which all the contents of this folder are moved.
		/// </param>
		/// <param name="recursive">
		/// Indicates whether to move this folder's contents recursively.
		/// </param>
		/// <param name="overwrite">
		/// Indicates whether to overwrite existing files of the same name in the destination folder.
		/// </param>
		/// <returns>
		/// Returns an instance of <see cref="Folder"/> representing the destination.
		/// </returns>
		public Folder MoveContentsTo(string destination, bool recursive, bool overwrite)
		{
			return this.MoveContentsTo(new Folder(destination), recursive, overwrite);
		}

		/// <summary>
		/// Moves the contents of the current folder to the destination, excluding the folder itself.
		/// </summary>
		/// <param name="destination">
		/// The folder to which all the contents of this folder are moved.
		/// </param>
		/// <returns>
		/// Returns an instance of <see cref="Folder"/> representing the destination.
		/// </returns>
		public Folder MoveContentsTo(Folder destination)
		{
			return this.MoveContentsTo(destination, RecursiveByDefault, OverwriteByDefault);
		}

		/// <summary>
		/// Moves the contents of the current folder to the destination, excluding the folder itself.
		/// </summary>
		/// <param name="destination">
		/// The folder to which all the contents of this folder are moved.
		/// </param>
		/// <param name="recursive">
		/// Indicates whether to move this folder's contents recursively.
		/// </param>
		/// <returns>
		/// Returns an instance of <see cref="Folder"/> representing the destination.
		/// </returns>
		public Folder MoveContentsTo(Folder destination, bool recursive)
		{
			return this.MoveContentsTo(destination, recursive, OverwriteByDefault);
		}

		/// <summary>
		/// Moves the contents of the current folder to the destination, excluding the folder itself.
		/// </summary>
		/// <param name="destination">
		/// The folder to which all the contents of this folder are moved.
		/// </param>
		/// <param name="recursive">
		/// Indicates whether to move this folder's contents recursively.
		/// </param>
		/// <param name="overwrite">
		/// Indicates whether to overwrite existing files of the same name in the destination folder.
		/// </param>
		/// <returns>
		/// Returns an instance of <see cref="Folder"/> representing the destination.
		/// </returns>
		public Folder MoveContentsTo(Folder destination, bool recursive, bool overwrite)
		{
			Folder subFolder;
			string destFile;
			var searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

			//Gets an array of the relative paths of the subdirectories within this directory.
			var dirs = Directory
						.GetDirectories(this.FullName, "*", searchOption)
						.Select(dirPath => dirPath.Substring(this.FullName.Length + 1));

			//Gets an array of the relative paths of the files within this directory.
			var files = Directory
						.GetFiles(this.FullName, "*", searchOption)
						.Select(filePath => filePath.Substring(this.FullName.Length + 1));

			//Create the new subdirectories.
			foreach (var dir in dirs)
			{
				subFolder = new Folder(Path.Combine(destination.FullName, dir));
			}

			//Create the new files.
			foreach (var file in files)
			{
				destFile = Path.Combine(destination.FullName, file);

				if (!File.Exists(destFile) || overwrite)
				{
					File.Move(Path.Combine(this.FullName, file), destFile);
				}
			}

			return destination;
		}
		#endregion

		#region Methods - Overrides
		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>
		/// A string that represents the current object.
		/// </returns>
		public override string ToString()
		{
			return this.FullName;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Gets the file contents of the current folder.
		/// </summary>
		/// <returns>
		/// Returns an array of strings representing the fully qualified file paths of each of the files contained within the current folder.
		/// </returns>
		public string[] GetFiles()
		{
			return this.GetFiles(RecursiveByDefault);
		}

		/// <summary>
		/// Gets the file contents of the current folder.
		/// </summary>
		/// <param name="recursive">
		/// Indicates whether to search recursively.
		/// </param>
		/// <returns>
		/// Returns an array of strings representing the fully qualified file paths of each of the files contained within the current folder.
		/// </returns>
		public string[] GetFiles(bool recursive)
		{
			var searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

			var files = Directory.GetFiles(this.FullName, "*", searchOption);

			return files;
		}

		/// <summary>
		/// Gets the file contents of the current folder.
		/// </summary>
		/// <param name="path">
		/// The path to the directory to search.
		/// </param>
		/// <param name="searchPattern">
		/// The search string to match against the names of files in path.
		/// </param>
		/// <param name="searchOption">
		/// A value of type <see cref="SearchOption"/> indicating whether to search recursively.
		/// </param>
		/// <returns>
		/// Returns an array of strings representing the fully qualified file paths of each of the files contained within the current folder.
		/// If an error occurs during the search, an empty array is returned.
		/// </returns>
		public static string[] GetFilesSafe(string path, string searchPattern = "*", SearchOption searchOption = SearchOption.TopDirectoryOnly)
		{
			try
			{
				return Directory.GetFiles(path, searchPattern, searchOption);
			}
			catch
			{
				return new string[0];
			}
		}

		/// <summary>
		/// Get a subfolder of the current folder.
		/// </summary>
		/// <param name="name">
		/// The name or relative path of the subfolder within the current folder.
		/// </param>
		/// <returns>
		/// Returns a <see cref="Folder"/> objects representing the subfolder.
		/// </returns>
		public Folder GetSubfolder(string name)
		{
			return new Folder(Path.Combine(this.FullName, name));
		}

		/// <summary>
		/// Gets a collection of folders contained within the current folder.
		/// </summary>
		/// <returns>
		/// Returns an array of <see cref="Folder"/> objects representing the folders contained within the current folder.
		/// </returns>
		public Folder[] GetSubfolders()
		{
			return this.GetSubfolders(RecursiveByDefault);
		}

		/// <summary>
		/// Gets a collection of folders contained within the current folder.
		/// </summary>
		/// <param name="recursive">
		/// Indicates whether to search recursively.
		/// </param>
		/// <returns>
		/// Returns an array of <see cref="Folder"/> objects representing the folders contained within the current folder.
		/// </returns>
		public Folder[] GetSubfolders(bool recursive)
		{
			var searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

			//Gets an array of the absolute paths of the subdirectories within this directory.
			var dirs = Directory
						.GetDirectories(this.FullName, "*", searchOption)
						.Select(dir => new Folder(dir))
						.ToArray();

			return dirs;
		}

		/// <summary>
		/// Verifies that the specified path is a directory
		/// and not an existing file, or creates the directory
		/// if it does not exist.
		/// </summary>
		/// <param name="directory">
		/// The directory to create.
		/// </param>
		/// <exception cref="System.ArgumentException">
		/// Thrown if directory is null, empty, or whitespace, or if
		/// the specified directory already exists as a file, or if
		/// the specified directory could not be created.
		/// </exception>
		private static void CreateVerifyDirectory(string directory)
		{
			if (directory.IsBlank())
			{
				throw new ArgumentException("Must provide a directory location.");
			}

			if (!Directory.Exists(directory))
			{
				if (File.Exists(directory))
				{
					throw new ArgumentException("A file with this name already exists: " + directory);
				}

				var dir = Directory.CreateDirectory(directory);

				if (dir == null)
				{
					throw new ArgumentException("Directory could not be created: " + directory);
				}
			}
		}
		#endregion

		#region Operators
		///// <summary>
		///// Converts an instance of <see cref="Folder"/> to a string.
		///// </summary>
		///// <param name="folder">The instance of <see cref="Folder"/> to convert.</param>
		///// <returns>The path to the folder as a string value.</returns>
		//public static implicit operator string(Folder folder)
		//{
		//	return folder.FullName;
		//}

		///// <summary>
		///// Converts a string path to an instance of <see cref="Folder"/>.
		///// </summary>
		///// <param name="path">The path to the folder.</param>
		///// <returns>An instance of <see cref="Folder"/>.</returns>
		//public static implicit operator Folder(string path)
		//{
		//	return new Folder(path);
		//}
		#endregion
	}
}
