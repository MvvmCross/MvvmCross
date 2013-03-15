
using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;
using Cirrious.MvvmCross.Mac.Views;
using TwitterSearch.Core.ViewModels;

namespace TwitterSearch.UI.Mac
{
	public partial class HomeViewController : MvxViewController
	{
		#region Constructors
		
		// Called when created from unmanaged code
		public HomeViewController (IntPtr handle) : base (handle)
		{
			Initialize ();
		}
		
		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public HomeViewController (NSCoder coder) : base (coder)
		{
			Initialize ();
		}
		
		// Call to load from the XIB/NIB file
		public HomeViewController () : base ("HomeView", NSBundle.MainBundle)
		{
			Initialize ();
		}
		
		// Shared initialization code
		void Initialize ()
		{
		}
		
		#endregion
		
		//strongly typed view accessor
		public new HomeView View {
			get {
				return (HomeView)base.View;
			}
		}

		/*
		public new HomeViewModel ViewModel
		{
			get { return (HomeViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}
		*/
	}
}

