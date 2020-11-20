// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.Globalization;

namespace System.Windows.Data
{
	/// <summary>
	/// Provides a base class for implementations of <see cref="IValueConverter"/>.
	/// </summary>
	public class ValueConverter : IValueConverter
	{
		/// <summary>
		/// When derived in a child class, converts a value.
		/// </summary>
		/// <param name="value">The value produced by the binding source.</param>
		/// <param name="targetType">The type of the binding target property.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>
		/// A converted value. If the method returns null, the valid null value is used.
		/// </returns>
		public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}

		/// <summary>
		/// When derived in a child class, converts a value.
		/// </summary>
		/// <param name="value">The value that is produced by the binding target.</param>
		/// <param name="targetType">The type to convert to.</param>
		/// <param name="parameter">The converter parameter to use.</param>
		/// <param name="culture">The culture to use in the converter.</param>
		/// <returns>
		/// A converted value. If the method returns null, the valid null value is used.
		/// </returns>
		public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}
