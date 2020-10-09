// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.Globalization;
using System.Windows.Data;

namespace System.Windows
{
	public class CultureInfoToFlowDirectionConverter : ValueConverter, IValueConverter
	{
		public static FlowDirection DefaultFlowDirection = FlowDirection.LeftToRight;

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

		public static FlowDirection Convert(CultureInfo cultureInfo)
		{
			return cultureInfo.TextInfo.IsRightToLeft ?
				FlowDirection.RightToLeft :
				FlowDirection.LeftToRight;
		}
	}
}
