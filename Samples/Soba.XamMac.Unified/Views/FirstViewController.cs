using System;
using System.Collections.Generic;
using System.Linq;
using AppKit;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Mac.Views;
using Foundation;
using Soba.Core.ViewModels;

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
			set.Bind (isOnButton).For(v => v.State).To (vm => vm.IsOn);		// need to match "state" to boolean
			set.Bind (valSlider).For(v => v.IntValue).To (vm => vm.Value);		// need to match "state" to boolean
			set.Bind (valLabel).For (v => v.StringValue).To (vm => vm.Value);
			set.Bind (msgTextField).For(v => v.StringValue).To (vm => vm.Msg);		// need to match "state" to boolean
			set.Bind (msgLabel).For (v => v.StringValue).To (vm => vm.Msg);
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
