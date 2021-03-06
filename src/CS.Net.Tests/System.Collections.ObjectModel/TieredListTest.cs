// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Collections.ObjectModel.Tests
{
	[TestClass]
	public partial class TieredListTest
	{
		[TestMethod]
		public void TieredListToStringTest()
		{
			var list = new TieredList<ValueString>()
			{
				new TieredListItem<ValueString>("T0-0"),
				new TieredListItem<ValueString>("T0-1"),
				new TieredListItem<ValueString>("T0-2")
				{
					new TieredListItem<ValueString>("T1-0"),
					new TieredListItem<ValueString>("T1-1")
					{
						new TieredListItem<ValueString>("T2-0"),
						new TieredListItem<ValueString>("T2-1")
					},
					new TieredListItem<ValueString>("T1-2")
				},
				new TieredListItem<ValueString>("T0-3")
			};

			Assert.AreEqual("T0-2", list[2].Item.Value.ToString());
			Assert.AreEqual("T2-0", list[2][1][0].Item.Value.ToString());
			Assert.AreEqual("T2-1", list[2][1][1].Item.Value.ToString());
		}
	}
}
