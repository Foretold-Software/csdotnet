// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace System.Globalization.Tests
{
	[TestClass]
	public partial class CultureInfoTest
	{
		[TestMethod]
		public void TryGetCultureTest_en_US()
		{
			CultureInfo cultureInfo;

			var returnValue = _CultureInfo.TryGetCulture("en-US", out cultureInfo);

			Assert.IsTrue(returnValue);
			Assert.IsNotNull(cultureInfo);
			Assert.AreEqual("en-US", cultureInfo.Name);
		}

		[TestMethod]
		public void TryGetCultureTest_fr_FR()
		{
			CultureInfo cultureInfo;

			var returnValue = _CultureInfo.TryGetCulture("fr-FR", out cultureInfo);

			Assert.IsTrue(returnValue);
			Assert.IsNotNull(cultureInfo);
			Assert.AreEqual("fr-FR", cultureInfo.Name);
		}

		[TestMethod]
		public void TryGetCultureTest_UnknownCulture()
		{
			CultureInfo cultureInfo;

			var returnValue = _CultureInfo.TryGetCulture("ab-CD", out cultureInfo);

			Assert.IsTrue(returnValue);
			Assert.IsNotNull(cultureInfo);
		}

		[TestMethod]
		public void TryGetCultureTest_InvalidName()
		{
			CultureInfo cultureInfo;

			var returnValue = _CultureInfo.TryGetCulture("ab-CD2", out cultureInfo);

			Assert.IsFalse(returnValue);
			Assert.IsNull(cultureInfo);
		}

		[TestMethod]
		public void TryGetCultureTest_EmptyString()
		{
			CultureInfo cultureInfo;

			var returnValue = _CultureInfo.TryGetCulture("", out cultureInfo);

			Assert.IsTrue(returnValue);
			Assert.IsNotNull(cultureInfo);
			Assert.AreEqual(CultureInfo.InvariantCulture, cultureInfo);
		}

		[TestMethod]
		public void TryGetSpecificCultureTest_en_US()
		{
			CultureInfo cultureInfo;

			var returnValue = _CultureInfo.TryGetSpecificCulture("en-US", out cultureInfo);

			Assert.IsTrue(returnValue);
			Assert.IsNotNull(cultureInfo);
			Assert.AreEqual("en-US", cultureInfo.Name);
		}

		[TestMethod]
		public void TryGetSpecificCultureTest_fr_FR()
		{
			CultureInfo cultureInfo;

			var returnValue = _CultureInfo.TryGetSpecificCulture("fr-FR", out cultureInfo);

			Assert.IsTrue(returnValue);
			Assert.IsNotNull(cultureInfo);
			Assert.AreEqual("fr-FR", cultureInfo.Name);
		}

		[TestMethod]
		public void TryGetSpecificCultureTest_UnknownCulture()
		{
			CultureInfo cultureInfo;

			var returnValue = _CultureInfo.TryGetSpecificCulture("ab-CD2", out cultureInfo);

			Assert.IsFalse(returnValue);
			Assert.IsNull(cultureInfo);
		}

		[TestMethod]
		public void TryGetSpecificCultureTest_InvalidName()
		{
			CultureInfo cultureInfo;

			var returnValue = _CultureInfo.TryGetSpecificCulture("ab-CD", out cultureInfo);

			Assert.IsTrue(returnValue);
			Assert.IsNotNull(cultureInfo);
		}

		[TestMethod]
		public void TryGetSpecificCultureTest_EmptyString()
		{
			CultureInfo cultureInfo;

			var returnValue = _CultureInfo.TryGetSpecificCulture("", out cultureInfo);

			Assert.IsFalse(returnValue);
			Assert.IsNull(cultureInfo);
		}

		[TestMethod]
		public void GetFallbackCulturesTest_en_US()
		{
			string expected, actual;

			var fallback = _CultureInfo.GetFallbackCultures("en-US");

			Assert.AreEqual(3, fallback.Length);

			expected = "en-US";
			actual = fallback[0].Name;
			Assert.AreEqual(expected, actual);

			expected = "en";
			actual = fallback[1].Name;
			Assert.AreEqual(expected, actual);

			Assert.AreEqual(CultureInfo.InvariantCulture, fallback[2]);
		}

		[TestMethod]
		public void GetFallbackCulturesTest_fr_FR()
		{
			string expected, actual;

			var fallback = _CultureInfo.GetFallbackCultures("fr-FR");

			Assert.AreEqual(3, fallback.Length);

			expected = "fr-FR";
			actual = fallback[0].Name;
			Assert.AreEqual(expected, actual);

			expected = "fr";
			actual = fallback[1].Name;
			Assert.AreEqual(expected, actual);

			Assert.AreEqual(CultureInfo.InvariantCulture, fallback[2]);
		}

		[TestMethod]
		public void GetFallbackCulturesTest_ar_SA()
		{
			string expected, actual;

			var fallback = _CultureInfo.GetFallbackCultures("ar-SA");

			Assert.AreEqual(3, fallback.Length);

			expected = "ar-SA";
			actual = fallback[0].Name;
			Assert.AreEqual(expected, actual);

			expected = "ar";
			actual = fallback[1].Name;
			Assert.AreEqual(expected, actual);

			Assert.AreEqual(CultureInfo.InvariantCulture, fallback[2]);
		}

		[TestMethod]
		public void GetFallbackCulturesTest_zh_TW()
		{
			var expectedFallback = new CultureInfo[]
			{
				new CultureInfo("zh-TW"),
#if !NET5_0
				new CultureInfo("zh-CHT"),
#endif
				new CultureInfo("zh-Hant"),
				new CultureInfo("zh"),
				CultureInfo.InvariantCulture
			};

			var actualFallback = _CultureInfo.GetFallbackCultures("zh-TW");

			Assert.AreEqual(expectedFallback.Length, actualFallback.Length);

			for (int i = 0; i < expectedFallback.Length; i++)
			{
				Assert.AreEqual(expectedFallback[i], actualFallback[i]);
			}
		}
	}
}