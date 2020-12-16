// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.ComponentModel;

namespace System.Windows
{
	/// <summary>
	/// A static class containing extension methods for classes within the <see cref="System.Windows"/> namespace.
	/// </summary>
	public static class Extensions
	{
		#region Private Delegates
		private delegate object DependencyObjectGetValueDelegate(DependencyProperty property);
		private delegate void DependencyObjectSetValueDelegate(DependencyProperty property, object value);
		#endregion

		#region DependencyObject Extensions
		/// <summary>
		/// Gets the value of the <see cref="DesignerProperties.IsInDesignModeProperty"/> attached property for the specified <see cref="DependencyObject"/>.
		/// </summary>
		/// <param name="dependencyObject">The element from which the property value is read.</param>
		/// <returns>The <see cref="DesignerProperties.IsInDesignModeProperty"/> value for the element.</returns>
		public static bool GetIsInDesignMode(this DependencyObject dependencyObject)
		{
			return DesignerProperties.GetIsInDesignMode(dependencyObject);
		}

		/// <summary>
		/// Returns the current effective value of a dependency property
		/// on this instance of a <see cref="DependencyObject"/>.
		/// This method is safe to run on a thread other than the UI thread.
		/// </summary>
		/// <param name="dependencyObject">
		/// The <see cref="DependencyObject"/> whose <see cref="DependencyProperty"/> is to be retrieved.
		/// </param>
		/// <param name="property">
		/// The <see cref="DependencyProperty"/> identifier of the dependency property to retrieve the value for.
		/// </param>
		/// <returns>Returns the current effective value.</returns>
		public static object GetValueThreadSafe(this DependencyObject dependencyObject, DependencyProperty property)
		{
			try
			{
				//If this code is running of the correct thread...
				if (dependencyObject.Dispatcher.CheckAccess())
				{
					//...then get the value.
					return dependencyObject.GetValue(property);
				}
				else
				{
					//...otherwise, invoke the value to be gotten on the correct thread.
					return dependencyObject.Dispatcher.Invoke((DependencyObjectGetValueDelegate)dependencyObject.GetValue, property);
				}
			}
			catch
			{ return property.DefaultMetadata.DefaultValue; }
		}

		/// <summary>
		/// Sets the local value of a dependency property,
		/// specified by its dependency property identifier.
		/// </summary>
		/// <param name="dependencyObject">
		/// The <see cref="DependencyObject"/> whose <see cref="DependencyProperty"/> is to be retrieved.
		/// </param>
		/// <param name="property">
		/// The <see cref="DependencyProperty"/> identifier of the dependency property to set.
		/// </param>
		/// <param name="value">The new local value.</param>
		public static void SetValueThreadSafe(this DependencyObject dependencyObject, DependencyProperty property, object value)
		{
			//If this code is running of the correct thread...
			if (dependencyObject.Dispatcher.CheckAccess())
			{
				//...then set the value.
				dependencyObject.SetValue(property, value);
			}
			else
			{
				//...otherwise, invoke the value to be set on the correct thread.
				dependencyObject.Dispatcher.Invoke((DependencyObjectSetValueDelegate)dependencyObject.SetValue, property, value);
			}
		}
		#endregion
	}
}
