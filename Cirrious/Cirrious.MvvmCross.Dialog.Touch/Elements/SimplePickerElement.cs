// SimplePickerElement.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections;
using System.Drawing;
using System.Globalization;
using Cirrious.CrossCore.Converters;
using Cirrious.CrossCore.Interfaces.Converters;
using CrossUI.Touch.Dialog;
using CrossUI.Touch.Dialog.Elements;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Dialog.Touch.Elements
{
    public class SimplePickerElement : ValueElement<object>
    {
        private static readonly NSString Key = new NSString("SimplePickerElement");

        private UIPickerView _picker;

        public IList Entries { get; set; }

        public IMvxValueConverter DisplayValueConverter { get; set; }

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
            DisplayValueConverter = displayValueConverter ?? new ToStringDisplayValueConverter();
        }

        protected override UITableViewCell GetCellImpl(UITableView tv)
        {
            var cell = tv.DequeueReusableCell(Key) ??
                       new UITableViewCell(UITableViewCellStyle.Value1, Key)
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
                if (_picker != null)
                {
                    _picker.Model.Dispose();
                    _picker.Model = null;
                    _picker.Dispose();
                    _picker = null;
                }
            }
        }

        public virtual UIPickerView CreatePicker()
        {
            var picker = new UIPickerView(RectangleF.Empty)
                {
                    AutoresizingMask = UIViewAutoresizing.FlexibleWidth,
                    Model = new SimplePickerViewModel(this),
                    ShowSelectionIndicator = true,
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

        private string GetString(int row)
        {
            if (Entries == null)
                return string.Empty;

            if (row >= Entries.Count)
                return string.Empty;

            var whichObject = Entries[row];
            return ConvertToString(whichObject);
        }

        private string ConvertToString(object whichObject)
        {
            return
                DisplayValueConverter.Convert(whichObject, typeof (string), null, CultureInfo.CurrentUICulture)
                                     .ToString();
        }

        private class SimplePickerViewModel : UIPickerViewModel
        {
            private readonly SimplePickerElement _owner;

            public SimplePickerViewModel(SimplePickerElement owner)
            {
                _owner = owner;
            }

            public override int GetComponentCount(UIPickerView picker)
            {
                return 1;
            }

            public override int GetRowsInComponent(UIPickerView picker, int component)
            {
                if (_owner.Entries == null)
                    return 0;

                return _owner.Entries.Count;
            }

            public override string GetTitle(UIPickerView picker, int row, int component)
            {
                return _owner.GetString(row) ?? string.Empty;
            }

            public override float GetComponentWidth(UIPickerView picker, int component)
            {
                // TODO - need to get this better (currently just using a fixed value like in http://weblogs.asp.net/wallym/archive/2010/01/07/uipicker-in-the-iphone-with-monotouch.aspx)
                return 300.0f;
            }

            public override float GetRowHeight(UIPickerView picker, int component)
            {
                // TODO - need to get this better (currently just using a fixed value like in http://weblogs.asp.net/wallym/archive/2010/01/07/uipicker-in-the-iphone-with-monotouch.aspx)
                return 40.0f;
            }

            public override void Selected(UIPickerView picker, int row, int component)
            {
                // TODO - update the value here...
                _owner.OnUserValueChanged(_owner.Entries[row]);
            }
        }

        private class SimplePickerViewController : UIViewController
        {
            private readonly SimplePickerElement _container;

            public SimplePickerViewController(SimplePickerElement container)
            {
                _container = container;
            }

            public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
            {
                base.DidRotate(fromInterfaceOrientation);
                _container._picker.Frame = PickerFrameWithSize(_container._picker.SizeThatFits(SizeF.Empty));
            }

            public bool Autorotate { get; set; }

#warning Need to update autorotation code after ios6 changes
            [Obsolete]
            public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
            {
                return Autorotate;
            }
        }

        public override void Selected(DialogViewController dvc, UITableView tableView, NSIndexPath path)
        {
            var vc = new SimplePickerViewController(this)
                {
                    Autorotate = dvc.Autorotate
                };
            _picker = CreatePicker();
            _picker.Frame = PickerFrameWithSize(_picker.SizeThatFits(SizeF.Empty));

            if (Entries != null)
            {
                var index = Entries.IndexOf(Value);
                if (index >= 0)
                {
                    _picker.Select(index, 0, true);
                }
            }

            vc.View.BackgroundColor = UIColor.Black;
            vc.View.AddSubview(_picker);
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
                cell.DetailTextLabel.Text = ConvertToString(Value);
            }
        }
    }
}