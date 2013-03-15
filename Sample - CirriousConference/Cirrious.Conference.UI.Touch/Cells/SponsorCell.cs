using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Binding.Touch.Views;
using MonoTouch.ObjCRuntime;

namespace Cirrious.Conference.UI.Touch
{
#warning Don't use this code a reference - use http://slodge.blogspot.co.uk/2013/01/uitableviewcell-using-xib-editor.html
	public partial class SponsorCell : MvxStandardTableViewCell
	{
		public static NSString Identifier = new NSString("SponsorCell");
		public const string BindingText = "ImagePath Item.Image; SelectedCommand Command";
		
		public static SponsorCell LoadFromNib(NSObject owner)
		{
			// this bizarre loading sequence is modified from a blog post on AlexYork.net
			// basically we create an empty cell in C#, then pass that through a NIB loading, which then magically
			// gives us a new cell back in MonoTouch again
			var views = NSBundle.MainBundle.LoadNib("SponsorCell", owner, null);
			var cell2 = Runtime.GetNSObject( views.ValueAt(0) ) as SponsorCell;
			views = null;
			cell2.Initialise();
			return cell2;
		}
		
		public SponsorCell(IntPtr handle)
			: base(BindingText, handle)
		{
		}		
		
		public SponsorCell ()
			: base(BindingText, MonoTouch.UIKit.UITableViewCellStyle.Default, Identifier)
		{
		}

		public SponsorCell (string bindingText)
			: base(bindingText, MonoTouch.UIKit.UITableViewCellStyle.Default, Identifier)
		{
		}
		
		private void Initialise()
		{
			ContentView.BackgroundColor = UIColor.White;
			//this.BackgroundView = new UIView(Frame){ BackgroundColor = UIColor.LightGray };
		}	
			
		protected override void Dispose (bool disposing)
		{
			if (disposing)
			{
				// TODO - really not sure that Dispose is the right place for this call 
				// - but couldn't see how else to do this in a TableViewCell
				ReleaseDesignerOutlets();
			}
			
			base.Dispose (disposing);
		} 

		public override string ReuseIdentifier 
		{
			get 
			{
				return Identifier.ToString();
			}
		}

	    private string _imagePath;
        public string ImagePath
		{
            get { return _imagePath; }
			set
			{
                if (_imagePath == value)
                    return;
			    _imagePath = value;
                if (TheImage != null) TheImage.Image = UIImage.FromFile("ConfResources/SponsorImages/" + _imagePath);
			}
		}
	}
}

