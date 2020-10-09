// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Imaging;

namespace System.Drawing
{
	/// <summary>
	/// Provides properties used to obtain bitmaps of stock Windows icons.
	/// </summary>
	public static class StockIcon
	{
		#region Methods - GetBitmapSource
		/// <summary>
		/// Gets the stock icon with the given id.
		/// </summary>
		/// <param name="stockIconId">The id of the icon to retrieve.</param>
		/// <returns>Returns an instance of <see cref="BitmapSource"/> containing the stock icon.</returns>
		private static BitmapSource GetBitmapSource(StockIconId stockIconId)
		{
			return GetBitmapSource(stockIconId, 0);
		}

		/// <summary>
		/// Gets the stock icon with the given id and flags.
		/// </summary>
		/// <param name="stockIconId">The id of the icon to retrieve.</param>
		/// <param name="flags">Flag values indicating how to create the icon.</param>
		/// <returns>Returns an instance of <see cref="BitmapSource"/> containing the stock icon.</returns>
		private static BitmapSource GetBitmapSource(StockIconId stockIconId, StockIconFlags flags)
		{
			int hResult;
			BitmapSource bitmapSource;
			var stockIconInfo = StockIconInfo.Empty();

			//Get the stock icon's handle.
			hResult = SHGetStockIconInfo((uint)stockIconId, 0x100 | (uint)flags, ref stockIconInfo);

			//If the icon retrieval failed, throw an error.
			if (hResult < 0)
			{
				throw new COMException("An error occurred when calling SHGetStockIconInfo.", hResult);
			}


			try
			{
				//Convert the handle into an instance of BitmapSource.
				bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(stockIconInfo.IconHandle, Int32Rect.Empty, null);
			}
			finally
			{
				//Don't forget to release the resources.
				DestroyIcon(stockIconInfo.IconHandle);
			}

			//Freeze the bitmap.
			bitmapSource.Freeze();

			return bitmapSource;
		}
		#endregion

		#region Win32 Interop
		/// <summary>
		/// Retrieves information about system-defined Shell icons.
		/// </summary>
		/// <param name="siid">
		/// One of the values from the SHSTOCKICONID enumeration that specifies which icon should be retrieved.
		/// </param>
		/// <param name="uFlags">
		/// A combination of zero or more of the flags that specify which information is requested.
		/// </param>
		/// <param name="psii">
		/// A pointer to a SHSTOCKICONINFO structure.
		/// When this function is called, the cbSize member of this structure needs to be set to the size of the SHSTOCKICONINFO structure.
		/// When this function returns, contains a pointer to a SHSTOCKICONINFO structure that contains the requested information.
		/// </param>
		/// <returns>
		/// If this function succeeds, it returns S_OK.
		/// Otherwise, it returns an HRESULT error code.
		/// </returns>
		/// <remarks>
		/// https://msdn.microsoft.com/en-us/library/windows/desktop/bb762205.aspx
		/// If this function returns an icon handle in the hIcon member of the SHSTOCKICONINFO structure
		/// pointed to by psii, you are responsible for freeing the icon with DestroyIcon when you no longer need it.
		/// </remarks>
		[DllImport("Shell32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = false)]
		internal static extern int SHGetStockIconInfo(uint siid, uint uFlags, ref StockIconInfo psii);

		/// <summary>
		/// Destroys an icon and frees any memory the icon occupied. 
		/// </summary>
		/// <param name="hIcon">
		/// A handle to the icon to be destroyed. The icon must not be in use.
		/// </param>
		/// <returns>
		/// If the function succeeds, the return value is nonzero.
		/// If the function fails, the return value is zero.
		/// To get extended error information, call GetLastError. 
		/// </returns>
		/// <remarks>https://msdn.microsoft.com/en-us/library/windows/desktop/ms648063.aspx</remarks>
		[DllImport("User32.dll", SetLastError = true)]
		internal static extern bool DestroyIcon(IntPtr hIcon);

		/// <summary>
		/// Receives information used to retrieve a stock Shell icon.
		/// </summary>
		/// <remarks>https://msdn.microsoft.com/en-us/library/windows/desktop/bb759805.aspx</remarks>
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct StockIconInfo
		{
			/// <summary>
			/// The size of this structure, in bytes.
			/// </summary>
			internal uint cbSize;
			/// <summary>
			/// The handle to the icon.
			/// </summary>
			internal IntPtr IconHandle;
			/// <summary>
			/// The index of the image in the system icon cache.
			/// </summary>
			internal int SystemImageIndex;
			/// <summary>
			/// The index of the icon in the resource whose path is received in <see cref="Path"/>.
			/// </summary>
			internal int IconIdentifier;
			/// <summary>
			/// The path of the resource that contains the icon.
			/// The index of the icon within the resource is received in <see cref="IconIdentifier"/>.
			/// </summary>
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			internal string Path;

			/// <summary>
			/// Creates a new, empty instance of <see cref="StockIconInfo"/>
			/// with the <see cref="cbSize"/> field populated.
			/// </summary>
			/// <returns>The new instance of <see cref="StockIconInfo"/>.</returns>
			public static StockIconInfo Empty()
			{
				var stockIconInfo = new StockIconInfo();

				stockIconInfo.cbSize = (uint)Marshal.SizeOf(stockIconInfo);

				return stockIconInfo;
			}
		}
		#endregion

		#region Properties - Icons
		/// <summary>
		/// Document of a type with no associated application. SIID_DOCNOASSOC
		/// </summary>
		public static BitmapSource DocNoAssoc
		{ get { return GetBitmapSource(StockIconId.DocNoAssoc); } }

		/// <summary>
		/// Document of a type with an associated application. SIID_DOCASSOC
		/// </summary>
		public static BitmapSource DocAssoc
		{ get { return GetBitmapSource(StockIconId.DocAssoc); } }

		/// <summary>
		/// Generic application with no custom icon. SIID_APPLICATION
		/// </summary>
		public static BitmapSource Application
		{ get { return GetBitmapSource(StockIconId.Application); } }

		/// <summary>
		/// Folder (generic, unspecified state). SIID_FOLDER
		/// </summary>
		public static BitmapSource Folder
		{ get { return GetBitmapSource(StockIconId.Folder); } }

		/// <summary>
		/// Folder (open). SIID_FOLDEROPEN
		/// </summary>
		public static BitmapSource FolderOpen
		{ get { return GetBitmapSource(StockIconId.FolderOpen); } }

		/// <summary>
		/// 5.25-inch disk drive. SIID_DRIVE525
		/// </summary>
		public static BitmapSource Drive525
		{ get { return GetBitmapSource(StockIconId.Drive525); } }

		/// <summary>
		/// 3.5-inch disk drive. SIID_DRIVE35
		/// </summary>
		public static BitmapSource Drive35
		{ get { return GetBitmapSource(StockIconId.Drive35); } }

		/// <summary>
		/// Removable drive. SIID_DRIVEREMOVE
		/// </summary>
		public static BitmapSource DriveRemove
		{ get { return GetBitmapSource(StockIconId.DriveRemove); } }

		/// <summary>
		/// Fixed drive (hard disk). SIID_DRIVEFIXED
		/// </summary>
		public static BitmapSource DriveFixed
		{ get { return GetBitmapSource(StockIconId.DriveFixed); } }

		/// <summary>
		/// Network drive (connected). SIID_DRIVENET
		/// </summary>
		public static BitmapSource DriveNet
		{ get { return GetBitmapSource(StockIconId.DriveNet); } }

		/// <summary>
		/// Network drive (disconnected). SIID_DRIVENETDISABLED
		/// </summary>
		public static BitmapSource DriveNetDisabled
		{ get { return GetBitmapSource(StockIconId.DriveNetDisabled); } }

		/// <summary>
		/// CD drive. SIID_DRIVECD
		/// </summary>
		public static BitmapSource DriveCD
		{ get { return GetBitmapSource(StockIconId.DriveCD); } }

		/// <summary>
		/// RAM disk drive. SIID_DRIVERAM
		/// </summary>
		public static BitmapSource DriveRAM
		{ get { return GetBitmapSource(StockIconId.DriveRAM); } }

		/// <summary>
		/// The entire network. SIID_WORLD
		/// </summary>
		public static BitmapSource World
		{ get { return GetBitmapSource(StockIconId.World); } }

		/// <summary>
		/// A computer on the network. SIID_SERVER
		/// </summary>
		public static BitmapSource Server
		{ get { return GetBitmapSource(StockIconId.Server); } }

		/// <summary>
		/// A local printer or print destination. SIID_PRINTER
		/// </summary>
		public static BitmapSource Printer
		{ get { return GetBitmapSource(StockIconId.Printer); } }

		/// <summary>
		/// The Network virtual folder (FOLDERID_NetworkFolder/CSIDL_NETWORK). SIID_MYNETWORK
		/// </summary>
		public static BitmapSource MyNetwork
		{ get { return GetBitmapSource(StockIconId.MyNetwork); } }

		/// <summary>
		/// The Search feature. SIID_FIND
		/// </summary>
		public static BitmapSource Find
		{ get { return GetBitmapSource(StockIconId.Find); } }

		/// <summary>
		/// The Help and Support feature. SIID_HELP
		/// </summary>
		public static BitmapSource Help
		{ get { return GetBitmapSource(StockIconId.Help); } }

		/// <summary>
		/// Overlay for a shared item. SIID_SHARE
		/// </summary>
		public static BitmapSource Share
		{ get { return GetBitmapSource(StockIconId.Share); } }

		/// <summary>
		/// Overlay for a shortcut. SIID_LINK
		/// </summary>
		public static BitmapSource Link
		{ get { return GetBitmapSource(StockIconId.Link); } }

		/// <summary>
		/// Overlay for items that are expected to be slow to access. SIID_SLOWFILE
		/// </summary>
		public static BitmapSource SlowFile
		{ get { return GetBitmapSource(StockIconId.SlowFile); } }

		/// <summary>
		/// The Recycle Bin (empty). SIID_RECYCLER
		/// </summary>
		public static BitmapSource Recycler
		{ get { return GetBitmapSource(StockIconId.Recycler); } }

		/// <summary>
		/// The Recycle Bin (not empty). SIID_RECYCLERFULL
		/// </summary>
		public static BitmapSource RecyclerFull
		{ get { return GetBitmapSource(StockIconId.RecyclerFull); } }

		/// <summary>
		/// Audio CD media. SIID_MEDIACDAUDIO
		/// </summary>
		public static BitmapSource MediaCDAudio
		{ get { return GetBitmapSource(StockIconId.MediaCDAudio); } }

		/// <summary>
		/// Security lock. SIID_LOCK
		/// </summary>
		public static BitmapSource Lock
		{ get { return GetBitmapSource(StockIconId.Lock); } }

		/// <summary>
		/// A virtual folder that contains the results of a search. SIID_AUTOLIST
		/// </summary>
		public static BitmapSource AutoList
		{ get { return GetBitmapSource(StockIconId.AutoList); } }

		/// <summary>
		/// A network printer. SIID_PRINTERNET
		/// </summary>
		public static BitmapSource PrinterNet
		{ get { return GetBitmapSource(StockIconId.PrinterNet); } }

		/// <summary>
		/// A server shared on a network. SIID_SERVERSHARE
		/// </summary>
		public static BitmapSource ServerShare
		{ get { return GetBitmapSource(StockIconId.ServerShare); } }

		/// <summary>
		/// A local fax printer. SIID_PRINTERFAX
		/// </summary>
		public static BitmapSource PrinterFax
		{ get { return GetBitmapSource(StockIconId.PrinterFax); } }

		/// <summary>
		/// A network fax printer. SIID_PRINTERFAXNET
		/// </summary>
		public static BitmapSource PrinterFaxNet
		{ get { return GetBitmapSource(StockIconId.PrinterFaxNet); } }

		/// <summary>
		/// A file that receives the output of a Print to file operation. SIID_PRINTERFILE
		/// </summary>
		public static BitmapSource PrinterFile
		{ get { return GetBitmapSource(StockIconId.PrinterFile); } }

		/// <summary>
		/// A category that results from a Stack by command to organize the contents of a folder. SIID_STACK
		/// </summary>
		public static BitmapSource Stack
		{ get { return GetBitmapSource(StockIconId.Stack); } }

		/// <summary>
		/// Super Video CD (SVCD) media. SIID_MEDIASVCD
		/// </summary>
		public static BitmapSource MediaSVCD
		{ get { return GetBitmapSource(StockIconId.MediaSVCD); } }

		/// <summary>
		/// A folder that contains only subfolders as child items. SIID_STUFFEDFOLDER
		/// </summary>
		public static BitmapSource StuffedFolder
		{ get { return GetBitmapSource(StockIconId.StuffedFolder); } }

		/// <summary>
		/// Unknown drive type. SIID_DRIVEUNKNOWN
		/// </summary>
		public static BitmapSource DriveUnknown
		{ get { return GetBitmapSource(StockIconId.DriveUnknown); } }

		/// <summary>
		/// DVD drive. SIID_DRIVEDVD
		/// </summary>
		public static BitmapSource DriveDVD
		{ get { return GetBitmapSource(StockIconId.DriveDVD); } }

		/// <summary>
		/// DVD media. SIID_MEDIADVD
		/// </summary>
		public static BitmapSource MediaDVD
		{ get { return GetBitmapSource(StockIconId.MediaDVD); } }

		/// <summary>
		/// DVD-RAM media. SIID_MEDIADVDRAM
		/// </summary>
		public static BitmapSource MediaDVDRAM
		{ get { return GetBitmapSource(StockIconId.MediaDVDRAM); } }

		/// <summary>
		/// DVD-RW media. SIID_MEDIADVDRW
		/// </summary>
		public static BitmapSource MediaDVDRW
		{ get { return GetBitmapSource(StockIconId.MediaDVDRW); } }

		/// <summary>
		/// DVD-R media. SIID_MEDIADVDR
		/// </summary>
		public static BitmapSource MediaDVDR
		{ get { return GetBitmapSource(StockIconId.MediaDVDR); } }

		/// <summary>
		/// DVD-ROM media. SIID_MEDIADVDROM
		/// </summary>
		public static BitmapSource MediaDVDROM
		{ get { return GetBitmapSource(StockIconId.MediaDVDROM); } }

		/// <summary>
		/// CD+ (enhanced audio CD) media. SIID_MEDIACDAUDIOPLUS
		/// </summary>
		public static BitmapSource MediaCDAudioPlus
		{ get { return GetBitmapSource(StockIconId.MediaCDAudioPlus); } }

		/// <summary>
		/// CD-RW media. SIID_MEDIACDRW
		/// </summary>
		public static BitmapSource MediaCDRW
		{ get { return GetBitmapSource(StockIconId.MediaCDRW); } }

		/// <summary>
		/// CD-R media. SIID_MEDIACDR
		/// </summary>
		public static BitmapSource MediaCDR
		{ get { return GetBitmapSource(StockIconId.MediaCDR); } }

		/// <summary>
		/// A writeable CD in the process of being burned. SIID_MEDIACDBURN
		/// </summary>
		public static BitmapSource MediaCDBurn
		{ get { return GetBitmapSource(StockIconId.MediaCDBurn); } }

		/// <summary>
		/// Blank writable CD media. SIID_MEDIABLANKCD
		/// </summary>
		public static BitmapSource MediaBlankCD
		{ get { return GetBitmapSource(StockIconId.MediaBlankCD); } }

		/// <summary>
		/// CD-ROM media. SIID_MEDIACDROM
		/// </summary>
		public static BitmapSource MediaCDROM
		{ get { return GetBitmapSource(StockIconId.MediaCDROM); } }

		/// <summary>
		/// An audio file. SIID_AUDIOFILES
		/// </summary>
		public static BitmapSource AudioFiles
		{ get { return GetBitmapSource(StockIconId.AudioFiles); } }

		/// <summary>
		/// An image file. SIID_IMAGEFILES
		/// </summary>
		public static BitmapSource ImageFiles
		{ get { return GetBitmapSource(StockIconId.ImageFiles); } }

		/// <summary>
		/// A video file. SIID_VIDEOFILES
		/// </summary>
		public static BitmapSource VideoFiles
		{ get { return GetBitmapSource(StockIconId.VideoFiles); } }

		/// <summary>
		/// A mixed file. SIID_MIXEDFILES
		/// </summary>
		public static BitmapSource MixedFiles
		{ get { return GetBitmapSource(StockIconId.MixedFiles); } }

		/// <summary>
		/// Folder back. SIID_FOLDERBACK
		/// </summary>
		public static BitmapSource FolderBack
		{ get { return GetBitmapSource(StockIconId.FolderBack); } }

		/// <summary>
		/// Folder front. SIID_FOLDERFRONT
		/// </summary>
		public static BitmapSource FolderFront
		{ get { return GetBitmapSource(StockIconId.FolderFront); } }

		/// <summary>
		/// Security shield. Use for UAC prompts only. SIID_SHIELD
		/// </summary>
		public static BitmapSource Shield
		{ get { return GetBitmapSource(StockIconId.Shield); } }

		/// <summary>
		/// Warning. SIID_WARNING
		/// </summary>
		public static BitmapSource Warning
		{ get { return GetBitmapSource(StockIconId.Warning); } }

		/// <summary>
		/// Informational. SIID_INFO
		/// </summary>
		public static BitmapSource Info
		{ get { return GetBitmapSource(StockIconId.Info); } }

		/// <summary>
		/// Error. SIID_ERROR
		/// </summary>
		public static BitmapSource Error
		{ get { return GetBitmapSource(StockIconId.Error); } }

		/// <summary>
		/// Key. SIID_KEY
		/// </summary>
		public static BitmapSource Key
		{ get { return GetBitmapSource(StockIconId.Key); } }

		/// <summary>
		/// Software. SIID_SOFTWARE
		/// </summary>
		public static BitmapSource Software
		{ get { return GetBitmapSource(StockIconId.Software); } }

		/// <summary>
		/// A UI item, such as a button, that issues a rename command. SIID_RENAME
		/// </summary>
		public static BitmapSource Rename
		{ get { return GetBitmapSource(StockIconId.Rename); } }

		/// <summary>
		/// A UI item, such as a button, that issues a delete command. SIID_DELETE
		/// </summary>
		public static BitmapSource Delete
		{ get { return GetBitmapSource(StockIconId.Delete); } }

		/// <summary>
		/// Audio DVD media. SIID_MEDIAAUDIODVD
		/// </summary>
		public static BitmapSource MediaAudioDVD
		{ get { return GetBitmapSource(StockIconId.MediaAudioDVD); } }

		/// <summary>
		/// Movie DVD media. SIID_MEDIAMOVIEDVD
		/// </summary>
		public static BitmapSource MediaMovieDVD
		{ get { return GetBitmapSource(StockIconId.MediaMovieDVD); } }

		/// <summary>
		/// Enhanced CD media. SIID_MEDIAENHANCEDCD
		/// </summary>
		public static BitmapSource MediaEnhancedCD
		{ get { return GetBitmapSource(StockIconId.MediaEnhancedCD); } }

		/// <summary>
		/// Enhanced DVD media. SIID_MEDIAENHANCEDDVD
		/// </summary>
		public static BitmapSource MediaEnhancedDVD
		{ get { return GetBitmapSource(StockIconId.MediaEnhancedDVD); } }

		/// <summary>
		/// High definition DVD media in the HD DVD format. SIID_MEDIAHDDVD
		/// </summary>
		public static BitmapSource MediaHDDVD
		{ get { return GetBitmapSource(StockIconId.MediaHDDVD); } }

		/// <summary>
		/// High definition DVD media in the Blu-ray Disc™ format. SIID_MEDIABLURAY
		/// </summary>
		public static BitmapSource MediaBLURAY
		{ get { return GetBitmapSource(StockIconId.MediaBLURAY); } }

		/// <summary>
		/// Video CD (VCD) media. SIID_MEDIAVCD
		/// </summary>
		public static BitmapSource MediaVCD
		{ get { return GetBitmapSource(StockIconId.MediaVCD); } }

		/// <summary>
		/// DVD+R media. SIID_MEDIADVDPLUSR
		/// </summary>
		public static BitmapSource MediaDVDPlusR
		{ get { return GetBitmapSource(StockIconId.MediaDVDPlusR); } }

		/// <summary>
		/// DVD+RW media. SIID_MEDIADVDPLUSRW
		/// </summary>
		public static BitmapSource MediaDVDPlusRW
		{ get { return GetBitmapSource(StockIconId.MediaDVDPlusRW); } }

		/// <summary>
		/// A desktop computer. SIID_DESKTOPPC
		/// </summary>
		public static BitmapSource DesktopPC
		{ get { return GetBitmapSource(StockIconId.DesktopPC); } }

		/// <summary>
		/// A mobile computer (laptop). SIID_MOBILEPC
		/// </summary>
		public static BitmapSource MobilePC
		{ get { return GetBitmapSource(StockIconId.MobilePC); } }

		/// <summary>
		/// The User Accounts Control Panel item. SIID_USERS
		/// </summary>
		public static BitmapSource Users
		{ get { return GetBitmapSource(StockIconId.Users); } }

		/// <summary>
		/// Smart media. SIID_MEDIASMARTMEDIA
		/// </summary>
		public static BitmapSource MediaSmartMedia
		{ get { return GetBitmapSource(StockIconId.MediaSmartMedia); } }

		/// <summary>
		/// CompactFlash media. SIID_MEDIACOMPACTFLASH
		/// </summary>
		public static BitmapSource MediaCompactFlash
		{ get { return GetBitmapSource(StockIconId.MediaCompactFlash); } }

		/// <summary>
		/// A cell phone. SIID_DEVICECELLPHONE
		/// </summary>
		public static BitmapSource DeviceCellPhone
		{ get { return GetBitmapSource(StockIconId.DeviceCellPhone); } }

		/// <summary>
		/// A digital camera. SIID_DEVICECAMERA
		/// </summary>
		public static BitmapSource DeviceCamera
		{ get { return GetBitmapSource(StockIconId.DeviceCamera); } }

		/// <summary>
		/// A digital video camera. SIID_DEVICEVIDEOCAMERA
		/// </summary>
		public static BitmapSource DeviceVideoCamera
		{ get { return GetBitmapSource(StockIconId.DeviceVideoCamera); } }

		/// <summary>
		/// An audio player. SIID_DEVICEAUDIOPLAYER
		/// </summary>
		public static BitmapSource DeviceAudioPlayer
		{ get { return GetBitmapSource(StockIconId.DeviceAudioPlayer); } }

		/// <summary>
		/// Connect to network. SIID_NETWORKCONNECT
		/// </summary>
		public static BitmapSource NetworkConnect
		{ get { return GetBitmapSource(StockIconId.NetworkConnect); } }

		/// <summary>
		/// The Network and Internet Control Panel item. SIID_INTERNET
		/// </summary>
		public static BitmapSource Internet
		{ get { return GetBitmapSource(StockIconId.Internet); } }

		/// <summary>
		/// A compressed file with a .zip file name extension. SIID_ZIPFILE
		/// </summary>
		public static BitmapSource ZipFile
		{ get { return GetBitmapSource(StockIconId.ZipFile); } }

		/// <summary>
		/// The Additional Options Control Panel item. SIID_SETTINGS
		/// </summary>
		public static BitmapSource Settings
		{ get { return GetBitmapSource(StockIconId.Settings); } }

		/// <summary>
		/// Windows Vista with Service Pack 1 (SP1) and later. High definition DVD drive (any type - HD DVD-ROM, HD DVD-R, HD-DVD-RAM) that uses the HD DVD format. SIID_DRIVEHDDVD
		/// </summary>
		public static BitmapSource DriveHDDVD
		{ get { return GetBitmapSource(StockIconId.DriveHDDVD); } }

		/// <summary>
		/// Windows Vista with SP1 and later. High definition DVD drive (any type - BD-ROM, BD-R, BD-RE) that uses the Blu-ray Disc format. SIID_DRIVEBD
		/// </summary>
		public static BitmapSource DriveBD
		{ get { return GetBitmapSource(StockIconId.DriveBD); } }

		/// <summary>
		/// Windows Vista with SP1 and later. High definition DVD-ROM media in the HD DVD-ROM format. SIID_MEDIAHDDVDROM
		/// </summary>
		public static BitmapSource MediaHDDVDROM
		{ get { return GetBitmapSource(StockIconId.MediaHDDVDROM); } }

		/// <summary>
		/// Windows Vista with SP1 and later. High definition DVD-R media in the HD DVD-R format. SIID_MEDIAHDDVDR
		/// </summary>
		public static BitmapSource MediaHDDVDR
		{ get { return GetBitmapSource(StockIconId.MediaHDDVDR); } }

		/// <summary>
		/// Windows Vista with SP1 and later. High definition DVD-RAM media in the HD DVD-RAM format. SIID_MEDIAHDDVDRAM
		/// </summary>
		public static BitmapSource MediaHDDVDRAM
		{ get { return GetBitmapSource(StockIconId.MediaHDDVDRAM); } }

		/// <summary>
		/// Windows Vista with SP1 and later. High definition DVD-ROM media in the Blu-ray Disc BD-ROM format. SIID_MEDIABDROM
		/// </summary>
		public static BitmapSource MediaBDROM
		{ get { return GetBitmapSource(StockIconId.MediaBDROM); } }

		/// <summary>
		/// Windows Vista with SP1 and later. High definition write-once media in the Blu-ray Disc BD-R format. SIID_MEDIABDR
		/// </summary>
		public static BitmapSource MediaBDR
		{ get { return GetBitmapSource(StockIconId.MediaBDR); } }

		/// <summary>
		/// Windows Vista with SP1 and later. High definition read/write media in the Blu-ray Disc BD-RE format. SIID_MEDIABDRE
		/// </summary>
		public static BitmapSource MediaBDRE
		{ get { return GetBitmapSource(StockIconId.MediaBDRE); } }

		/// <summary>
		/// Windows Vista with SP1 and later. A cluster disk array. SIID_CLUSTEREDDRIVE
		/// </summary>
		public static BitmapSource ClusteredDrive
		{ get { return GetBitmapSource(StockIconId.ClusteredDrive); } }
		#endregion
	}

	/// <summary>
	/// Used to indicate any modifications that need to be made to the icon.
	/// </summary>
	[Flags]
	public enum StockIconFlags
	{
		//IconLocation		= 0x00000000, //Implied
		//LargeIcon			= 0x00000000, //Implied
		//Icon				= 0x00000100, //Always added in this project
		//SystemIconIndex	= 0x00004000, //Not needed for this project
		/// <summary>
		/// Retrieves the icon, unmodified.
		/// </summary>
		Default				= 0x00000000,
		/// <summary>
		/// Retrieves the small version of the icon, as specified by the SM_CXSMICON and SM_CYSMICON system metrics.
		/// </summary>
		SmallIcon			= 0x00000001,
		/// <summary>
		/// Retrieves the Shell-sized icons rather than the sizes specified by the system metrics.
		/// </summary>
		ShellIconSize		= 0x00000004,
		/// <summary>
		/// Adds the link overlay to the icon.
		/// </summary>
		LinkOverlay			= 0x00008000,
		/// <summary>
		/// Blends the icon with the system highlight color.
		/// </summary>
		Selected			= 0x00010000,
	}

	/// <summary>
	/// Used by SHGetStockIconInfo to identify which stock system icon to retrieve.
	/// SHSTOCKICONID enumeration.
	/// https://msdn.microsoft.com/en-us/library/windows/desktop/bb762542.aspx
	/// </summary>
	public enum StockIconId : uint
	{
		/// <summary>
		/// Document of a type with no associated application. SIID_DOCNOASSOC
		/// </summary>
		DocNoAssoc         = 0,
		/// <summary>
		/// Document of a type with an associated application. SIID_DOCASSOC
		/// </summary>
		DocAssoc           = 1,
		/// <summary>
		/// Generic application with no custom icon. SIID_APPLICATION
		/// </summary>
		Application        = 2,
		/// <summary>
		/// Folder (generic, unspecified state). SIID_FOLDER
		/// </summary>
		Folder             = 3,
		/// <summary>
		/// Folder (open). SIID_FOLDEROPEN
		/// </summary>
		FolderOpen         = 4,
		/// <summary>
		/// 5.25-inch disk drive. SIID_DRIVE525
		/// </summary>
		Drive525           = 5,
		/// <summary>
		/// 3.5-inch disk drive. SIID_DRIVE35
		/// </summary>
		Drive35            = 6,
		/// <summary>
		/// Removable drive. SIID_DRIVEREMOVE
		/// </summary>
		DriveRemove        = 7,
		/// <summary>
		/// Fixed drive (hard disk). SIID_DRIVEFIXED
		/// </summary>
		DriveFixed         = 8,
		/// <summary>
		/// Network drive (connected). SIID_DRIVENET
		/// </summary>
		DriveNet           = 9,
		/// <summary>
		/// Network drive (disconnected). SIID_DRIVENETDISABLED
		/// </summary>
		DriveNetDisabled   = 10,
		/// <summary>
		/// CD drive. SIID_DRIVECD
		/// </summary>
		DriveCD            = 11,
		/// <summary>
		/// RAM disk drive. SIID_DRIVERAM
		/// </summary>
		DriveRAM           = 12,
		/// <summary>
		/// The entire network. SIID_WORLD
		/// </summary>
		World              = 13,
		/// <summary>
		/// A computer on the network. SIID_SERVER
		/// </summary>
		Server             = 15,
		/// <summary>
		/// A local printer or print destination. SIID_PRINTER
		/// </summary>
		Printer            = 16,
		/// <summary>
		/// The Network virtual folder (FOLDERID_NetworkFolder/CSIDL_NETWORK). SIID_MYNETWORK
		/// </summary>
		MyNetwork          = 17,
		/// <summary>
		/// The Search feature. SIID_FIND
		/// </summary>
		Find               = 22,
		/// <summary>
		/// The Help and Support feature. SIID_HELP
		/// </summary>
		Help               = 23,
		/// <summary>
		/// Overlay for a shared item. SIID_SHARE
		/// </summary>
		Share              = 28,
		/// <summary>
		/// Overlay for a shortcut. SIID_LINK
		/// </summary>
		Link               = 29,
		/// <summary>
		/// Overlay for items that are expected to be slow to access. SIID_SLOWFILE
		/// </summary>
		SlowFile           = 30,
		/// <summary>
		/// The Recycle Bin (empty). SIID_RECYCLER
		/// </summary>
		Recycler           = 31,
		/// <summary>
		/// The Recycle Bin (not empty). SIID_RECYCLERFULL
		/// </summary>
		RecyclerFull       = 32,
		/// <summary>
		/// Audio CD media. SIID_MEDIACDAUDIO
		/// </summary>
		MediaCDAudio       = 40,
		/// <summary>
		/// Security lock. SIID_LOCK
		/// </summary>
		Lock               = 47,
		/// <summary>
		/// A virtual folder that contains the results of a search. SIID_AUTOLIST
		/// </summary>
		AutoList           = 49,
		/// <summary>
		/// A network printer. SIID_PRINTERNET
		/// </summary>
		PrinterNet         = 50,
		/// <summary>
		/// A server shared on a network. SIID_SERVERSHARE
		/// </summary>
		ServerShare        = 51,
		/// <summary>
		/// A local fax printer. SIID_PRINTERFAX
		/// </summary>
		PrinterFax         = 52,
		/// <summary>
		/// A network fax printer. SIID_PRINTERFAXNET
		/// </summary>
		PrinterFaxNet      = 53,
		/// <summary>
		/// A file that receives the output of a Print to file operation. SIID_PRINTERFILE
		/// </summary>
		PrinterFile        = 54,
		/// <summary>
		/// A category that results from a Stack by command to organize the contents of a folder. SIID_STACK
		/// </summary>
		Stack              = 55,
		/// <summary>
		/// Super Video CD (SVCD) media. SIID_MEDIASVCD
		/// </summary>
		MediaSVCD          = 56,
		/// <summary>
		/// A folder that contains only subfolders as child items. SIID_STUFFEDFOLDER
		/// </summary>
		StuffedFolder      = 57,
		/// <summary>
		/// Unknown drive type. SIID_DRIVEUNKNOWN
		/// </summary>
		DriveUnknown       = 58,
		/// <summary>
		/// DVD drive. SIID_DRIVEDVD
		/// </summary>
		DriveDVD           = 59,
		/// <summary>
		/// DVD media. SIID_MEDIADVD
		/// </summary>
		MediaDVD           = 60,
		/// <summary>
		/// DVD-RAM media. SIID_MEDIADVDRAM
		/// </summary>
		MediaDVDRAM        = 61,
		/// <summary>
		/// DVD-RW media. SIID_MEDIADVDRW
		/// </summary>
		MediaDVDRW         = 62,
		/// <summary>
		/// DVD-R media. SIID_MEDIADVDR
		/// </summary>
		MediaDVDR          = 63,
		/// <summary>
		/// DVD-ROM media. SIID_MEDIADVDROM
		/// </summary>
		MediaDVDROM        = 64,
		/// <summary>
		/// CD+ (enhanced audio CD) media. SIID_MEDIACDAUDIOPLUS
		/// </summary>
		MediaCDAudioPlus   = 65,
		/// <summary>
		/// CD-RW media. SIID_MEDIACDRW
		/// </summary>
		MediaCDRW          = 66,
		/// <summary>
		/// CD-R media. SIID_MEDIACDR
		/// </summary>
		MediaCDR           = 67,
		/// <summary>
		/// A writeable CD in the process of being burned. SIID_MEDIACDBURN
		/// </summary>
		MediaCDBurn        = 68,
		/// <summary>
		/// Blank writable CD media. SIID_MEDIABLANKCD
		/// </summary>
		MediaBlankCD       = 69,
		/// <summary>
		/// CD-ROM media. SIID_MEDIACDROM
		/// </summary>
		MediaCDROM         = 70,
		/// <summary>
		/// An audio file. SIID_AUDIOFILES
		/// </summary>
		AudioFiles         = 71,
		/// <summary>
		/// An image file. SIID_IMAGEFILES
		/// </summary>
		ImageFiles         = 72,
		/// <summary>
		/// A video file. SIID_VIDEOFILES
		/// </summary>
		VideoFiles         = 73,
		/// <summary>
		/// A mixed file. SIID_MIXEDFILES
		/// </summary>
		MixedFiles         = 74,
		/// <summary>
		/// Folder back. SIID_FOLDERBACK
		/// </summary>
		FolderBack         = 75,
		/// <summary>
		/// Folder front. SIID_FOLDERFRONT
		/// </summary>
		FolderFront        = 76,
		/// <summary>
		/// Security shield. Use for UAC prompts only. SIID_SHIELD
		/// </summary>
		Shield             = 77,
		/// <summary>
		/// Warning. SIID_WARNING
		/// </summary>
		Warning            = 78,
		/// <summary>
		/// Informational. SIID_INFO
		/// </summary>
		Info               = 79,
		/// <summary>
		/// Error. SIID_ERROR
		/// </summary>
		Error              = 80,
		/// <summary>
		/// Key. SIID_KEY
		/// </summary>
		Key                = 81,
		/// <summary>
		/// Software. SIID_SOFTWARE
		/// </summary>
		Software           = 82,
		/// <summary>
		/// A UI item, such as a button, that issues a rename command. SIID_RENAME
		/// </summary>
		Rename             = 83,
		/// <summary>
		/// A UI item, such as a button, that issues a delete command. SIID_DELETE
		/// </summary>
		Delete             = 84,
		/// <summary>
		/// Audio DVD media. SIID_MEDIAAUDIODVD
		/// </summary>
		MediaAudioDVD      = 85,
		/// <summary>
		/// Movie DVD media. SIID_MEDIAMOVIEDVD
		/// </summary>
		MediaMovieDVD      = 86,
		/// <summary>
		/// Enhanced CD media. SIID_MEDIAENHANCEDCD
		/// </summary>
		MediaEnhancedCD    = 87,
		/// <summary>
		/// Enhanced DVD media. SIID_MEDIAENHANCEDDVD
		/// </summary>
		MediaEnhancedDVD   = 88,
		/// <summary>
		/// High definition DVD media in the HD DVD format. SIID_MEDIAHDDVD
		/// </summary>
		MediaHDDVD         = 89,
		/// <summary>
		/// High definition DVD media in the Blu-ray Disc™ format. SIID_MEDIABLURAY
		/// </summary>
		MediaBLURAY        = 90,
		/// <summary>
		/// Video CD (VCD) media. SIID_MEDIAVCD
		/// </summary>
		MediaVCD           = 91,
		/// <summary>
		/// DVD+R media. SIID_MEDIADVDPLUSR
		/// </summary>
		MediaDVDPlusR      = 92,
		/// <summary>
		/// DVD+RW media. SIID_MEDIADVDPLUSRW
		/// </summary>
		MediaDVDPlusRW     = 93,
		/// <summary>
		/// A desktop computer. SIID_DESKTOPPC
		/// </summary>
		DesktopPC          = 94,
		/// <summary>
		/// A mobile computer (laptop). SIID_MOBILEPC
		/// </summary>
		MobilePC           = 95,
		/// <summary>
		/// The User Accounts Control Panel item. SIID_USERS
		/// </summary>
		Users              = 96,
		/// <summary>
		/// Smart media. SIID_MEDIASMARTMEDIA
		/// </summary>
		MediaSmartMedia    = 97,
		/// <summary>
		/// CompactFlash media. SIID_MEDIACOMPACTFLASH
		/// </summary>
		MediaCompactFlash  = 98,
		/// <summary>
		/// A cell phone. SIID_DEVICECELLPHONE
		/// </summary>
		DeviceCellPhone    = 99,
		/// <summary>
		/// A digital camera. SIID_DEVICECAMERA
		/// </summary>
		DeviceCamera       = 100,
		/// <summary>
		/// A digital video camera. SIID_DEVICEVIDEOCAMERA
		/// </summary>
		DeviceVideoCamera  = 101,
		/// <summary>
		/// An audio player. SIID_DEVICEAUDIOPLAYER
		/// </summary>
		DeviceAudioPlayer  = 102,
		/// <summary>
		/// Connect to network. SIID_NETWORKCONNECT
		/// </summary>
		NetworkConnect     = 103,
		/// <summary>
		/// The Network and Internet Control Panel item. SIID_INTERNET
		/// </summary>
		Internet           = 104,
		/// <summary>
		/// A compressed file with a .zip file name extension. SIID_ZIPFILE
		/// </summary>
		ZipFile            = 105,
		/// <summary>
		/// The Additional Options Control Panel item. SIID_SETTINGS
		/// </summary>
		Settings           = 106,
		/// <summary>
		/// Windows Vista with Service Pack 1 (SP1) and later. High definition DVD drive (any type - HD DVD-ROM, HD DVD-R, HD-DVD-RAM) that uses the HD DVD format. SIID_DRIVEHDDVD
		/// </summary>
		DriveHDDVD         = 132,
		/// <summary>
		/// Windows Vista with SP1 and later. High definition DVD drive (any type - BD-ROM, BD-R, BD-RE) that uses the Blu-ray Disc format. SIID_DRIVEBD
		/// </summary>
		DriveBD            = 133,
		/// <summary>
		/// Windows Vista with SP1 and later. High definition DVD-ROM media in the HD DVD-ROM format. SIID_MEDIAHDDVDROM
		/// </summary>
		MediaHDDVDROM      = 134,
		/// <summary>
		/// Windows Vista with SP1 and later. High definition DVD-R media in the HD DVD-R format. SIID_MEDIAHDDVDR
		/// </summary>
		MediaHDDVDR        = 135,
		/// <summary>
		/// Windows Vista with SP1 and later. High definition DVD-RAM media in the HD DVD-RAM format. SIID_MEDIAHDDVDRAM
		/// </summary>
		MediaHDDVDRAM      = 136,
		/// <summary>
		/// Windows Vista with SP1 and later. High definition DVD-ROM media in the Blu-ray Disc BD-ROM format. SIID_MEDIABDROM
		/// </summary>
		MediaBDROM         = 137,
		/// <summary>
		/// Windows Vista with SP1 and later. High definition write-once media in the Blu-ray Disc BD-R format. SIID_MEDIABDR
		/// </summary>
		MediaBDR           = 138,
		/// <summary>
		/// Windows Vista with SP1 and later. High definition read/write media in the Blu-ray Disc BD-RE format. SIID_MEDIABDRE
		/// </summary>
		MediaBDRE          = 139,
		/// <summary>
		/// Windows Vista with SP1 and later. A cluster disk array. SIID_CLUSTEREDDRIVE
		/// </summary>
		ClusteredDrive     = 140,
		/// <summary>
		/// The highest valid value in the enumeration. Values over 160 are Windows 7-only icons. SIID_MAX_ICONS
		/// </summary>
		MAX_ICONS          = 175,
		///// <summary>
		///// With a value of -1, indicates an invalid <see cref="StockIconId"/> value. SIID_INVALID
		///// </summary>
		//Invalid = -1
	}
}
