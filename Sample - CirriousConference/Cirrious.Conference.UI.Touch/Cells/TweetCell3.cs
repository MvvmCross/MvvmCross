using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Binding.Touch.Views;
using MonoTouch.ObjCRuntime;

namespace Cirrious.Conference.UI.Touch
{
    public partial class TweetCell3
            : MvxBindableTableViewCell
    {
        public static NSString Identifier = new NSString("TweetCell3");
        public const string BindingText = @"
{
'SelectedCommand':{'Path':'Command'},
'HttpImageUrl':{'Path':'Item.ProfileImageUrl'},
'Author':{'Path':'Item.Author'},
'Content':{'Path':'Item.Title'},
'When':{'Path':'Item.Timestamp','Converter':'TimeAgo'}
}";
        
        public static TweetCell3 LoadFromNib(NSObject owner)
        {
            // this bizarre loading sequence is modified from a blog post on AlexYork.net
            // basically we create an empty cell in C#, then pass that through a NIB loading, which then magically
            // gives us a new cell back in MonoTouch again
            var views = NSBundle.MainBundle.LoadNib("TweetCell3", owner, null);
            var cell2 = Runtime.GetNSObject( views.ValueAt(0) ) as TweetCell3;
			views = null;
            cell2.Initialise();
            return cell2;
        }
        
        public TweetCell3(IntPtr handle)
            : base(BindingText, handle)
        {
        }		
        
        public TweetCell3 ()
            : base(BindingText, MonoTouch.UIKit.UITableViewCellStyle.Default, Identifier)
        {
        }

        public TweetCell3 (string bindingText)
            : base(bindingText, MonoTouch.UIKit.UITableViewCellStyle.Default, Identifier)
        {
        }
        
        private void Initialise()
        {
            AuthorLabel.Lines = 1;
            AuthorLabel.AdjustsFontSizeToFitWidth = false;
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
        
        public override UIImageView ImageView {
            get 
            {
                return ProfileImage;
            }
        }
        public string Author
        {
            get { return AuthorLabel.Text; }
            set { if (AuthorLabel != null) AuthorLabel.Text = value; }
        }
        
        public string When
        {
            get { return WhenLabel.Text; }
            set { if (WhenLabel != null) WhenLabel.Text = value; }
        }
        
        public string Content
        {
            get { return ContentLabel.Text; }
            set { if (ContentLabel != null) { ContentLabel.Text = value; ContentLabel.SizeToFit(); } }
        }
    }
}

