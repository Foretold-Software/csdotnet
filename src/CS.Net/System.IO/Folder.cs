// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.Linq;

namespace System.IO
{
	public class Folder
	{
		#region Constructor
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
		public string FullName
		{ get; private set; }
		public string Name
		{ get { return Path.GetFileName(this.FullName); } }
		#endregion

		#region Methods - Copy
		public Folder CopyTo(string destination)
		{
			return this.CopyContentsTo(new Folder(Path.Combine(destination, this.Name)), RecursiveByDefault, OverwriteByDefault);
		}
		public Folder CopyTo(string destination, bool recursive)
		{
			return this.CopyContentsTo(new Folder(Path.Combine(destination, this.Name)), recursive, OverwriteByDefault);
		}
		public Folder CopyTo(string destination, bool recursive, bool overwrite)
		{
			return this.CopyContentsTo(new Folder(Path.Combine(destination, this.Name)), recursive, overwrite);
		}
		public Folder CopyTo(Folder destination)
		{
			return this.CopyContentsTo(destination.GetSubfolder(this.Name), RecursiveByDefault, OverwriteByDefault);
		}
		public Folder CopyTo(Folder destination, bool recursive)
		{
			return this.CopyContentsTo(destination.GetSubfolder(this.Name), recursive, OverwriteByDefault);
		}
		public Folder CopyTo(Folder destination, bool recursive, bool overwrite)
		{
			return this.CopyContentsTo(destination.GetSubfolder(this.Name), recursive, overwrite);
		}
		public Folder CopyContentsTo(string destination)
		{
			return this.CopyContentsTo(new Folder(destination), RecursiveByDefault, OverwriteByDefault);
		}
		public Folder CopyContentsTo(string destination, bool recursive)
		{
			return this.CopyContentsTo(new Folder(destination), recursive, OverwriteByDefault);
		}
		public Folder CopyContentsTo(string destination, bool recursive, bool overwrite)
		{
			return this.CopyContentsTo(new Folder(destination), recursive, overwrite);
		}
		public Folder CopyContentsTo(Folder destination)
		{
			return this.CopyContentsTo(destination, RecursiveByDefault, OverwriteByDefault);
		}
		public Folder CopyContentsTo(Folder destination, bool recursive)
		{
			return this.CopyContentsTo(destination, recursive, OverwriteByDefault);
		}
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
		[Obsolete("If you use this method, then the folder that this object represents will be gone and an error might occur.", true)]
		public Folder MoveTo(string destination)
		{
			Folder folder = this.MoveContentsTo(new Folder(Path.Combine(destination, this.Name)), RecursiveByDefault, OverwriteByDefault);

			Directory.Delete(this.FullName, true);
			return folder;
		}
		[Obsolete("If you use this method, then the folder that this object represents will be gone and an error might occur.", true)]
		public Folder MoveTo(string destination, bool recursive)
		{
			Folder folder = this.MoveContentsTo(new Folder(Path.Combine(destination, this.Name)), recursive, OverwriteByDefault);

			Directory.Delete(this.FullName, true);
			return folder;
		}
		[Obsolete("If you use this method, then the folder that this object represents will be gone and an error might occur.", true)]
		public Folder MoveTo(string destination, bool recursive, bool overwrite)
		{
			Folder folder = this.MoveContentsTo(new Folder(Path.Combine(destination, this.Name)), recursive, overwrite);

			Directory.Delete(this.FullName, true);
			return folder;
		}
		[Obsolete("If you use this method, then the folder that this object represents will be gone and an error might occur.", true)]
		public Folder MoveTo(Folder destination)
		{
			Folder folder = this.MoveContentsTo(destination.GetSubfolder(this.Name), RecursiveByDefault, OverwriteByDefault);

			Directory.Delete(this.FullName, true);
			return folder;
		}
		[Obsolete("If you use this method, then the folder that this object represents will be gone and an error might occur.", true)]
		public Folder MoveTo(Folder destination, bool recursive)
		{
			Folder folder = this.MoveContentsTo(destination.GetSubfolder(this.Name), recursive, OverwriteByDefault);

			Directory.Delete(this.FullName, true);
			return folder;
		}
		[Obsolete("If you use this method, then the folder that this object represents will be gone and an error might occur.", true)]
		public Folder MoveTo(Folder destination, bool recursive, bool overwrite)
		{
			Folder folder = this.MoveContentsTo(destination.GetSubfolder(this.Name), recursive, overwrite);

			Directory.Delete(this.FullName, true);
			return folder;
		}
		public Folder MoveContentsTo(string destination)
		{
			return this.MoveContentsTo(new Folder(destination), RecursiveByDefault, OverwriteByDefault);
		}
		public Folder MoveContentsTo(string destination, bool recursive)
		{
			return this.MoveContentsTo(new Folder(destination), recursive, OverwriteByDefault);
		}
		public Folder MoveContentsTo(string destination, bool recursive, bool overwrite)
		{
			return this.MoveContentsTo(new Folder(destination), recursive, overwrite);
		}
		public Folder MoveContentsTo(Folder destination)
		{
			return this.MoveContentsTo(destination, RecursiveByDefault, OverwriteByDefault);
		}
		public Folder MoveContentsTo(Folder destination, bool recursive)
		{
			return this.MoveContentsTo(destination, recursive, OverwriteByDefault);
		}
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
		public string[] GetFiles()
		{
			return this.GetFiles(RecursiveByDefault);
		}
		public string[] GetFiles(bool recursive)
		{
			var searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

			var files = Directory.GetFiles(this.FullName, "*", searchOption);

			return files;
		}
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

		public Folder GetSubfolder(string name)
		{
			return new Folder(Path.Combine(this.FullName, name));
		}

		public Folder[] GetSubfolders()
		{
			return this.GetSubfolders(RecursiveByDefault);
		}
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
		/// <param name="directory">The directory to create.</param>
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
