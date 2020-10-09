// Copyright (c) Foretold Software, LLC. All rights reserved. Licensed under the Microsoft Public License (MS-PL). See the license.md file in the project root directory for full license information.

namespace System
{
	public class DisposableSingletonTestType : DisposableSingleton<DisposableSingletonTestType>
	{
		public bool IsDisposed = false;
		public static bool WasDisposed = false;

		protected override void Dispose(bool disposing)
		{
			this.IsDisposed = true;
			base.Dispose(disposing);
		}

		protected override void FreeManagedResources()
		{
			WasDisposed = true;
		}
		protected override void FreeUnmanagedResources()
		{
			WasDisposed = true;
		}
	}

	public class DisposableSingletonTestType2 : DisposableSingleton<DisposableSingletonTestType2>
	{
		public bool HasDisposed = false;

		protected override void Dispose(bool disposing)
		{
			this.HasDisposed = true;
			base.Dispose(disposing);
		}
	}

	public class DisposableSingletonTestTypeNoCtor : DisposableSingleton<DisposableSingletonTestTypeNoCtor>
	{
		private DisposableSingletonTestTypeNoCtor() { }
	}
}
