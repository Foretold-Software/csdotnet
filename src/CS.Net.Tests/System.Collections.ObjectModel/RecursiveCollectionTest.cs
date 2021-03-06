// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Collections.ObjectModel.Tests
{
	[TestClass]
	public partial class RecursiveCollectionTest
	{
		[TestMethod]
		public void AddChildrenTest()
		{
			var coll = new RecursiveCollection<string>("V0", "V0-0", "V0-1", "V0-2");
			coll.Add("V0-3");

			Assert.IsTrue(coll.Value == "V0");
			Assert.IsTrue(coll[1].Value == "V0-1");

			coll[1].Add("V0-1-0");
			coll[1].Add("V0-1-1");
			coll[1].Add("V0-1-2");

			Assert.IsTrue(coll[1][2].Value == "V0-1-2");

			coll[2].AddRange(
				new RecursiveCollection<string>("V0-2-0"),
				new RecursiveCollection<string>("V0-2-1"));

			coll[2].AddRange(new[] {
				new RecursiveCollection<string>("V0-2-2"),
				new RecursiveCollection<string>("V0-2-3")
			});

			Assert.IsTrue(coll[2].Value == "V0-2");
			Assert.IsTrue(coll[2][2].Value == "V0-2-2");
		}
	}
}
