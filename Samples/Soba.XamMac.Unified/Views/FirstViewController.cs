using System;
using System.Collections.Generic;
using System.Linq;
using AppKit;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Mac.Views;
using Foundation;
using Soba.Core.ViewModels;
using System.Diagnostics;

namespace Soba.XamMac.Unified
{
	public partial class FirstViewController : MvxViewController
	{
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			valSlider.MinValue = 0;
			valSlider.MaxValue = 10;

			var set = this.CreateBindingSet<FirstViewController, FirstViewModel> ();
			set.Bind (isOnButton).For(v => v.State).To (vm => vm.IsOn);
			set.Bind (isOnLabel).For (v => v.Hidden).To (vm => vm.IsHidden);
			set.Bind (valSlider).To (vm => vm.Value);
			set.Bind (valLabel).To (vm => vm.Value);
			set.Bind (msgTextField).To (vm => vm.Msg);
			set.Bind (msgLabel).To (vm => vm.Msg);
			set.Bind (querySearchField).To (vm => vm.Query);
			set.Bind (queryLabel).To (vm => vm.Query);
			set.Bind (goButton).To (vm => vm.GoCommand);
			set.Bind (fruitSegControl).To (vm => vm.Selected);
			set.Bind (fruitLabel).To (vm => vm.Selected);
			set.Bind (datePicker).For ("Date").To (vm => vm.Date);
//			set.Bind (datePicker).For ("Time").To (vm => vm.Time);
			set.Bind (dateLabel).To (vm => vm.Date);
//			set.Bind (timeLabel).To (vm => vm.Time);
			set.Apply ();
		}

		#region Constructors

		// Called when created from unmanaged code
		public FirstViewController (IntPtr handle) : base (handle)
		{
			Initialize ();
		}

		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public FirstViewController (NSCoder coder) : base (coder)
		{
			Initialize ();
		}

		// Call to load from the XIB/NIB file
		public FirstViewController () : base ("FirstView", NSBundle.MainBundle)
		{
			Initialize ();
		}

		// Shared initialization code
		void Initialize ()
		{
		}

		#endregion

		//strongly typed view accessor
		public new FirstView View {
			get {
				return (FirstView)base.View;
			}
		}
	}
}
