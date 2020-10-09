// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace System.Windows
{
	/// <summary>
	/// Converts a collection or its item count to <see cref="Visibility"/> enumeration values.
	/// </summary>
	/// <remarks>
	/// This class can be used to determine whether to display UI elements, based on the state of a collection.
	/// For example:
	/// If the 'parameter' argument is a string containing "Any", then
	/// the <see cref="Convert(object, Type, object, CultureInfo)"/> method
	/// will return true is the collection contains any items, and false otherwise.
	/// </remarks>
	public class CollectionToVisibilityConverter : ValueConverter, IValueConverter
	{
		/// <summary>
		/// Converts a collection or its item count to a <see cref="Visibility"/> enumeration value.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter">A string value representing the conversion function to apply.</param>
		/// <param name="culture"></param>
		/// <returns></returns>
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool visible;

			//Check that 'value' is not null.
			if (value == null)
			{
				throw new ArgumentNullException("value", "A valid value must be provided.");
			}

			//Check that 'parameter' is a valid value.
			if (parameter == null)
			{
				throw new ArgumentNullException("parameter", "A valid converter parameter must be provided.");
			}
			else if (!(parameter is string))
			{
				throw new ArgumentException("parameter", "The converter parameter must be a string value.");
			}
			else
			{
				switch (parameter as string)
				{
					case "Any":
						break;
					//Can add other parameter values here.
					//case "SomeOtherString": break;
					default:
						throw new ArgumentException("parameter", "The converter parameter must be a supported value.");
				}
			}

			//Check that 'targetType' is a valid value.
			if (targetType != typeof(Visibility))
			{
				throw new ArgumentException("targetType", "The target type must be System.Windows.Visibility.");
			}

			//Check that 'value' is a valid value and convert it.
			if (value is int)
			{
				visible = Convert((int)value, parameter as string);
			}
			else if (value is ICollection)
			{
				visible = Convert(value as ICollection, parameter as string);
			}
			else if (value is IEnumerable)
			{
				visible = Convert(value as IEnumerable, parameter as string);
			}
			else
			{
				throw new ArgumentException("value", "The value to be converted is not of a supported type.");
			}


			return visible ? Visibility.Visible : Visibility.Collapsed;
		}

		/// <summary>
		/// Converts a collection's item count to a <see cref="Visibility"/> enumeration value.
		/// </summary>
		/// <param name="count">The collection's item count.</param>
		/// <param name="parameter">The conversion function to apply.</param>
		/// <returns>Return true if the value passes the conversion function's test, otherwise false.</returns>
		private bool Convert(int count, string parameter)
		{
			switch (parameter)
			{
				case "Any":
					return count > 0;
				default:
					throw ParameterNotSupportedForType(parameter, typeof(int));
			}
		}

		/// <summary>
		/// Converts a collection to a <see cref="Visibility"/> enumeration value.
		/// </summary>
		/// <param name="collection">The collection to convert.</param>
		/// <param name="parameter">The conversion function to apply.</param>
		/// <returns>Return true if the value passes the conversion function's test, otherwise false.</returns>
		private bool Convert(ICollection collection, string parameter)
		{
			switch (parameter)
			{
				case "Any":
					return collection.Count > 0;
				default:
					throw ParameterNotSupportedForType(parameter, typeof(ICollection));
			}
		}

		/// <summary>
		/// Converts a collection to a <see cref="Visibility"/> enumeration value.
		/// </summary>
		/// <param name="collection">The collection to convert.</param>
		/// <param name="parameter">The conversion function to apply.</param>
		/// <returns>Return true if the value passes the conversion function's test, otherwise false.</returns>
		private bool Convert(IEnumerable collection, string parameter)
		{
			switch (parameter)
			{
				case "Any":
					foreach (var item in collection)
					{
						return true;
					}
					return false;
				default:
					throw ParameterNotSupportedForType(parameter, typeof(IEnumerable));
			}
		}

		/// <summary>
		/// Helper method to keep the code short and readable.
		/// </summary>
		/// <param name="parameter">The conversion function string.</param>
		/// <param name="type">The collection's type.</param>
		/// <returns>Returns an exception to throw, with a formatted message.</returns>
		private ArgumentException ParameterNotSupportedForType(string parameter, Type type)
		{
			return new ArgumentException("parameter", string.Format("The converter parameter value '{0}' is not supported for the type '{1}'.", parameter, type));
		}
	}
}
