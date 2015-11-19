// DateTimeElement.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using CoreGraphics;
using CrossUI.Core;
using Foundation;
using UIKit;

namespace CrossUI.Touch.Dialog.Elements
{
    public class DateTimeElement : ValueElement<DateTime?>
    {
        private static readonly NSString Key = new NSString("DateTimeElement");

        private UIDatePicker _datePicker;

        public string DateTimeFormat { get; set; }
        public UIColor BackgroundColor { get; set; }

        public DateTimeElement()
            : this("", null)
        {
        }

        public DateTimeElement(string caption, DateTime? date)
            : base(caption, date)
        {
            if (date.HasValue && date.Value.Kind != DateTimeKind.Utc)
                DialogTrace.WriteLine("Warning - it's safest to use Utc time with DateTimeElement");

            BackgroundColor = (UIDevice.CurrentDevice.CheckSystemVersion(7, 0)) ? UIColor.White : UIColor.Black;  
            DateTimeFormat = "G";
        }

        protected override UITableViewCell GetCellImpl(UITableView tv)
        {
            var cell = tv.DequeueReusableCell(Key) ?? new UITableViewCell(UITableViewCellStyle.Value1, Key)
            {
                Accessory = UITableViewCellAccessory.DisclosureIndicator
            };

            UpdateDetailDisplay(cell);
            return cell;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                if (_datePicker != null)
                {
                    _datePicker.Dispose();
                    _datePicker = null;
                }
            }
        }

        public virtual string FormatDate(DateTime dt)
        {
            // note that we removed ToLocalTime() and NSDateFormatter here
            // - use DotNet formatting instead
            // - for formats, see http://msdn.microsoft.com/en-us/library/zdtaw1bw.aspx
            return dt.ToString(DateTimeFormat);
        }

        public virtual UIDatePicker CreatePicker()
        {
            var picker = new UIDatePicker(CGRect.Empty)
                {
                    //ensure picker will stay centered, regardless current screen orientation
                    AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin | UIViewAutoresizing.FlexibleTopMargin | UIViewAutoresizing.FlexibleBottomMargin,
                    Mode = UIDatePickerMode.DateAndTime,
                };
            return picker;
        }

        private class DateTimeViewController : UIViewController
        {
            private readonly DateTimeElement _container;

            public DateTimeViewController(DateTimeElement container)
            {
                this._container = container;
            }

            public override void ViewWillDisappear(bool animated)
            {
                base.ViewWillDisappear(animated);
                _container.OnDateTimeFromPicker(_container._datePicker.Date);
            }

            public bool Autorotate { get; set; }

#warning Need to update autorotation code after ios6 changes
            [Obsolete]
            public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
            {
                return Autorotate;
            }
        }

        protected virtual void OnDateTimeFromPicker(NSDate simpleDate)
        {
            var utcDateTime = DateTimeFromPickerDateTime(simpleDate);
            OnUserValueChanged(utcDateTime);
        }

        protected virtual DateTime DateTimeFromPickerDateTime(NSDate simpleDate)
        {
            var components = NSCalendar.CurrentCalendar.Components(
                NSCalendarUnit.Year | NSCalendarUnit.Month | NSCalendarUnit.Day | NSCalendarUnit.Hour |
                NSCalendarUnit.Minute | NSCalendarUnit.Second, simpleDate);
            return new DateTime((int)components.Year, (int)components.Month, (int)components.Day, (int)components.Hour, (int)components.Minute, (int)components.Second, DateTimeKind.Utc);
        }

        protected virtual DateTime DateTimeToPickerDateTime(DateTime simpleDate)
        {
            return new DateTime(simpleDate.Year, simpleDate.Month, simpleDate.Day, simpleDate.Hour, simpleDate.Minute, simpleDate.Second, DateTimeKind.Local);
        }

        public override void Selected(DialogViewController dvc, UITableView tableView, NSIndexPath path)
        {
            var vc = new DateTimeViewController(this)
                {
                    Autorotate = dvc.Autorotate
                };
            if (_datePicker == null)
                _datePicker = CreatePicker();
            _datePicker.Date = (NSDate)DateTimeToPickerDateTime(Value ?? DateTime.UtcNow);

            vc.View.BackgroundColor = BackgroundColor;
            vc.View.AddSubview(_datePicker);
            dvc.ActivateController(vc);

            //ensure picker will stay centered, regardless current screen orientation
            _datePicker.Center = vc.View.Center;
        }

        protected override void UpdateDetailDisplay(UITableViewCell cell)
        {
            if (cell?.DetailTextLabel != null)
            {
                cell.DetailTextLabel.Text = Value.HasValue ? FormatDate(Value.Value) : string.Empty;
            }
        }
    }
}