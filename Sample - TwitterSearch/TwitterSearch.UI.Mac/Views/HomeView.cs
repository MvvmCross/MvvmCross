
using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;
using Cirrious.MvvmCross.Mac.Views;
using TwitterSearch.Core.ViewModels;

namespace TwitterSearch.UI.Mac
{
	public partial class HomeView : NSView
	{
		#region Constructors
		
		// Called when created from unmanaged code
		public HomeView (IntPtr handle) : base (handle)
		{
			Initialize ();
		}
		
		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public HomeView (NSCoder coder) : base (coder)
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

