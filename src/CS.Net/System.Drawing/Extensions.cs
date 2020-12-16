// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Imaging;
using InteropImaging = System.Windows.Interop.Imaging;

namespace System.Drawing
{
	/// <summary>
	/// A static class containing extension methods for classes within the <see cref="System.Drawing"/> namespace.
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// The DeleteObject function deletes a logical pen,
		/// brush, font, bitmap, region, or palette, freeing
		/// all system resources associated with the object.
		/// After the object is deleted, the specified handle is no longer valid.
		/// </summary>
		/// <param name="hObject">
		/// A handle to a logical pen, brush, font, bitmap, region, or palette.
		/// </param>
		/// <returns>
		/// If the function succeeds, the return value is nonzero.
		/// If the specified handle is not valid or is currently selected into a DC, the return value is zero.		
		/// </returns>
		/// <remarks>
		/// Do not delete a drawing object (pen or brush) while it is still selected into a DC.
		/// When a pattern brush is deleted, the bitmap associated with the brush is not deleted. The bitmap must be deleted independently.
		/// https://msdn.microsoft.com/en-us/library/dd183539.aspx
		/// </remarks>
		[DllImport("gdi32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool DeleteObject(IntPtr hObject);

		/// <summary>
		/// Converts an instance of <see cref="Bitmap"/> to <see cref="BitmapSource"/>.
		/// Use this method to retrieve an image from a resx resource and display it in a WPF UI.
		/// </summary>
		/// <remarks>Uses GDI to do the conversion. Hence the call to the marshalled DeleteObject.</remarks>
		public static BitmapSource ToBitmapSource(this Bitmap source)
		{
			BitmapSource bitmapSource;

			var hBitmap = source.GetHbitmap();

			try
			{
				bitmapSource = InteropImaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
			}
			catch (Win32Exception)
			{
				//This should not be needed.
				bitmapSource = null;
			}
			finally
			{
				//Since this method uses GDI to do the conversion, the DeleteObject function must be called.
				DeleteObject(hBitmap);
			}

			return bitmapSource;
		}
	}
}
