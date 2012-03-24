using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Binding.Touch.Views;
using MonoTouch.ObjCRuntime;

namespace Cirrious.Conference.UI.Touch
{
	[Register ("TweetCell")]
	public partial class TweetCell : MvxBindableTableViewCell
	{
		public static NSString Identifier = new NSString("TweetCell");
        public const string CellBindingText = "{'AuthorText':{'Path':'Author'},'ContentText':{'Path':'Title'},'WhenText':{'Path':'Timestamp','Converter':'TimeAgo'},'HttpImageUrl':{'Path':'ProfileImageUrl'}}";
		
		public static TweetCell LoadFromNib()
		{
			// this bizarre loading sequence is modified from a blog post on AlexYork.net
			// basically we create an empty cell in C#, then pass that through a NIB loading, which then magically
			// gives us a new cell back in MonoTouch again
			
			var cell = new TweetCell("{}");
			var views = NSBundle.MainBundle.LoadNib("TweetCell", cell, null);
			var view0 = Runtime.GetNSObject( views.ValueAt(0) );
			var cell2 = view0 as TweetCell;
			return cell2;
		}
		
		public TweetCell(IntPtr handle)
			: base(CellBindingText, handle)
		{
		}		
		
		public TweetCell ()
			: base(CellBindingText, MonoTouch.UIKit.UITableViewCellStyle.Default, Identifier)
		{
		}

		public TweetCell (string bindingText)
			: base(bindingText, MonoTouch.UIKit.UITableViewCellStyle.Default, Identifier)
		{
		}
		
		protected override void Dispose (bool isDisposing)
		{
			if (isDisposing)
			{
				ReleaseDesignerOutlets ();
			}
			base.Dispose(isDisposing);
		}
		
		public override string ReuseIdentifier 
		{
			get 
			{
				return Identifier.ToString();
			}
		}
				
		public string AuthorText
		{
			get { return Name.Text; }
			set { if (Name != null) Name.Text = value; }
		}
		
		public string WhenText
		{
			get { return When.Text; }
			set { if (When != null) When.Text = value; }
		}
		
		public string ContentText
		{
			get { return Detail.Text; }
			set { if (Detail != null) Detail.Text = value; }
		}
		
		public override UIImageView ImageView {
			get {
				return ProfileImage;
			}
		}
	}
}

