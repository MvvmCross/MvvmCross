// SimplePickerElement.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Dialog.Touch.Elements
{
    using System;
    using System.Collections;
    using System.Globalization;

    using CoreGraphics;

    using CrossUI.Touch.Dialog;
    using CrossUI.Touch.Dialog.Elements;

    using Foundation;

    using MvvmCross.Platform.Converters;

    using UIKit;

    public class SimplePickerElement : ValueElement<object>
    {
        private static readonly NSString Key = new NSString("SimplePickerElement");

        private UIPickerView _picker;

        public IList Entries { get; set; }

        public IMvxValueConverter DisplayValueConverter { get; set; }

        public UIColor BackgroundColor { get; set; }

        private class ToStringDisplayValueConverter : MvxValueConverter
        {
            public override object Convert(object value, System.Type targetType, object parameter,
                                           System.Globalization.CultureInfo culture)
            {
                if (value == null)
                    return string.Empty;

                return value.ToString();
            }
        }

        public SimplePickerElement(string caption, object value, IMvxValueConverter displayValueConverter = null)
            : base(caption, value)
        {
            this.DisplayValueConverter = displayValueConverter ?? new ToStringDisplayValueConverter();
            this.BackgroundColor = (UIDevice.CurrentDevice.CheckSystemVersion(7, 0)) ? UIColor.White : UIColor.Black;
        }

        protected override UITableViewCell GetCellImpl(UITableView tv)
        {
            var cell = tv.DequeueReusableCell(Key) ??
                       new UITableViewCell(UITableViewCellStyle.Value1, Key)
                       {
                           Accessory = UITableViewCellAccessory.DisclosureIndicator
                       };

            this.UpdateDetailDisplay(cell);
            return cell;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                if (this._picker != null)
                {
                    this._picker.Model.Dispose();
                    this._picker.Model = null;
                    this._picker.Dispose();
                    this._picker = null;
                }
            }
        }

        public virtual UIPickerView CreatePicker()
        {
            var picker = new UIPickerView(CGRect.Empty)
            {
                AutoresizingMask = UIViewAutoresizing.FlexibleWidth,
                Model = new SimplePickerViewModel(this),
                ShowSelectionIndicator = true,
            };
            return picker;
        }

        private static CGRect PickerFrameWithSize(CGSize size)
        {
            var screenRect = UIScreen.MainScreen.ApplicationFrame;
            nfloat fY = 0, fX = 0;

            switch (UIApplication.SharedApplication.StatusBarOrientation)
            {
                case UIInterfaceOrientation.LandscapeLeft:
                case UIInterfaceOrientation.LandscapeRight:
                    fX = (screenRect.Height - size.Width) / 2;
                    fY = (screenRect.Width - size.Height) / 2 - 17;
                    break;

                case UIInterfaceOrientation.Portrait:
                case UIInterfaceOrientation.PortraitUpsideDown:
                    fX = (screenRect.Width - size.Width) / 2;
                    fY = (screenRect.Height - size.Height) / 2 - 25;
                    break;
            }

            return new CGRect(fX, fY, size.Width, size.Height);
        }

        private string GetString(nint row)
        {
            if (this.Entries == null)
                return string.Empty;

            if (row >= this.Entries.Count)
                return string.Empty;

            var whichObject = this.Entries[(int)row];
            return this.ConvertToString(whichObject);
        }

        private string ConvertToString(object whichObject)
        {
            return
                this.DisplayValueConverter.Convert(whichObject, typeof(string), null, CultureInfo.CurrentUICulture)
                                     .ToString();
        }

        private class SimplePickerViewModel : UIPickerViewModel
        {
            private readonly SimplePickerElement _owner;

            public SimplePickerViewModel(SimplePickerElement owner)
            {
                this._owner = owner;
            }

            public override nint GetComponentCount(UIPickerView picker)
            {
                return 1;
            }

            public override nint GetRowsInComponent(UIPickerView picker, nint component)
            {
                if (this._owner.Entries == null)
                    return 0;

                return this._owner.Entries.Count;
            }

            public override string GetTitle(UIPickerView picker, nint row, nint component)
            {
                return this._owner.GetString(row) ?? string.Empty;
            }

            public override nfloat GetComponentWidth(UIPickerView picker, nint component)
            {
                // TODO - need to get this better (currently just using a fixed value like in http://weblogs.asp.net/wallym/archive/2010/01/07/uipicker-in-the-iphone-with-monotouch.aspx)
                return 300.0f;
            }

            public override nfloat GetRowHeight(UIPickerView picker, nint component)
            {
                // TODO - need to get this better (currently just using a fixed value like in http://weblogs.asp.net/wallym/archive/2010/01/07/uipicker-in-the-iphone-with-monotouch.aspx)
                return 40.0f;
            }

            public override void Selected(UIPickerView picker, nint row, nint component)
            {
                // TODO - update the value here...
                this._owner.OnUserValueChanged(this._owner.Entries[(int)row]);
            }
        }

        private class SimplePickerViewController : UIViewController
        {
            private readonly SimplePickerElement _container;

            public SimplePickerViewController(SimplePickerElement container)
            {
                this._container = container;
            }

            public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
            {
                base.DidRotate(fromInterfaceOrientation);
                this._container._picker.Frame = PickerFrameWithSize(this._container._picker.SizeThatFits(CGSize.Empty));
            }

            public bool Autorotate { get; set; }

#warning Need to update autorotation code after ios6 changes

            [Obsolete]
            public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
            {
                return this.Autorotate;
            }
        }

        public override void Selected(DialogViewController dvc, UITableView tableView, NSIndexPath path)
        {
            var vc = new SimplePickerViewController(this)
            {
                Autorotate = dvc.Autorotate
            };
            this._picker = this.CreatePicker();
            this._picker.Frame = PickerFrameWithSize(this._picker.SizeThatFits(CGSize.Empty));

            if (this.Entries != null)
            {
                var index = this.Entries.IndexOf(this.Value);
                if (index >= 0)
                {
                    this._picker.Select(index, 0, true);
                }
            }

            vc.View.BackgroundColor = this.BackgroundColor;
            vc.View.AddSubview(this._picker);
            dvc.ActivateController(vc);
        }

        protected override void UpdateDetailDisplay(UITableViewCell cell)
        {
            if (cell?.DetailTextLabel != null)
            {
                cell.DetailTextLabel.Text = this.ConvertToString(this.Value);
            }
        }
    }
}