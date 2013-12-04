using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;
using Cirrious.MvvmCross.Mac.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.ViewModels;
using <root>.Core.ViewModels;

namespace $rootnamespace$.Views
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

		// strongly typed view accessor
		public new FirstView View {
			get {
				return (FirstView)base.View;
			}
		}
		
		public override void ViewDidLoad ()
		{
s			base.ViewDidLoad ();

			var set = this.CreateBindingSet<FirstViewController, FirstViewModel> ();
			set.Bind (textFirst).For(v => v.StringValue).To (vm => vm.Hello);
			set.Apply ();
		}
	}
}

