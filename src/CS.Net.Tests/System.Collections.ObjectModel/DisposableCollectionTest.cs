// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace System.Collections.ObjectModel.Tests
{
	[TestClass]
	public partial class DisposableCollectionTest
	{
		[TestMethod]
		public void AddItemTest()
		{
			var collection = new DisposableCollection<TempFolder>();

			using (collection)
			{
				collection.Add(TempFolder.Create());
				collection.Add(TempFolder.Create());
				collection.Add(TempFolder.Create());
				collection.Add(TempFolder.Create());
				collection.Add(TempFolder.Create());
				collection.Add(TempFolder.Create());

				Assert.IsTrue(collection.Count == 6);

				foreach (var item in collection)
				{
					Assert.IsTrue(Directory.Exists(item.FullName));
				}
			}
		}

		[TestMethod]
		public void DisposeTest()
		{
			var collection = new DisposableCollection<TempFolder>();

			using (collection)
			{
				collection.Add(TempFolder.Create());
				collection.Add(TempFolder.Create());
				collection.Add(TempFolder.Create());
				collection.Add(TempFolder.Create());
				collection.Add(TempFolder.Create());
				collection.Add(TempFolder.Create());

				foreach (var item in collection)
				{
					Assert.IsTrue(Directory.Exists(item.FullName));
				}
			}

			Assert.IsTrue(collection.Count == 6);

			foreach (var item in collection)
			{
				Assert.IsFalse(Directory.Exists(item.FullName));
			}
		}
	}
}
