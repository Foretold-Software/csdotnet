// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.Windows.Threading;

namespace System.ComponentModel
{
	/// <summary>
	/// A base class that implements the <see cref="INotifyPropertyChanged"/> interface.
	/// </summary>
	public abstract class NotifyPropertyChanged : INotifyPropertyChanged
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
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
		/// Calls the <see cref="PropertyChanged"/> event on
		/// the <see cref="DispatcherObject"/>'s thread. Used
		/// to indicate property changes on the UI thread.
		/// </summary>
		/// <param name="dispatcherObject">The dispatcher to use to call the event on the UI thread.</param>
		/// <param name="propertyName">The name of the property.</param>
		protected virtual void OnPropertyChanged(DispatcherObject dispatcherObject, string propertyName)
		{
			dispatcherObject.CheckAndInvoke(() => OnPropertyChanged(propertyName));
		}

		/// <summary>
		/// Calls the <see cref="PropertyChanged"/> event on
		/// the <see cref="DispatcherObject"/>'s thread. Used
		/// to indicate property changes on the UI thread.
		/// </summary>
		/// <param name="dispatcherObject">The dispatcher to use to call the event on the UI thread.</param>
		/// <param name="propertyNames">The names of the properties.</param>
		protected virtual void OnPropertyChanged(DispatcherObject dispatcherObject, params string[] propertyNames)
		{
			dispatcherObject.CheckAndInvoke(() => OnPropertyChanged(propertyNames));
		}
		#endregion
	}
}
