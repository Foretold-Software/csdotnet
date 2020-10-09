// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

namespace System.Collections.Generic
{
	/// <summary>
	/// Provides members for implementing a tiered list of items.
	/// </summary>
	/// <typeparam name="T">
	/// The type of the item at each node in the tiered list.
	/// </typeparam>
	public interface ITieredListItem<T>
		: IList<ITieredListItem<T>>, ICollection<ITieredListItem<T>>, IEnumerable<ITieredListItem<T>>
		, IEnumerable
	{
		///// <summary>
		///// The items that are tiered children of the item at the current node.
		///// </summary>
		//ITieredList<ITieredListItem<T>> Children { get; }

		/// <summary>
		/// The item at the current node in the tiered list.
		/// </summary>
		T Item { get; set; }

		/// <summary>
		/// The item that is the tiered parent of the item at the current node.
		/// </summary>
		ITieredListItem<T> Parent { get; set; }
	}
}

