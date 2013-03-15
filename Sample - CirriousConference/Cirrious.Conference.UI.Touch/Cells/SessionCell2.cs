using System;
using System.Drawing;
using Cirrious.Conference.UI.Touch.Bindings;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Binding.Touch.Views;
using MonoTouch.ObjCRuntime;

namespace Cirrious.Conference.UI.Touch
{
#warning Don't use this code a reference - use http://slodge.blogspot.co.uk/2013/01/uitableviewcell-using-xib-editor.html
	public partial class SessionCell2 : MvxStandardTableViewCell
	{
		public static NSString Identifier = new NSString("SessionCell2");
        public const string BindingText = @"
SpeakerText Item.Session.SpeakerKey;
MainText Item.Session.Title;
RoomText Item.Session,Converter=SessionSmallDetails,ConverterParameter='SmallDetailsFormat';
SelectedCommand Command;
IsFavorite Item.IsFavorite
";
		
		public static SessionCell2 LoadFromNib(NSObject owner)
		{
			// this bizarre loading sequence is modified from a blog post on AlexYork.net
			// basically we create an empty cell in C#, then pass that through a NIB loading, which then magically
			// gives us a new cell back in MonoTouch again
			var views = NSBundle.MainBundle.LoadNib("SessionCell2", owner, null);
			var cell2 = Runtime.GetNSObject( views.ValueAt(0) ) as SessionCell2;
			views = null;
			cell2.Initialise();
			return cell2;
		}
		
		public SessionCell2(IntPtr handle)
			: base(BindingText, handle)
		{
		}		
		
		public SessionCell2 ()
			: base(BindingText, MonoTouch.UIKit.UITableViewCellStyle.Default, Identifier)
		{
		}

		public SessionCell2 (string bindingText)
			: base(bindingText, MonoTouch.UIKit.UITableViewCellStyle.Default, Identifier)
		{
		}
		
		private void Initialise()
		{
			Image1.Image = UIImage.FromFile("ConfResources/Images/appbar.people.png");
			Image2.Image = UIImage.FromFile("ConfResources/Images/appbar.city.png");
            FavoritesButton.TouchUpInside += HandleFavoritesButtonTouchDown;
			TitleLabel.Lines = 2;
			TitleLabel.AdjustsFontSizeToFitWidth = false;
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
		
		public event EventHandler PublicFavoritesButtonPressed;
		
		void HandleFavoritesButtonTouchDown (object sender, EventArgs e)
		{
			var handler = PublicFavoritesButtonPressed;
			if (handler != null)
				handler(this, EventArgs.Empty);			
		}	
		
		public override string ReuseIdentifier 
		{
			get 
			{
				return Identifier.ToString();
			}
		}
		
		public string MainText
		{
			get { return TitleLabel.Text; }
			set { if (TitleLabel != null) TitleLabel.Text = value; }
		}
		
		public string SpeakerText
		{
			get { return Label1.Text; }
			set { if (Label1 != null) Label1.Text = value; }
		}
		
		public string RoomText
		{
			get { return Label2.Text; }
			set { if (Label2 != null) Label2.Text = value; }
		}

	    public UIButton PublicFavoritesButton
	    {
            get { return FavoritesButton; }
	    }
	}
}

