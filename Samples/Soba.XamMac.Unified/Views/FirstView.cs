using System;
using System.Collections.Generic;
using System.Linq;
using AppKit;
using Cirrious.MvvmCross.Binding.Mac.Views;
using Cirrious.MvvmCross.ViewModels;
using Foundation;
using Soba.Core.ViewModels;

namespace Soba.XamMac.Unified
{
	[MvxViewFor(typeof(FirstViewModel))]
	public partial class FirstView : MvxView
	{
		#region Constructors

		// Called when created from unmanaged code
		public FirstView (IntPtr handle) : base (handle)
		{
			Initialize ();
		}

		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public FirstView (NSCoder coder) : base (coder)
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
