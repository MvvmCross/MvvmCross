using System;
using Cirrious.MvvmCross.Binding.Touch.Views;
using MonoTouch.Foundation;
using MonoTouch.ObjCRuntime;

namespace Tutorial.UI.Touch
{
	[Register("PullToRefreshTableCellView")]
	public partial class PullToRefreshTableCellView 
		: MvxStandardTableViewCell
	{
		public static NSString Identifier = new NSString("PullToRefreshTableCellView");
		public const string BindingText = @"From From;Header Header;Message Message";
		
		public static PullToRefreshTableCellView LoadFromNib()
		{
			// this bizarre loading sequence is modified from a blog post on AlexYork.net
			// basically we create an empty cell in C#, then pass that through a NIB loading, which then magically
			// gives us a new cell back in MonoTouch again
			var cell = new PullToRefreshTableCellView("{}");
			var views = NSBundle.MainBundle.LoadNib("PullToRefreshTableCellView", cell, null);
			var cell2 = Runtime.GetNSObject( views.ValueAt(0) ) as PullToRefreshTableCellView;
			return cell2;
		}
		
		public PullToRefreshTableCellView(IntPtr handle)
			: base(BindingText, handle)
		{
		}		
		
		public PullToRefreshTableCellView ()
			: base(BindingText, MonoTouch.UIKit.UITableViewCellStyle.Default, Identifier)
		{
		}

		public PullToRefreshTableCellView (string bindingText)
			: base(bindingText, MonoTouch.UIKit.UITableViewCellStyle.Default, Identifier)
		{
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
		
		public string From
		{
			get { return FromLabel.Text; }
			set { if (FromLabel != null) FromLabel.Text = value; }
		}
		
		public string Header
		{
			get { return HeaderLabel.Text; }
			set { if (HeaderLabel != null) HeaderLabel.Text = value; }
		}
		
		public string Message
		{
			get { return MessageLabel.Text; }
			set { if (MessageLabel != null) MessageLabel.Text = value; }
		}
		
	}
}

