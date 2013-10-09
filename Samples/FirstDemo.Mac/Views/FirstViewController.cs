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

namespace FirstDemo.Mac
{
	[MvxViewFor(typeof(SecondViewModel))]
	public class SecondViewController : MvxViewController
	{
		public override void ViewDidLoad ()
		{
			View = new NSView (new RectangleF (0, 0, 320, 400));
			base.ViewDidLoad ();

			var textEditFirst = new NSTextField(new System.Drawing.RectangleF(0,0,320,40));
			View.AddSubview (textEditFirst);
			var textEditSecond = new NSTextField(new System.Drawing.RectangleF(0,50,320,40));
			View.AddSubview(textEditSecond);
			var slider = new NSSlider(new System.Drawing.RectangleF(0,150,320,40));
			slider.MinValue = 0;
			slider.MaxValue = 100;
			slider.IntValue = 23;
			View.AddSubview(slider);
			var labelFull = new NSTextField(new System.Drawing.RectangleF(0,100,320,40));
			labelFull.Editable = false;
			labelFull.Bordered = false;
			labelFull.AllowsEditingTextAttributes = false;
			labelFull.DrawsBackground = false;
			View.AddSubview (labelFull);
			var sw = new NSButton(new RectangleF(0,200,320,40));
			sw.SetButtonType (NSButtonType.Switch);
			View.AddSubview (sw);
			//sw.AddObserver()

			var set = this.CreateBindingSet<SecondViewController, SecondViewModel> ();
			set.Bind (textEditFirst).For(v => v.StringValue).To (vm => vm.FirstName);
			set.Bind (textEditSecond).For(v => v.StringValue).To (vm => vm.LastName);
			set.Bind (labelFull).Described("SliderValue + ' ' + OnOffValue").For("StringValue");	
			set.Bind (slider).For("IntValue").To (vm => vm.SliderValue);
			set.Bind (sw).For(c => c.State).To (vm => vm.OnOffValue);


			set.Apply ();
		}
	}


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
			View = new NSView(new RectangleF(0,100,320, 400));
			base.ViewDidLoad ();

			var textEditFirst = new NSTextField(new System.Drawing.RectangleF(10,0,320,40));
			View.AddSubview (textEditFirst);
			var textEditSecond = new NSTextField(new System.Drawing.RectangleF(10,50,320,40));
			View.AddSubview(textEditSecond);
			var labelFull = new NSTextField(new System.Drawing.RectangleF(10,100,320,40));
			View.AddSubview (labelFull);
			var bu = new NSButton (new RectangleF (0, 150, 320, 40));
			bu.Title = "Hello";
			View.AddSubview (bu);

			var set = this.CreateBindingSet<FirstViewController, FirstViewModel> ();
			set.Bind (textEditFirst).For(v => v.StringValue).To (vm => vm.FirstName);
			set.Bind (textEditSecond).For(v => v.StringValue).To (vm => vm.LastName);
			set.Bind (labelFull).For(v => v.StringValue).To (vm => vm.FullName);	
			set.Bind (bu).For("Activated").To ("GoCommand");
			set.Apply ();
		}
	}
}

