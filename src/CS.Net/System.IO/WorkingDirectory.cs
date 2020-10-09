// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

namespace System.IO
{
	/// <summary>
	/// Provides methods for quickly setting then resetting
	/// the current working directory, utilizing the Disposable Pattern.
	/// </summary>
	public class WorkingDirectory : Disposable
	{
		#region Constructor
		/// <summary>
		/// Creates a new instance of <see cref="WorkingDirectory"/> and
		/// sets the current working directory to the specified path.
		/// </summary>
		/// <param name="path">The path to use for the current working directory.</param>
		public WorkingDirectory(string path)
		{
			PreviousFolder = Directory.GetCurrentDirectory();
			NewFolder = path;

			Directory.SetCurrentDirectory(NewFolder);
		}
		#endregion

		#region Fields
		private readonly string PreviousFolder;
		private readonly string NewFolder;
		#endregion

		#region Methods
		/// <summary>
		/// Sets the current working directory to the specified path.
		/// This is an alias for the constructor.
		/// </summary>
		/// <param name="path">The path to use for the current working directory.</param>
		/// <returns>Returns a new instance of <see cref="WorkingDirectory"/>.</returns>
		public static WorkingDirectory Set(string path)
		{
			return new WorkingDirectory(path);
		}

		/// <summary>
		/// Resets the current working directory to the previous value.
		/// </summary>
		protected override void FreeManagedResources()
		{
			Directory.SetCurrentDirectory(PreviousFolder);
		}
		#endregion
	}
}
