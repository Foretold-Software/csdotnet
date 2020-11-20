// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.Globalization;
using System.Windows.Data;

namespace System.Drawing
{
	public class BitmapSourceConverter : OneWayValueConverter, IValueConverter
	{
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var bitmap = value as Bitmap;

			return bitmap.ToBitmapSource();
		}
	}
}
