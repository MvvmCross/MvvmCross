using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;
using Cirrious.MvvmCross.Mac.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using FirstDemo.Core.ViewModels;
using System.Drawing;
using Cirrious.MvvmCross.ViewModels;
using Loqu8.KVC.Mac;

namespace FirstDemo.Mac
{
	[MvxViewFor(typeof(FirstViewModel))]
    public partial class FirstViewController : MvxViewController
	{
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
		public FirstViewController () : base ()
		{
			Initialize ();
		}
		// Shared initialization code
		void Initialize ()
		{
		}
		#endregion

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			var set = this.CreateBindingSet<FirstViewController, FirstViewModel> ();
			set.Bind (tfFirst).For(v => v.StringValue).To (vm => vm.FirstName);
			set.Bind (tfLast).For(v => v.StringValue).To (vm => vm.LastName);
			set.Bind (tfCombined).For (v => v.StringValue).To (vm => vm.FullName);

			set.Bind (cvContacts).To (vm => vm.Contacts);
			set.Bind (tvContacts).For (v => v.WeakDataSource).To (vm => vm.Contacts);
			set.Bind (ovContacts).For (v => v.WeakDataSource).To (vm => vm.Contacts);
			set.Apply ();
		}
	}
}

