// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

namespace System.Collections
{
	/// <summary>
	/// Provides members for implementing a tiered list of items.
	/// </summary>
	public interface ITieredListItem : ITieredList, IList, ICollection, IEnumerable
	{
		/// <summary>
		/// The items that are tiered children of the item at the current node.
		/// </summary>
		ITieredList Children { get; }

		/// <summary>
		/// The item at the current node in the tiered list.
		/// </summary>
		object Item { get; set; }

		/// <summary>
		/// The item that is the tiered parent of the item at the current node.
		/// </summary>
		ITieredListItem Parent { get; set; }
	}
}
