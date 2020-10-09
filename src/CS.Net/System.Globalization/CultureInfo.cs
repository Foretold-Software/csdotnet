// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.Collections.Generic;

namespace System.Globalization
{
	/// <summary>
	/// Provides methods to simplify the use of the <see cref="CultureInfo"/> class.
	/// </summary>
	public static class _CultureInfo
	{
		/// <summary>
		/// Attempts to create an instance of <see cref="CultureInfo"/> using the specified name.
		/// </summary>
		/// <param name="name">The name of the culture to create.</param>
		/// <param name="cultureInfo">
		/// The instance of <see cref="CultureInfo"/> being returned.
		/// </param>
		/// <returns>
		/// Returns true if an instance of <see cref="CultureInfo"/> was created successfully.
		/// Otherwise, returns falls and the <paramref name="cultureInfo"/> is set to null.
		/// </returns>
		public static bool TryGetCulture(string name, out CultureInfo cultureInfo)
		{
			try
			{
				if (name != null)
				{
					cultureInfo = CultureInfo.GetCultureInfo(name);
					return true;
				}
			}
			catch { }

			cultureInfo = null;
			return false;
		}

		/// <summary>
		/// Attempts to create an instance of <see cref="CultureInfo"/> using the specified name,
		/// excluding the invariant culture.
		/// </summary>
		/// <param name="name">The name of the culture to create.</param>
		/// <param name="cultureInfo">
		/// The instance of <see cref="CultureInfo"/> being returned.
		/// </param>
		/// <returns>
		/// Returns true if an instance of <see cref="CultureInfo"/> was created successfully.
		/// Otherwise, returns falls and the <paramref name="cultureInfo"/> is set to null.
		/// </returns>
		public static bool TryGetSpecificCulture(string name, out CultureInfo cultureInfo)
		{
			try
			{
				if (!name.IsBlank())
				{
					cultureInfo = CultureInfo.GetCultureInfo(name);
					return true;
				}
			}
			catch { }

			cultureInfo = null;
			return false;
		}

		/// <summary>
		/// Creates an array of <see cref="CultureInfo"/> objects representing
		/// the culture fallback hierarchy, starting with the value of the
		/// <see cref="CultureInfo.CurrentUICulture"/> property.
		/// </summary>
		/// <returns>
		/// Returns an arrary of <see cref="CultureInfo"/> objects representing
		/// the culture fallback hierarchy, starting with the value of the
		/// <see cref="CultureInfo.CurrentUICulture"/> property.
		/// </returns>
		public static CultureInfo[] GetFallbackCultures()
		{
			return GetFallbackCultures(CultureInfo.CurrentUICulture);
		}

		/// <summary>
		/// Creates an array of <see cref="CultureInfo"/> objects representing
		/// the culture fallback hierarchy, starting with the given culture info.
		/// </summary>
		/// <param name="cultureName">The name of the culture info to query.</param>
		/// <returns>
		/// Returns an arrary of <see cref="CultureInfo"/> objects representing
		/// the culture fallback hierarchy, starting with the given culture info.
		/// </returns>
		public static CultureInfo[] GetFallbackCultures(string cultureName)
		{
			return GetFallbackCultures(CultureInfo.GetCultureInfo(cultureName));
		}

		/// <summary>
		/// Creates an array of <see cref="CultureInfo"/> objects representing
		/// the culture fallback hierarchy, starting with the given culture info.
		/// </summary>
		/// <param name="cultureInfo">The culture info to query.</param>
		/// <returns>
		/// Returns an arrary of <see cref="CultureInfo"/> objects representing
		/// the culture fallback hierarchy, starting with the given culture info.
		/// </returns>
		public static CultureInfo[] GetFallbackCultures(this CultureInfo cultureInfo)
		{
			CultureInfo info = cultureInfo;
			var cultures = new List<CultureInfo>();

			do
			{
				if (!cultures.Contains(info))
				{
					cultures.Add(info);
				}
				else break;

			} while (null != (info = info.Parent));

			//If the invariant culture is not yet in the list, add it.
			if (!cultures.Contains(CultureInfo.InvariantCulture))
			{
				cultures.Add(CultureInfo.InvariantCulture);
			}

			return cultures.ToArray();
		}
	}
}
