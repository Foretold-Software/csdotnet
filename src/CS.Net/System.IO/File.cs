// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

namespace System.IO
{
	public static class _File
	{
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
