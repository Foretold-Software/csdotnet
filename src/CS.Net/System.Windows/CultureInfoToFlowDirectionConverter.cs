// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.Globalization;
using System.Windows.Data;

namespace System.Windows
{
	/// <summary>
	/// A value converter class to convert the current UI thread culture
	/// into a <see cref="FlowDirection"/> enumeration value, depending on
	/// whether that culture using a RTL or LTR flow direction.
	/// </summary>
	public class CultureInfoToFlowDirectionConverter : OneWayValueConverter, IValueConverter
	{
		/// <summary>
		/// The default UI flow direction.
		/// </summary>
		public static FlowDirection DefaultFlowDirection = FlowDirection.LeftToRight;

		/// <summary>
		/// Converts a <see cref="CultureInfo"/> value to a <see cref="FlowDirection"/> enumeration value.
		/// </summary>
		/// <param name="value">
		/// The <see cref="CultureInfo"/> object for the UI thread.
		/// </param>
		/// <param name="targetType">
		/// This parameter is not used.
		/// </param>
		/// <param name="parameter">
		/// A <see cref="CultureInfo"/> object, used as a backup to <paramref name="value"/>.
		/// </param>
		/// <param name="culture">
		/// A <see cref="CultureInfo"/> object, used as a backup to <paramref name="value"/>.
		/// </param>
		/// <returns>
		/// Returns the flow direction associated with the given culture.
		/// </returns>
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			try
			{
				CultureInfo cultureInfo = null;

				if (value != null && value is CultureInfo)
				{
					return Convert(value as CultureInfo);
				}
				else if (_CultureInfo.TryGetSpecificCulture(parameter as string, out cultureInfo))
				{
					return Convert(cultureInfo);
				}
				else if (culture != null)
				{
					return Convert(culture);
				}
			}
			catch { }

			return DefaultFlowDirection;
		}

		/// <summary>
		/// Converts a <see cref="CultureInfo"/> value to a <see cref="FlowDirection"/> enumeration value.
		/// </summary>
		/// <param name="cultureInfo">
		/// The <see cref="CultureInfo"/> object for the UI thread.
		/// </param>
		/// <returns>
		/// Returns the flow direction associated with the given culture.
		/// </returns>
		public static FlowDirection Convert(CultureInfo cultureInfo)
		{
			return cultureInfo.TextInfo.IsRightToLeft ?
				FlowDirection.RightToLeft :
				FlowDirection.LeftToRight;
		}
	}
}
