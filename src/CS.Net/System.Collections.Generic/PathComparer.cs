// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

namespace System.Collections.Generic
{
	/// <summary>
	/// A class that implements the <see cref="IEqualityComparer{T}"/>
	/// interface to compare strings that represent file and folder paths.
	/// </summary>
	public class PathComparer : IEqualityComparer<string>
	{
		/// <summary>
		/// Determines whether the two file paths are equivalent.
		/// </summary>
		/// <param name="path1">The first path to compare.</param>
		/// <param name="path2">The second path to compare.</param>
		/// <returns>A boolean value indicating whether the paths are equivalent.</returns>
		public static bool IsEquivalent(string path1, string path2)
		{
			if (object.ReferenceEquals(path1, path2)) return true;

			if (path1 == null || path2 == null) return false;

			return path1.Equals(path2, StringComparison.OrdinalIgnoreCase);
		}

		#region IEqualityComparer<string> Members
		/// <summary>
		/// Determines whether the specified objects are equal.
		/// </summary>
		/// <param name="x">The first path to compare.</param>
		/// <param name="y">The second path to compare.</param>
		/// <returns>A boolean value indicating whether the paths are equivalent.</returns>
		public bool Equals(string x, string y)
		{
			return IsEquivalent(x, y);
		}

		/// <summary>
		/// Returns a hash code for the specified object.
		/// </summary>
		/// <param name="obj">The <see cref="string"/> for which a hash code is to be returned.</param>
		/// <returns>A hash code for the specified object.</returns>
		public int GetHashCode(string obj)
		{
			//TODO: Should I change this to the following, since case is ignore in this class's comparisons?
			//        return obj.ToLower().GetHashCode();
			return obj.GetHashCode();
		}
		#endregion
	}
}
