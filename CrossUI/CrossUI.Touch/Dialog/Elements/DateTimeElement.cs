// DateTimeElement.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Drawing;
using CrossUI.Core;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace CrossUI.Touch.Dialog.Elements
{
    public class DateTimeElement : ValueElement<DateTime?>
    {
        private static readonly NSString Key = new NSString("DateTimeElement");

        private UIDatePicker _datePicker;

        public string DateTimeFormat { get; set; }

        public DateTimeElement()
            : this("", null)
        {
        }

        public DateTimeElement(string caption, DateTime? date)
            : base(caption, date)
        {
            if (date.HasValue && date.Value.Kind != DateTimeKind.Utc)
                DialogTrace.WriteLine("Warning - it's safest to use Utc time with DateTimeElement");

            DateTimeFormat = "G";
        }

        protected override UITableViewCell GetCellImpl(UITableView tv)
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
            var picker = new UIDatePicker(RectangleF.Empty)
                {
                    AutoresizingMask = UIViewAutoresizing.FlexibleWidth,
                    Mode = UIDatePickerMode.DateAndTime,
                };
            return picker;
        }

        private static RectangleF PickerFrameWithSize(SizeF size)
        {
            var screenRect = UIScreen.MainScreen.ApplicationFrame;
            float fY = 0, fX = 0;

            switch (UIApplication.SharedApplication.StatusBarOrientation)
            {
                case UIInterfaceOrientation.LandscapeLeft:
                case UIInterfaceOrientation.LandscapeRight:
                    fX = (screenRect.Height - size.Width)/2;
                    fY = (screenRect.Width - size.Height)/2 - 17;
                    break;

                case UIInterfaceOrientation.Portrait:
                case UIInterfaceOrientation.PortraitUpsideDown:
                    fX = (screenRect.Width - size.Width)/2;
                    fY = (screenRect.Height - size.Height)/2 - 25;
                    break;
            }

            return new RectangleF(fX, fY, size.Width, size.Height);
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

            public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
            {
                base.DidRotate(fromInterfaceOrientation);
                _container._datePicker.Frame = PickerFrameWithSize(_container._datePicker.SizeThatFits(SizeF.Empty));
            }

            public bool Autorotate { get; set; }

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
            return new DateTime(components.Year, components.Month, components.Day, components.Hour, components.Minute, components.Second, DateTimeKind.Utc);
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
            _datePicker.Date = DateTimeToPickerDateTime(Value.HasValue ? Value.Value : DateTime.UtcNow);
            _datePicker.Frame = PickerFrameWithSize(_datePicker.SizeThatFits(SizeF.Empty));

            vc.View.BackgroundColor = UIColor.Black;
            vc.View.AddSubview(_datePicker);
            dvc.ActivateController(vc);
        }

        protected override void UpdateDetailDisplay(UITableViewCell cell)
        {
            if (cell == null)
            {
                return;
            }

            if (cell.DetailTextLabel != null)
            {
                cell.DetailTextLabel.Text = Value.HasValue ? FormatDate(Value.Value) : string.Empty;
            }
        }
    }
}