// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

namespace System.IO
{
	/// <summary>
	/// A class used for creation and disposal of temporary folders by way of <see cref="IDisposable"/>.
	/// </summary>
	/// <remarks>
	/// Dispose Pattern reference:
	/// https://msdn.microsoft.com/en-us/library/b1yfkh5e.aspx
	/// </remarks>
	public class TempFolder : Folder, IDisposable
	{
		#region Constructor / Destructor
		/// <summary>
		/// Creates an instance of <see cref="TempFolder"/>.
		/// </summary>
		/// <param name="path">
		/// The path to the folder to create.
		/// </param>
		private TempFolder(string path) : base(path)
		{
			this.SuppressDisposal = false;
		}

		/// <summary>
		/// Creates an instance of <see cref="TempFolder"/>.
		/// </summary>
		/// <param name="path">
		/// The path to the folder to create.
		/// </param>
		/// <param name="suppressDisposal">
		/// Indicates whether to suppress disposal of the objects resources when the object reference is lost.
		/// </param>
		private TempFolder(string path, bool suppressDisposal) : base(path)
		{
			this.SuppressDisposal = suppressDisposal;
		}

		/// <summary>
		/// Deconstructs the instance of <see cref="TempFolder"/>.
		/// </summary>
		~TempFolder()
		{
			if (!this.SuppressDisposal)
			{
				this.Dispose();
			}
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets a value indicating whether to suppress the
		/// disposal of the current temporary folder on object destruction.
		/// </summary>
		public bool SuppressDisposal { get; set; }
		#endregion

		#region Methods
		/// <summary>
		/// Creates an instance of <see cref="TempFolder"/>.
		/// The temporary folder is created within the user %temp% directory.
		/// </summary>
		/// <returns>
		/// Returns the <see cref="TempFolder"/> object representing the temporary directory.
		/// </returns>
		public static TempFolder Create()
		{
			//Get a temporary location.
			string tempFolder = TempFolder.GetNewPath();

			return new TempFolder(tempFolder);
		}

		/// <summary>
		/// Generates a filepath to a non-existant temporary folder.
		/// </summary>
		/// <returns>
		/// Returns a filepath to a non-existant temporary folder.
		/// </returns>
		public static string GetNewPath()
		{
			//Get a temporary location.
			string tempFolder = Path.GetTempFileName();
			File.Delete(tempFolder);

			return tempFolder;
		}
		#endregion

		#region IDisposable Members
		/// <summary>
		/// Recursively deletes the temporary directory from the system.
		/// </summary>
		public void Dispose()
		{
			if (Directory.Exists(FullName))
			{
				Directory.Delete(FullName, true);
			}
		}
		#endregion
	}
}
