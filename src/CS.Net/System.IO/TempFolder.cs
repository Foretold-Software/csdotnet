// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

namespace System.IO
{
	public class TempFolder : Folder, IDisposable
	{
		#region Constructor / Destructor
		private TempFolder(string path) : base(path)
		{
			this.SuppressDisposal = false;
		}
		private TempFolder(string path, bool suppressDisposal) : base(path)
		{
			this.SuppressDisposal = suppressDisposal;
		}
		~TempFolder()
		{
			if (!this.SuppressDisposal)
			{
				this.Dispose(false);
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
		public static TempFolder Create()
		{
			//Get a temporary location.
			string tempFolder = TempFolder.GetNewPath();

			//Log.WriteLine(LogLevel.Verbose, "Created new temporary folder: '{0}'", tempFolder);

			return new TempFolder(tempFolder);
		}

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
			this.Dispose(true);
		}

		private void Dispose(bool disposing)
		{
			if (Directory.Exists(this.FullName))
			{
				Directory.Delete(this.FullName, true);
				//if (disposing) Log.WriteLine("Temporary folder deleted: " + this.FullName);
			}
			//else if (disposing)
			//{
			//	Log.WriteLine("Temporary folder already deleted, ignoring: " + this.FullName);
			//}
		}
		#endregion
	}
}
