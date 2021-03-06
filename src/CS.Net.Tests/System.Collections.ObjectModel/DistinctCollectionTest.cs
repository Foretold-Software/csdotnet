// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Collections.ObjectModel.Tests
{
	[TestClass]
	public partial class DistinctCollectionTest
	{
		[TestMethod]
		public void AddItemTest()
		{
			var coll = new DistinctCollection<string>();

			coll.Add("V1");
			coll.Add("V2");
			coll.Add("V3");
			coll.Add("V4");
			coll.Add("V2");
			coll.Add("V3");
			coll.Add("V4");

			Assert.IsTrue(coll.Count == 4);
		}

		[TestMethod]
		public void InsertItemTest()
		{
			var coll = new DistinctCollection<string>();

			coll.Add("V1");
			coll.Add("V2");
			coll.Add("V3");
			coll.Add("V4");
			coll.Insert(0, "V2");
			coll.Insert(1, "V1");
			coll.Insert(2, "V4");

			Assert.IsTrue(coll.Count == 4);

			coll[0] = "V2";
			coll[1] = "V1";
			coll[2] = "V4";

			Assert.IsTrue(coll.Count == 4);

		}
	}
}
