// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.Globalization;

namespace System.Windows.Data
{
	/// <summary>
	/// Provides a parent class for a converter that only needs to convert one-way.
	/// </summary>
	public abstract class OneWayValueConverter : IValueConverter
	{
		/// <summary>
		/// Converts a value.
		/// </summary>
		/// <param name="value">The value produced by the binding source.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>
		/// A converted value. If the method returns null, the valid null value is used.
		/// </returns>
		public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

		/// <summary>
		/// This method does nothing.
		/// Since this class only converts one-way, the binding target will be returned.
		/// </summary>
		/// <param name="value">The value produced by the binding target.</param>
		/// <param name="targetType">N/A</param>
		/// <param name="parameter">N/A</param>
		/// <param name="culture">N/A</param>
		/// <returns>
		/// The binding target is returned. That is, the same <paramref name="value"/> object
		/// that was passed into this method is returned by it.
		/// </returns>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}
	}
}
