// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.Globalization;

namespace System.Windows.Data
{
	public class ValueConverter : IValueConverter
	{
		public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}

		public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}
