using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Binding.Touch.Views;
using MonoTouch.ObjCRuntime;

namespace Cirrious.Conference.UI.Touch
{
#warning Don't use this code a reference - use http://slodge.blogspot.co.uk/2013/01/uitableviewcell-using-xib-editor.html
    public partial class SeparatorCell : MvxStandardTableViewCell
    {
        public static NSString Identifier = new NSString("SeparatorCell");
        public const string BindingText = "MainText Converter=SimpleDate";
        
        public static SeparatorCell LoadFromNib()
        {
            // this bizarre loading sequence is modified from a blog post on AlexYork.net
            // basically we create an empty cell in C#, then pass that through a NIB loading, which then magically
            // gives us a new cell back in MonoTouch again
            var cell = new SeparatorCell("{}");
            var views = NSBundle.MainBundle.LoadNib("SeparatorCell", cell, null);
            var cell2 = Runtime.GetNSObject( views.ValueAt(0) ) as SeparatorCell;
            cell2.Initialise();
            return cell2;
        }
        
        public SeparatorCell(IntPtr handle)
            : base(BindingText, handle)
        {
        }		
        
        public SeparatorCell ()
            : base(BindingText, MonoTouch.UIKit.UITableViewCellStyle.Default, Identifier)
        {
        }

        public SeparatorCell (string bindingText)
            : base(bindingText, MonoTouch.UIKit.UITableViewCellStyle.Default, Identifier)
        {
        }
        
        private void Initialise()
        {
            ContentView.BackgroundColor = UIColor.LightGray;
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
        
        public string MainText
        {
            get { return TitleLabel.Text; }
            set { if (TitleLabel != null) TitleLabel.Text = value; }
        }
    }
}

