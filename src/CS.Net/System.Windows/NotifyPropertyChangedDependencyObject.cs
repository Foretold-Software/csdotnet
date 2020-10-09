// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.ComponentModel;
using System.Windows.Threading;

namespace System.Windows
{
	/// <summary>
	/// A base class that inherits from <see cref="DependencyObjectBase"/>
	/// and implements the <see cref="INotifyPropertyChanged"/> interface.
	/// </summary>
	public class NotifyPropertyChangedDependencyObject : DependencyObjectBase, INotifyPropertyChanged
	{
		#region Events
		/// <summary>
		/// Occurs when a property value changes.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;
		#endregion

		#region Methods
		/// <summary>
		/// Calls the <see cref="PropertyChanged"/> event.
		/// </summary>
		/// <param name="propertyName">The name of the property.</param>
		protected virtual void OnPropertyChanged(string propertyName)
		{
			var handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		/// <summary>
		/// Calls the <see cref="PropertyChanged"/> event.
		/// </summary>
		/// <param name="propertyNames">The names of the properties.</param>
		protected virtual void OnPropertyChanged(params string[] propertyNames)
		{
			var handler = PropertyChanged;
			if (handler != null)
			{
				foreach (var propertyName in propertyNames)
				{
					handler(this, new PropertyChangedEventArgs(propertyName));
				}
			}
		}

		/// <summary>
		/// Calls the <see cref="PropertyChanged"/> event on this
		/// instance's (as a <see cref="DispatcherObject"/>) thread.
		/// Used to indicate property changes on the UI thread.
		/// </summary>
		/// <param name="propertyName">The name of the property.</param>
		protected virtual void OnPropertyChangedThreadSafe(string propertyName)
		{
			Dispatcher.CheckAndInvoke(() =>
			{
				OnPropertyChanged(propertyName);
			});
		}

		/// <summary>
		/// Calls the <see cref="PropertyChanged"/> event on this
		/// instance's (as a <see cref="DispatcherObject"/>) thread.
		/// Used to indicate property changes on the UI thread.
		/// </summary>
		/// <param name="propertyNames">The names of the properties.</param>
		protected virtual void OnPropertyChangedThreadSafe(params string[] propertyNames)
		{
			Dispatcher.CheckAndInvoke(() =>
			{
				var handler = PropertyChanged;
				if (handler != null)
				{
					foreach (var propertyName in propertyNames)
					{
						handler(this, new PropertyChangedEventArgs(propertyName));
					}
				}
			});
		}
		#endregion
	}
}
