// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Win32.Tests
{
	[TestClass()]
	public class ExtensionsTests
	{
		[TestMethod()]
		public void GetValueOrEmptyStringTest()
		{
			string regValue1 = null;
			string regValue2 = null;

			try
			{
				using (var hklm = Registry.LocalMachine)
				{
					using (var key = hklm.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion"))
					{
						try
						{
							regValue1 = key.GetValueOrEmptyString("ProductName");
						}
						catch { }
						try
						{
							regValue2 = key.GetValueOrEmptyString("jkadshfkjshzdkxrhfklzdj");
						}
						catch { }
					}
				}
			}
			catch { }

			Assert.IsFalse(string.IsNullOrWhiteSpace(regValue1), "ProductName value should have been found and return and not empty.");
			Assert.IsTrue(string.IsNullOrWhiteSpace(regValue2), "jkadshfkjshzdkxrhfklzdj value should not exist.");
		}
	}
}