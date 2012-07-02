using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Dialog.Touch.Dialog.Elements
{
    public class DateTimeElement : ValueElement<DateTime>
    {
        private static readonly NSString Key = new NSString("DateTimeElement");

        private UIDatePicker _datePicker;		
        private NSDateFormatter _formatter= new NSDateFormatter () {
                                                                            DateStyle = NSDateFormatterStyle.Short
                                                                        };
        protected NSDateFormatter Formatter
        {
            get { return _formatter; }
            set { _formatter = value; }
        }

        public DateTimeElement (string caption, DateTime date) 
            : base (caption, date)
        {
        }	
		
        protected override UITableViewCell GetCellImpl (UITableView tv)
        {
            var cell = tv.DequeueReusableCell(Key);

            if (cell == null)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Value1, Key);
                cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
            }
            UpdateDetailDisplay(cell);
            return cell;
        }
 
        protected override void Dispose (bool disposing)
        {
            base.Dispose (disposing);
            if (disposing){
                if (Formatter != null){
                    Formatter.Dispose ();
                    Formatter = null;
                }
                if (_datePicker != null){
                    _datePicker.Dispose ();
                    _datePicker = null;
                }
            }
        }
		
        public virtual string FormatDate (DateTime dt)
        {
            return Formatter.ToString (dt) + " " + dt.ToLocalTime ().ToShortTimeString ();
        }
		
        public virtual UIDatePicker CreatePicker ()
        {
            var picker = new UIDatePicker (RectangleF.Empty){
                                                                AutoresizingMask = UIViewAutoresizing.FlexibleWidth,
                                                                Mode = UIDatePickerMode.DateAndTime,
                                                                Date = Value
                                                            };
            return picker;
        }
		                                                                                                                                
        static RectangleF PickerFrameWithSize (SizeF size)
        {                                                                                                                                    
            var screenRect = UIScreen.MainScreen.ApplicationFrame;
            float fY = 0, fX = 0;
			
            switch (UIApplication.SharedApplication.StatusBarOrientation){
                case UIInterfaceOrientation.LandscapeLeft:
                case UIInterfaceOrientation.LandscapeRight:
                    fX = (screenRect.Height - size.Width) /2;
                    fY = (screenRect.Width - size.Height) / 2 -17;
                    break;
				
                case UIInterfaceOrientation.Portrait:
                case UIInterfaceOrientation.PortraitUpsideDown:
                    fX = (screenRect.Width - size.Width) / 2;
                    fY = (screenRect.Height - size.Height) / 2 - 25;
                    break;
            }
			
            return new RectangleF (fX, fY, size.Width, size.Height);
        }                                                                                                                                    

        class DateTimeViewController : UIViewController 
        {
            readonly DateTimeElement _container;
			
            public DateTimeViewController (DateTimeElement container)
            {
                this._container = container;
            }
			
            public override void ViewWillDisappear (bool animated)
            {
                base.ViewWillDisappear (animated);
                _container.OnUserValueChanged(_container._datePicker.Date);
            }
			
            public override void DidRotate (UIInterfaceOrientation fromInterfaceOrientation)
            {
                base.DidRotate (fromInterfaceOrientation);
                _container._datePicker.Frame = PickerFrameWithSize (_container._datePicker.SizeThatFits (SizeF.Empty));
            }
			
            public bool Autorotate { get; set; }
			
            public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
            {
                return Autorotate;
            }
        }
		
        public override void Selected (DialogViewController dvc, UITableView tableView, NSIndexPath path)
        {
            var vc = new DateTimeViewController (this) {
                                                     Autorotate = dvc.Autorotate
                                                 };
            _datePicker = CreatePicker ();
            _datePicker.Frame = PickerFrameWithSize (_datePicker.SizeThatFits (SizeF.Empty));
			                            
            vc.View.BackgroundColor = UIColor.Black;
            vc.View.AddSubview (_datePicker);
            dvc.ActivateController (vc);
        }

        protected override void UpdateDetailDisplay(UITableViewCell cell)
        {
            if (cell == null)
            {
                return;
            }

            if (cell.DetailTextLabel != null)
            {
                cell.DetailTextLabel.Text = FormatDate(Value);
            }
        }
    }
}