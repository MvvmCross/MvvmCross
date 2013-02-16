using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Binding.Touch.Views;
using MonoTouch.ObjCRuntime;

namespace Cirrious.Conference.UI.Touch
{
	[Register("TweetCell3")]
    public partial class TweetCell3
            : MvxBaseTableViewCell
    {
        public static NSString Identifier = new NSString("TweetCell3");
        public const string BindingText = @"{
'SelectedCommand':{'Path':'Command'},
'ImageUrl':{'Path':'Item.ProfileImageUrl'},
'Author':{'Path':'Item.Author'},
'Content':{'Path':'Item.Title'},
'When':{'Path':'Item.Timestamp','Converter':'TimeAgo'}
}";

		private MvxImageViewWrapper _imageWrapper;
                
        public TweetCell3(IntPtr handle)
            : base(BindingText, handle)
        {
			Initialise();
        }		
        
        public TweetCell3 ()
            : base(BindingText)
        {
			Initialise();
        }
		        
		private void Initialise ()
		{
			_imageWrapper = new MvxImageViewWrapper(() => ProfileImage);
		}

        protected override void Dispose (bool disposing)
        {
            if (disposing)
            {
				_imageWrapper.Dispose();

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
        
		public string ImageUrl
		{
			get { return _imageWrapper.ImageUrl; }
			set { _imageWrapper.ImageUrl = value; }
		}
		
		public string Author
        {
            get { return AuthorLabel.Text; }
            set { AuthorLabel.Text = value; }
        }
        
        public string When
        {
            get { return WhenLabel.Text; }
            set { WhenLabel.Text = value; }
        }
        
        public string Content
        {
            get { return ContentLabel.Text; }
            set { ContentLabel.Text = value; ContentLabel.SizeToFit(); }
        }
    }
}

