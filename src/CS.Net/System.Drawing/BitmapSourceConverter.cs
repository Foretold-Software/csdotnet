// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.Globalization;
using System.Windows.Data;

namespace System.Drawing
{
	/// <summary>
	/// A converter class used to convert an object of type <see cref="Bitmap"/> to an instance of <see cref="System.Windows.Media.Imaging.BitmapSource"/>.
	/// </summary>
	public class BitmapSourceConverter : OneWayValueConverter, IValueConverter
	{
		/// <summary>
		/// Converts an object of type <see cref="Bitmap"/> to a <see cref="System.Windows.Media.Imaging.BitmapSource"/>.
		/// </summary>
		/// <param name="value">
		/// The <see cref="Bitmap"/> object to convert.
		/// </param>
		/// <param name="targetType">
		/// This parameter is not used.
		/// </param>
		/// <param name="parameter">
		/// This parameter is not used.
		/// </param>
		/// <param name="culture">
		/// This parameter is not used.
		/// </param>
		/// <returns>
		/// Returns the converted value.
		/// If null is returned, the valid null value is used.
		/// </returns>
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var bitmap = value as Bitmap;

			return bitmap.ToBitmapSource();
		}
	}
}
