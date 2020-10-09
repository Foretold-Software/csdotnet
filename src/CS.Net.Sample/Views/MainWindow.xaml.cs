// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace CS.Net.Sample.Views
{
	public partial class MainWindow : DraggableWindow
	{
		public MainWindow()
		{
			MyDictionary = new ObservableDictionary<string, string>();

			DataContext = this;
			InitializeComponent();
		}

		#region Fields
		private volatile bool waiting = false;
		#endregion

		#region Properties
		public ObservableDictionary<string, string> MyDictionary { get; set; }
		#endregion

		#region Methods
		public void ShowAndWait()
		{
			waiting = true;

			BeginAddingItems();

            Show();
			System.Windows.Threading.Dispatcher.Run();
		}

		private void BeginAddingItems()
		{
			new Thread(() =>
			{
				while (waiting)
				{
					Thread.Sleep(3000);

					this.Dispatcher.CheckAndBeginInvoke(() => MyDictionary.Add("Key = " + DateTime.Now.ToString(), ""));
				}
			}).Start();
		}

		private void CloseButton_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void RunButton_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("SURPRIIIIIISE!!!");
		}

		protected override void OnClosed(EventArgs e)
		{
			base.OnClosed(e);

			if (waiting)
			{
				waiting = false;
				Dispatcher.InvokeShutdown();
			}
		}
		#endregion
	}
}
