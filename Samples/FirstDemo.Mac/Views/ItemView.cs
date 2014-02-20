using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace FirstDemo.Mac
{
	public partial class ItemView : MonoMac.AppKit.NSView
	{
		#region Constructors

		// Called when created from unmanaged code
		public ItemView (IntPtr handle) : base (handle)
		{
			Initialize ();
		}
		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public ItemView (NSCoder coder) : base (coder)
		{
			Initialize ();
		}
		// Shared initialization code
		void Initialize ()
		{
		}

		#endregion
	}
}

