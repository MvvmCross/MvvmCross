using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;
using Cirrious.MvvmCross.Mac.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using DevDemo.Core.ViewModels;

namespace DevDemo.Mac
{
	public partial class DevViewController : MvxViewController
	{
		#region Constructors
		// Called when created from unmanaged code
		public DevViewController (IntPtr handle) : base (handle)
		{
			Initialize ();
		}
		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public DevViewController (NSCoder coder) : base (coder)
		{
			Initialize ();
		}
		// Call to load from the XIB/NIB file
		public DevViewController () : base ("DevView", NSBundle.MainBundle)
		{
			Initialize ();
		}
		// Shared initialization code
		void Initialize ()
		{
		}
		#endregion
		//strongly typed view accessor
		public new DevView View {
			get {
				return (DevView)base.View;
			}
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			var set = this.CreateBindingSet<DevViewController, DevViewModel> ();
			set.Bind (devTextField).To (vm => vm.Hello);
			set.Bind (devMultiTextField).To (vm => vm.Hello);
//			devTextView.TextStorage.SetString(new NSAttributedString("Hello also"));
			set.Bind (devTextView).To (vm => vm.Lorem);		
			set.Bind (devTextView2).To (vm => vm.Lorem);

			set.Apply ();
		}
	}
}

