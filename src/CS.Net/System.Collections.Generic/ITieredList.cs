// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

namespace System.Collections.Generic
{
	/// <summary>
	/// Provides members for implementing a tiered list of items.
	/// </summary>
	/// <typeparam name="T">
	/// The type of the item at each node in the tiered list.
	/// </typeparam>
	public interface ITieredList<T>
		: IList<ITieredListItem<T>>, ICollection<ITieredListItem<T>>, IEnumerable<ITieredListItem<T>>
		, IEnumerable
	{
	}
}
