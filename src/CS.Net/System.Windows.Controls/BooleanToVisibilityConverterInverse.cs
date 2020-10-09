// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.Globalization;
using System.Windows.Data;

namespace System.Windows.Controls
{
	/// <summary>
	/// Converts <see cref="bool"/> values to and from <see cref="Visibility"/> enumeration values.
	/// </summary>
	public class BooleanToVisibilityConverterInverse : IValueConverter
	{
		/// <summary>
		/// Creates a new instance of <see cref="BooleanToVisibilityConverterInverse"/>.
		/// </summary>
		public BooleanToVisibilityConverterInverse()
		{
			converter = new BooleanToVisibilityConverter();
		}

		private readonly BooleanToVisibilityConverter converter;

		/// <summary>
		/// Converts a <see cref="bool"/> value to a <see cref="Visibility"/> enumeration value.
		/// </summary>
		/// <param name="value">The <see cref="bool"/> value to convert. This can also be a <see cref="Nullable{Boolean}"/> value.</param>
		/// <param name="targetType">This parameter is not used.</param>
		/// <param name="parameter">This parameter is not used.</param>
		/// <param name="culture">This parameter is not used.</param>
		/// <returns>Returns <see cref="Visibility.Collapsed"/> if the value is true, otherwise <see cref="Visibility.Visible"/>.</returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is bool?)
			{
				if (true == (bool?)value)
				{
					value = false;
				}
				else value = true;

				//var inverseValue = ((bool?)value).HasValue ? !((bool)value);
				return converter.Convert(value, targetType, parameter, culture);
			}
			else if (value is bool)
			{
				var inverseValue = !((bool)value);
				return converter.Convert(inverseValue, targetType, parameter, culture);
			}
			else
			{
				throw new InvalidOperationException("The value parameter must be of type System.Boolean.");
			}
        }

		/// <summary>
		/// Converts a <see cref="Visibility"/> enumeration value to a <see cref="bool"/> value.
		/// </summary>
		/// <param name="value">The <see cref="Visibility"/> enumeration value to convert.</param>
		/// <param name="targetType">This parameter is not used.</param>
		/// <param name="parameter">This parameter is not used.</param>
		/// <param name="culture">This parameter is not used.</param>
		/// <returns>Returns false if the value is <see cref="Visibility.Visible"/>, otherwise true.</returns>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value is Visibility)
			{
				return !((bool)converter.ConvertBack(value, targetType, parameter, culture));
			}
			else
			{
				throw new InvalidOperationException("The value parameter must be of type System.Windows.Visibility.");
			}
		}
	}
}
