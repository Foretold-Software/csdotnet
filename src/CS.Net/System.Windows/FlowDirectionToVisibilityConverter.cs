// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace System.Windows
{
	public class FlowDirectionToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			try
			{
				FlowDirection? desiredDirection = null;

				//Find the desired FlowDirection.
				if (parameter != null && parameter is string)
				{
					try
					{
						desiredDirection = (FlowDirection)Enum.Parse(typeof(FlowDirection), parameter as string, true);
					}
					catch { }
				}

				//If the desired FlowDirection was not found, then throw an exception.
				if (!desiredDirection.HasValue)
				{
					throw new ArgumentException("The ConverterParameter must be a string representing a valid FlowDirection value.", "parameter");
				}


				//If the value represents an Image, then get its visual parent.
				while (value != null && value is Image)
				{
					try
					{
						value = VisualTreeHelper.GetParent(value as Image);
					}
					catch { value = null; }
				}

				//If the value is a FrameworkElement, then get its FlowDirection.
				if (value is FrameworkElement)
				{
					value = (value as FrameworkElement).FlowDirection;
				}

				//If the value is null, throw an exception.
				if (value == null)
				{
					throw new ArgumentException("The value to be converted must be a valid instance of FlowDirection or FrameworkElement.", "value");

					//TODO: Instead of throwing an exception, will the following line of code work as expected???
					//value = System.Threading.Thread.CurrentThread.CurrentUICulture.TextInfo.IsRightToLeft ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
				}

				//If the value is a FlowDirection, then find out if it matches the desired FlowDirection.
				if (value is FlowDirection)
				{
					return desiredDirection == (FlowDirection)value ? Visibility.Visible : Visibility.Collapsed;
				}
			}
			catch { }

			return Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
