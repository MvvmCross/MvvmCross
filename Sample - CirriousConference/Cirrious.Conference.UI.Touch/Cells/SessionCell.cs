using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Binding.Touch.Views;
using MonoTouch.ObjCRuntime;

namespace Cirrious.Conference.UI.Touch
{
#warning Don't use this code a reference - use http://slodge.blogspot.co.uk/2013/01/uitableviewcell-using-xib-editor.html
	public partial class SessionCell : MvxStandardTableViewCell
	{
		public static NSString Identifier = new NSString("SessionCell");
		public const string BindingText = "SpeakerText Item.Session.SpeakerKey;TitleText Item.Session.Title; RoomText Item.Session,Converter=SessionSmallDetails";
		
		public static SessionCell LoadFromNib()
		{
			// this bizarre loading sequence is modified from a blog post on AlexYork.net
			// basically we create an empty cell in C#, then pass that through a NIB loading, which then magically
			// gives us a new cell back in MonoTouch again
			var cell = new SessionCell("{}");
			var views = NSBundle.MainBundle.LoadNib("SessionCell", cell, null);
			var cell2 = Runtime.GetNSObject( views.ValueAt(0) ) as SessionCell;
			cell.Initialise();
			return cell;
		}
		
		public SessionCell(IntPtr handle)
			: base(BindingText, handle)
		{
		}		
		
		public SessionCell ()
			: base(BindingText, MonoTouch.UIKit.UITableViewCellStyle.Default, Identifier)
		{
		}

		public SessionCell (string bindingText)
			: base(bindingText, MonoTouch.UIKit.UITableViewCellStyle.Default, Identifier)
		{
		}
		
		private void Initialise()
		{
			ImageView1.Image = UIImage.FromFile("ConfResources/Images/appbar.people.png");
			ImageView2.Image = UIImage.FromFile("ConfResources/Images/appbar.city.png");
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
		
		public string TitleText
		{
			get { return MainLabel.Text; }
			set { if (MainLabel != null) MainLabel.Text = value; }
		}
		
		public string SpeakerText
		{
			get { return SubLabel1.Text; }
			set { if (SubLabel1 != null) SubLabel1.Text = value; }
		}
		
		public string RoomText
		{
			get { return SubLabel2.Text; }
			set { if (SubLabel2 != null) SubLabel2.Text = value; }
		}
	}
}

