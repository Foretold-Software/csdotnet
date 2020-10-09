﻿// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace System.Windows
{
	/// <summary>
	/// A class with a short name for no other purpose than to simplify the creation and use of
	/// the <see cref="DependencyObject"/> class and its associated <see cref="DependencyProperty"/> members.
	/// </summary>
	public class DO : DependencyObject
	{
		//TODO: The first 3 overloads of CreatePCC are unused. Implement overloads of NewPropertyMetadata and DependencyPropertyRegister that use them.
		#region Methods - PropertyChangedCallback Redirection
		/// <summary>
		/// Creates a <see cref="PropertyChangedCallback"/> delegate that redirects to the specified action.
		/// </summary>
		/// <typeparam name="TDO">The type of the class inheriting from <see cref="DependencyObject"/>.</typeparam>
		/// <param name="action">The action to perform as the redirection.</param>
		/// <returns>The generated <see cref="PropertyChangedCallback"/> delegate.</returns>
		public static PropertyChangedCallback CreatePCC<TDO>(Action<TDO> action) where TDO : DependencyObject
		{
			return new PropertyChangedCallback((d, e) =>
			{
				action(d as TDO);
			});
		}

		/// <summary>
		/// Creates a <see cref="PropertyChangedCallback"/> delegate that redirects to the specified action.
		/// </summary>
		/// <typeparam name="TDO">The type of the class inheriting from <see cref="DependencyObject"/>.</typeparam>
		/// <typeparam name="TValue">The type of the property with an associated <see cref="DependencyProperty"/> that implements this method.</typeparam>
		/// <param name="action">The action to perform as the redirection.</param>
		/// <returns>The generated <see cref="PropertyChangedCallback"/> delegate.</returns>
		public static PropertyChangedCallback CreatePCC<TDO, TValue>(Action<TDO, TValue> action) where TDO : DependencyObject
		{
			return new PropertyChangedCallback((d, e) =>
			{
				action(d as TDO, (TValue)e.NewValue);
			});
		}

		/// <summary>
		/// Creates a <see cref="PropertyChangedCallback"/> delegate that redirects to the specified action.
		/// </summary>
		/// <typeparam name="TDO">The type of the class inheriting from <see cref="DependencyObject"/>.</typeparam>
		/// <typeparam name="TValue">The type of the property with an associated <see cref="DependencyProperty"/> that implements this method.</typeparam>
		/// <param name="action">The action to perform as the redirection.</param>
		/// <returns>The generated <see cref="PropertyChangedCallback"/> delegate.</returns>
		public static PropertyChangedCallback CreatePCC<TDO, TValue>(Action<TDO, TValue, TValue> action) where TDO : DependencyObject
		{
			return new PropertyChangedCallback((d, e) =>
			{
				action(d as TDO, (TValue)e.OldValue, (TValue)e.NewValue);
			});
		}

		/// <summary>
		/// Creates a <see cref="PropertyChangedCallback"/> delegate that redirects to the specified action.
		/// </summary>
		/// <typeparam name="TDO">The type of the class inheriting from <see cref="DependencyObject"/>.</typeparam>
		/// <param name="pccSelector">The action to perform as the redirection.</param>
		/// <returns>The generated <see cref="PropertyChangedCallback"/> delegate.</returns>
		public static PropertyChangedCallback CreatePCC<TDO>(Func<TDO, Action> pccSelector) where TDO : DependencyObject
		{
			return new PropertyChangedCallback((d, e) =>
			{
				pccSelector(d as TDO)();
			});
		}

		/// <summary>
		/// Creates a <see cref="PropertyChangedCallback"/> delegate that redirects to the specified action.
		/// </summary>
		/// <typeparam name="TDO">The type of the class inheriting from <see cref="DependencyObject"/>.</typeparam>
		/// <typeparam name="TValue">The type of the property with an associated <see cref="DependencyProperty"/> that implements this method.</typeparam>
		/// <param name="pccSelector">The action to perform as the redirection.</param>
		/// <returns>The generated <see cref="PropertyChangedCallback"/> delegate.</returns>
		public static PropertyChangedCallback CreatePCC<TDO, TValue>(Func<TDO, Action<TValue>> pccSelector) where TDO : DependencyObject
		{
			return new PropertyChangedCallback((d, e) =>
			{
				pccSelector(d as TDO)((TValue)e.NewValue);
			});
		}

		/// <summary>
		/// Creates a <see cref="PropertyChangedCallback"/> delegate that redirects to the specified action.
		/// </summary>
		/// <typeparam name="TDO">The type of the class inheriting from <see cref="DependencyObject"/>.</typeparam>
		/// <typeparam name="TValue">The type of the property with an associated <see cref="DependencyProperty"/> that implements this method.</typeparam>
		/// <param name="pccSelector">The action to perform as the redirection.</param>
		/// <returns>The generated <see cref="PropertyChangedCallback"/> delegate.</returns>
		public static PropertyChangedCallback CreatePCC<TDO, TValue>(Func<TDO, Action<TValue, TValue>> pccSelector) where TDO : DependencyObject
		{
			return new PropertyChangedCallback((d, e) =>
			{
				pccSelector(d as TDO)((TValue)e.OldValue, (TValue)e.NewValue);
			});
		}

		/// <summary>
		/// Creates a <see cref="PropertyChangedCallback"/> delegate that redirects to the specified action and adds the specified event handler.
		/// </summary>
		/// <typeparam name="TDO">
		/// The type of the class inheriting from <see cref="DependencyObject"/>.
		/// </typeparam>
		/// <param name="pccSelector">
		/// The selector function that returns the callback method to use.
		/// </param>
		/// <param name="nccehSelector">
		/// The selector function that returns the <see cref="NotifyCollectionChangedEventHandler"/> to use.
		/// Warning: DO NOT pass a selector that selects an anonymous method.
		/// </param>
		/// <remarks>
		/// Note: The acronym PCC means "Property Changed Callback" and NCCEH means "Notify Collection Changed Event Handler".
		/// </remarks>
		/// <returns>
		/// The generated <see cref="PropertyChangedCallback"/> delegate.
		/// </returns>
		public static PropertyChangedCallback CreatePCC<TDO>(Func<TDO, Action> pccSelector, Func<TDO, NotifyCollectionChangedEventHandler> nccehSelector)
			where TDO : DependencyObject
		{
			return new PropertyChangedCallback((d, e) =>
			{
				var dependencyObject = d as TDO;

				if (dependencyObject != null)
				{
					//Get the collection changed event handler from the selector.
					var cch = nccehSelector(dependencyObject);

					if (cch != null)
					{
						var oldValue = e.OldValue as INotifyCollectionChanged;
						var newValue = e.NewValue as INotifyCollectionChanged;

						//Remove the event handler from the old value.
						if (oldValue != null)
						{
							oldValue.CollectionChanged -= cch;
						}

						//Add the event handler to the new value.
						if (newValue != null)
						{
							newValue.CollectionChanged -= cch; //Remove the handler just in case it's already there, so it is not called multiple times.
							newValue.CollectionChanged += cch;
						}
					}

					//Get the property changed callback method from the selector.
					var pcc = pccSelector(dependencyObject);

					if (pcc != null)
					{
						pcc();
					}
				}
			});
		}

		/// <summary>
		/// Creates a <see cref="PropertyChangedCallback"/> delegate that redirects to the specified action and adds the specified event handler.
		/// </summary>
		/// <typeparam name="TDO">
		/// The type of the class inheriting from <see cref="DependencyObject"/>.
		/// </typeparam>
		/// <typeparam name="TValue">
		/// The type of the property with an associated <see cref="DependencyProperty"/> that implements this method.
		/// Must inherit from <see cref="INotifyCollectionChanged"/>.
		/// </typeparam>
		/// <param name="pccSelector">
		/// The selector function that returns the callback method to use.
		/// This method's parameter is the property's new value.
		/// </param>
		/// <param name="nccehSelector">
		/// The selector function that returns the <see cref="NotifyCollectionChangedEventHandler"/> to use.
		/// Warning: DO NOT pass a selector that selects an anonymous method.
		/// </param>
		/// <remarks>
		/// Note: The acronym PCC means "Property Changed Callback" and NCCEH means "Notify Collection Changed Event Handler".
		/// </remarks>
		/// <returns>
		/// The generated <see cref="PropertyChangedCallback"/> delegate.
		/// </returns>
		public static PropertyChangedCallback CreatePCC<TDO, TValue>(Func<TDO, Action<TValue>> pccSelector, Func<TDO, NotifyCollectionChangedEventHandler> nccehSelector)
			where TDO : DependencyObject
			where TValue : INotifyCollectionChanged
		{
			return new PropertyChangedCallback((d, e) =>
			{
				var dependencyObject = d as TDO;

				if (dependencyObject != null)
				{
					//Get the collection changed event handler from the selector.
					var cch = nccehSelector(dependencyObject);

					if (cch != null)
					{
						var oldValue = e.OldValue as INotifyCollectionChanged;
						var newValue = e.NewValue as INotifyCollectionChanged;

						//Remove the event handler from the old value.
						if (oldValue != null)
						{
							oldValue.CollectionChanged -= cch;
						}

						//Add the event handler to the new value.
						if (newValue != null)
						{
							newValue.CollectionChanged -= cch; //Remove the handler just in case it's already there, so it is not called multiple times.
							newValue.CollectionChanged += cch;
						}
					}

					//Get the property changed callback method from the selector.
					var pcc = pccSelector(dependencyObject);

					if (pcc != null)
					{
						pcc((TValue)e.NewValue);
					}
				}
			});
		}

		/// <summary>
		/// Creates a <see cref="PropertyChangedCallback"/> delegate that redirects to the specified action and adds the specified event handler.
		/// </summary>
		/// <typeparam name="TDO">
		/// The type of the class inheriting from <see cref="DependencyObject"/>.
		/// </typeparam>
		/// <typeparam name="TValue">
		/// The type of the property with an associated <see cref="DependencyProperty"/> that implements this method.
		/// Must inherit from <see cref="INotifyCollectionChanged"/>.
		/// </typeparam>
		/// <param name="pccSelector">
		/// The selector function that returns the callback method to use.
		/// This method's parameters are the property's old and new values, in that order.
		/// </param>
		/// <param name="nccehSelector">
		/// The selector function that returns the <see cref="NotifyCollectionChangedEventHandler"/> to use.
		/// Warning: DO NOT pass a selector that selects an anonymous method.
		/// </param>
		/// <remarks>
		/// Note: The acronym PCC means "Property Changed Callback" and NCCEH means "Notify Collection Changed Event Handler".
		/// </remarks>
		/// <returns>
		/// The generated <see cref="PropertyChangedCallback"/> delegate.
		/// </returns>
		public static PropertyChangedCallback CreatePCC<TDO, TValue>(Func<TDO, Action<TValue, TValue>> pccSelector, Func<TDO, NotifyCollectionChangedEventHandler> nccehSelector)
			where TDO : DependencyObject
			where TValue : INotifyCollectionChanged
		{
			return new PropertyChangedCallback((d, e) =>
			{
				var dependencyObject = d as TDO;

				if (dependencyObject != null)
				{
					//Get the collection changed event handler from the selector.
					var cch = nccehSelector(dependencyObject);

					if (cch != null)
					{
						var oldValue = e.OldValue as INotifyCollectionChanged;
						var newValue = e.NewValue as INotifyCollectionChanged;

						//Remove the event handler from the old value.
						if (oldValue != null)
						{
							oldValue.CollectionChanged -= cch;
						}

						//Add the event handler to the new value.
						if (newValue != null)
						{
							newValue.CollectionChanged -= cch; //Remove the handler just in case it's already there, so it is not called multiple times.
							newValue.CollectionChanged += cch;
						}
					}

					//Get the property changed callback method from the selector.
					var pcc = pccSelector(dependencyObject);

					if (pcc != null)
					{
						pcc((TValue)e.OldValue, (TValue)e.NewValue);
					}
				}
			});
		}
		#endregion

		#region Methods - PropertyMetadata Creation
		/// <summary>
		/// Creates a <see cref="PropertyMetadata"/> instance with a <see cref="PropertyChangedCallback"/> delegate that redirects to the specified action.
		/// </summary>
		/// <typeparam name="TDO">
		/// The type of the class inheriting from <see cref="DependencyObject"/>.
		/// </typeparam>
		/// <param name="pccSelector">
		/// The selector function that returns the callback method to use.
		/// </param>
		/// <remarks>
		/// Note: The acronym PCC means "Property Changed Callback".
		/// </remarks>
		/// <returns>
		/// The generated <see cref="PropertyMetadata"/> instance.
		/// </returns>
		public static PropertyMetadata NewPropertyMetadata<TDO>(Func<TDO, Action> pccSelector) where TDO : DependencyObject
		{
			return new PropertyMetadata(CreatePCC(pccSelector));
		}

		/// <summary>
		/// Creates a <see cref="PropertyMetadata"/> instance with a <see cref="PropertyChangedCallback"/> delegate that redirects to the specified action.
		/// </summary>
		/// <typeparam name="TDO">
		/// The type of the class inheriting from <see cref="DependencyObject"/>.
		/// </typeparam>
		/// <typeparam name="TValue">
		/// The type of the property with an associated <see cref="DependencyProperty"/> that implements this method.
		/// </typeparam>
		/// <param name="pccSelector">
		/// The selector function that returns the callback method to use.
		/// This method's parameter is the property's new value.
		/// </param>
		/// <remarks>
		/// Note: The acronym PCC means "Property Changed Callback".
		/// </remarks>
		/// <returns>
		/// The generated <see cref="PropertyMetadata"/> instance.
		/// </returns>
		public static PropertyMetadata NewPropertyMetadata<TDO, TValue>(Func<TDO, Action<TValue>> pccSelector) where TDO : DependencyObject
		{
			return new PropertyMetadata(CreatePCC(pccSelector));
		}

		/// <summary>
		/// Creates a <see cref="PropertyMetadata"/> instance with a <see cref="PropertyChangedCallback"/> delegate that redirects to the specified action.
		/// </summary>
		/// <typeparam name="TDO">
		/// The type of the class inheriting from <see cref="DependencyObject"/>.
		/// </typeparam>
		/// <typeparam name="TValue">
		/// The type of the property with an associated <see cref="DependencyProperty"/> that implements this method.
		/// </typeparam>
		/// <param name="pccSelector">
		/// The selector function that returns the callback method to use.
		/// This method's parameters are the property's old and new values, in that order.
		/// </param>
		/// <remarks>
		/// Note: The acronym PCC means "Property Changed Callback".
		/// </remarks>
		/// <returns>
		/// The generated <see cref="PropertyMetadata"/> instance.
		/// </returns>
		public static PropertyMetadata NewPropertyMetadata<TDO, TValue>(Func<TDO, Action<TValue, TValue>> pccSelector) where TDO : DependencyObject
		{
			return new PropertyMetadata(CreatePCC(pccSelector));
		}

		/// <summary>
		/// Creates a <see cref="PropertyMetadata"/> instance with a <see cref="PropertyChangedCallback"/> delegate that redirects to the specified action.
		/// </summary>
		/// <typeparam name="TDO">
		/// The type of the class inheriting from <see cref="DependencyObject"/>.
		/// </typeparam>
		/// <typeparam name="TValue">
		/// The type of the property with an associated <see cref="DependencyProperty"/> that implements this method.
		/// </typeparam>
		/// <param name="defaultValue">
		/// The default value of the dependency property, usually provided as a value of some specific type.
		/// </param>
		/// <param name="pccSelector">
		/// The selector function that returns the callback method to use.
		/// </param>
		/// <remarks>
		/// Note: The acronym PCC means "Property Changed Callback".
		/// </remarks>
		/// <returns>
		/// The generated <see cref="PropertyMetadata"/> instance.
		/// </returns>
		public static PropertyMetadata NewPropertyMetadata<TDO, TValue>(TValue defaultValue, Func<TDO, Action> pccSelector) where TDO : DependencyObject
		{
			return new PropertyMetadata(defaultValue, CreatePCC(pccSelector));
		}

		/// <summary>
		/// Creates a <see cref="PropertyMetadata"/> instance with a <see cref="PropertyChangedCallback"/> delegate that redirects to the specified action.
		/// </summary>
		/// <typeparam name="TDO">
		/// The type of the class inheriting from <see cref="DependencyObject"/>.
		/// </typeparam>
		/// <typeparam name="TValue">
		/// The type of the property with an associated <see cref="DependencyProperty"/> that implements this method.
		/// </typeparam>
		/// <param name="defaultValue">
		/// The default value of the dependency property, usually provided as a value of some specific type.
		/// </param>
		/// <param name="pccSelector">
		/// The selector function that returns the callback method to use.
		/// This method's parameter is the property's new value.
		/// </param>
		/// <remarks>
		/// Note: The acronym PCC means "Property Changed Callback".
		/// </remarks>
		/// <returns>
		/// The generated <see cref="PropertyMetadata"/> instance.
		/// </returns>
		public static PropertyMetadata NewPropertyMetadata<TDO, TValue>(TValue defaultValue, Func<TDO, Action<TValue>> pccSelector) where TDO : DependencyObject
		{
			return new PropertyMetadata(defaultValue, CreatePCC(pccSelector));
		}

		/// <summary>
		/// Creates a <see cref="PropertyMetadata"/> instance with a <see cref="PropertyChangedCallback"/> delegate that redirects to the specified action.
		/// </summary>
		/// <typeparam name="TDO">
		/// The type of the class inheriting from <see cref="DependencyObject"/>.
		/// </typeparam>
		/// <typeparam name="TValue">
		/// The type of the property with an associated <see cref="DependencyProperty"/> that implements this method.
		/// </typeparam>
		/// <param name="defaultValue">
		/// The default value of the dependency property, usually provided as a value of some specific type.
		/// </param>
		/// <param name="pccSelector">
		/// The selector function that returns the callback method to use.
		/// This method's parameters are the property's old and new values, in that order.
		/// </param>
		/// <remarks>
		/// Note: The acronym PCC means "Property Changed Callback".
		/// </remarks>
		/// <returns>
		/// The generated <see cref="PropertyMetadata"/> instance.
		/// </returns>
		public static PropertyMetadata NewPropertyMetadata<TDO, TValue>(TValue defaultValue, Func<TDO, Action<TValue, TValue>> pccSelector) where TDO : DependencyObject
		{
			return new PropertyMetadata(defaultValue, CreatePCC(pccSelector));
		}

		/// <summary>
		/// Creates a <see cref="PropertyMetadata"/> instance with a <see cref="PropertyChangedCallback"/> delegate that redirects to the
		/// specified action and assigns a <see cref="NotifyCollectionChangedEventHandler"/> to the <see cref="INotifyCollectionChanged"/>.
		/// </summary>
		/// <typeparam name="TDO">
		/// The type of the class inheriting from <see cref="DependencyObject"/>.
		/// </typeparam>
		/// <typeparam name="TValue">
		/// The type of the property with an associated <see cref="DependencyProperty"/> that implements this method.
		/// Must inherit from <see cref="INotifyCollectionChanged"/>.
		/// </typeparam>
		/// <param name="defaultValue">
		/// The default value of the dependency property, usually provided as a value of some specific type.
		/// </param>
		/// <param name="pccSelector">
		/// The selector function that returns the callback method to use.
		/// </param>
		/// <param name="nccehSelector">
		/// The selector function that returns the <see cref="NotifyCollectionChangedEventHandler"/> to use.
		/// Warning: DO NOT pass a selector that selects an anonymous method.
		/// </param>
		/// <remarks>
		/// Note: The acronym PCC means "Property Changed Callback" and NCCEH means "Notify Collection Changed Event Handler".
		/// </remarks>
		/// <returns>
		/// The generated <see cref="PropertyMetadata"/> instance.
		/// </returns>
		public static PropertyMetadata NewPropertyMetadata<TDO, TValue>(TValue defaultValue, Func<TDO, Action> pccSelector, Func<TDO, NotifyCollectionChangedEventHandler> nccehSelector)
			where TDO : DependencyObject
			where TValue : INotifyCollectionChanged
		{
			return new PropertyMetadata(defaultValue, CreatePCC(pccSelector, nccehSelector));
		}

		/// <summary>
		/// Creates a <see cref="PropertyMetadata"/> instance with a <see cref="PropertyChangedCallback"/> delegate that redirects to the
		/// specified action and assigns a <see cref="NotifyCollectionChangedEventHandler"/> to the <see cref="INotifyCollectionChanged"/>.
		/// </summary>
		/// <typeparam name="TDO">
		/// The type of the class inheriting from <see cref="DependencyObject"/>.
		/// </typeparam>
		/// <typeparam name="TValue">
		/// The type of the property with an associated <see cref="DependencyProperty"/> that implements this method.
		/// Must inherit from <see cref="INotifyCollectionChanged"/>.
		/// </typeparam>
		/// <param name="defaultValue">
		/// The default value of the dependency property, usually provided as a value of some specific type.
		/// </param>
		/// <param name="pccSelector">
		/// The selector function that returns the callback method to use.
		/// This method's parameter is the property's new value.
		/// </param>
		/// <param name="nccehSelector">
		/// The selector function that returns the <see cref="NotifyCollectionChangedEventHandler"/> to use.
		/// Warning: DO NOT pass a selector that selects an anonymous method.
		/// </param>
		/// <remarks>
		/// Note: The acronym PCC means "Property Changed Callback" and NCCEH means "Notify Collection Changed Event Handler".
		/// </remarks>
		/// <returns>
		/// The generated <see cref="PropertyMetadata"/> instance.
		/// </returns>
		public static PropertyMetadata NewPropertyMetadata<TDO, TValue>(TValue defaultValue, Func<TDO, Action<TValue>> pccSelector, Func<TDO, NotifyCollectionChangedEventHandler> nccehSelector)
			where TDO : DependencyObject
			where TValue : INotifyCollectionChanged
		{
			return new PropertyMetadata(defaultValue, CreatePCC(pccSelector, nccehSelector));
		}

		/// <summary>
		/// Creates a <see cref="PropertyMetadata"/> instance with a <see cref="PropertyChangedCallback"/> delegate that redirects to the
		/// specified action and assigns a <see cref="NotifyCollectionChangedEventHandler"/> to the <see cref="INotifyCollectionChanged"/>.
		/// </summary>
		/// <typeparam name="TDO">
		/// The type of the class inheriting from <see cref="DependencyObject"/>.
		/// </typeparam>
		/// <typeparam name="TValue">
		/// The type of the property with an associated <see cref="DependencyProperty"/> that implements this method.
		/// Must inherit from <see cref="INotifyCollectionChanged"/>.
		/// </typeparam>
		/// <param name="defaultValue">
		/// The default value of the dependency property, usually provided as a value of some specific type.
		/// </param>
		/// <param name="pccSelector">
		/// The selector function that returns the callback method to use.
		/// This method's parameters are the property's old and new values, in that order.
		/// </param>
		/// <param name="nccehSelector">
		/// The selector function that returns the <see cref="NotifyCollectionChangedEventHandler"/> to use.
		/// Warning: DO NOT pass a selector that selects an anonymous method.
		/// </param>
		/// <remarks>
		/// Note: The acronym PCC means "Property Changed Callback" and NCCEH means "Notify Collection Changed Event Handler".
		/// </remarks>
		/// <returns>
		/// The generated <see cref="PropertyMetadata"/> instance.
		/// </returns>
		public static PropertyMetadata NewPropertyMetadata<TDO, TValue>(TValue defaultValue, Func<TDO, Action<TValue, TValue>> pccSelector, Func<TDO, NotifyCollectionChangedEventHandler> nccehSelector)
			where TDO : DependencyObject
			where TValue : INotifyCollectionChanged
		{
			return new PropertyMetadata(defaultValue, CreatePCC(pccSelector, nccehSelector));
		}

		#endregion

		//TODO: Add documentation to the methods in this #region block
		#region Methods - DependencyProperty Creation

		public static DependencyProperty DependencyPropertyRegister<TDO, TValue>(string name)
			where TDO : DependencyObject
		{
			return DependencyProperty.Register(name, typeof(TValue), typeof(TDO));
		}

		public static DependencyProperty DependencyPropertyRegister<TDO, TValue>(string name, TValue defaultValue)
			where TDO : DependencyObject
		{
			return DependencyProperty.Register(name, typeof(TValue), typeof(TDO), new PropertyMetadata(defaultValue));
		}

		public static DependencyProperty DependencyPropertyRegister<TDO, TValue>(string name, Func<TDO, Action> selector) where TDO : DependencyObject
		{
			return DependencyProperty.Register(name, typeof(TValue), typeof(TDO), NewPropertyMetadata<TDO>(selector));
		}

		public static DependencyProperty DependencyPropertyRegister<TDO, TValue>(string name, Func<TDO, Action<TValue>> selector) where TDO : DependencyObject
		{
			return DependencyProperty.Register(name, typeof(TValue), typeof(TDO), NewPropertyMetadata<TDO, TValue>(selector));
		}

		public static DependencyProperty DependencyPropertyRegister<TDO, TValue>(string name, Func<TDO, Action<TValue, TValue>> selector) where TDO : DependencyObject
		{
			return DependencyProperty.Register(name, typeof(TValue), typeof(TDO), NewPropertyMetadata<TDO, TValue>(selector));
		}

		public static DependencyProperty DependencyPropertyRegister<TDO, TValue>(string name, TValue defaultValue, Func<TDO, Action> selector) where TDO : DependencyObject
		{
			return DependencyProperty.Register(name, typeof(TValue), typeof(TDO), NewPropertyMetadata<TDO, TValue>(defaultValue, selector));
		}

		public static DependencyProperty DependencyPropertyRegister<TDO, TValue>(string name, TValue defaultValue, Func<TDO, Action<TValue>> selector) where TDO : DependencyObject
		{
			return DependencyProperty.Register(name, typeof(TValue), typeof(TDO), NewPropertyMetadata<TDO, TValue>(defaultValue, selector));
		}

		public static DependencyProperty DependencyPropertyRegister<TDO, TValue>(string name, TValue defaultValue, Func<TDO, Action<TValue, TValue>> selector) where TDO : DependencyObject
		{
			return DependencyProperty.Register(name, typeof(TValue), typeof(TDO), NewPropertyMetadata<TDO, TValue>(defaultValue, selector));
		}

		public static DependencyProperty DependencyPropertyRegister<TDO, TValue>(string name, TValue defaultValue, Func<TDO, Action> pccSelector, Func<TDO, NotifyCollectionChangedEventHandler> nccehSelector)
			where TDO : DependencyObject
			where TValue : INotifyCollectionChanged
		{
			return DependencyProperty.Register(name, typeof(TValue), typeof(TDO), NewPropertyMetadata<TDO, TValue>(defaultValue, pccSelector, nccehSelector));
		}

		public static DependencyProperty DependencyPropertyRegister<TDO, TValue>(string name, TValue defaultValue, Func<TDO, Action<TValue>> pccSelector, Func<TDO, NotifyCollectionChangedEventHandler> nccehSelector)
			where TDO : DependencyObject
			where TValue : INotifyCollectionChanged
		{
			return DependencyProperty.Register(name, typeof(TValue), typeof(TDO), NewPropertyMetadata<TDO, TValue>(defaultValue, pccSelector, nccehSelector));
		}

		public static DependencyProperty DependencyPropertyRegister<TDO, TValue>(string name, TValue defaultValue, Func<TDO, Action<TValue, TValue>> pccSelector, Func<TDO, NotifyCollectionChangedEventHandler> nccehSelector)
			where TDO : DependencyObject
			where TValue : INotifyCollectionChanged
		{
			return DependencyProperty.Register(name, typeof(TValue), typeof(TDO), NewPropertyMetadata<TDO, TValue>(defaultValue, pccSelector, nccehSelector));
		}
		#endregion
	}
}
