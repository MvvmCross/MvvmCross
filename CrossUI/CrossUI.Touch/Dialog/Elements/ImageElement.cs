// ImageElement.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using CoreGraphics;
using Foundation;
using System;
using UIKit;

namespace CrossUI.Touch.Dialog.Elements
{
    public class ImageElement : ValueElement<UIImage>
    {
        private static CGRect _rect = new CGRect(0, 0, Dimx, Dimy);
        private static readonly NSString Ikey = new NSString("ImageElement");
        private UIImage _scaled;
        private UIPopoverController _popover;

        // Apple leaks this one, so share across all.
        private static UIImagePickerController _picker;

        // Height for rows
        private const int Dimx = 48;

        private const int Dimy = 43;

        // radius for rounding
        private const int Rad = 10;

        private static UIImage MakeEmpty()
        {
            using (var cs = CGColorSpace.CreateDeviceRGB())
            {
                using (
                    var bit = new CGBitmapContext(IntPtr.Zero, Dimx, Dimy, 8, 0, cs, CGImageAlphaInfo.PremultipliedFirst)
                    )
                {
                    bit.SetStrokeColor(1, 0, 0, 0.5f);
                    bit.FillRect(new CGRect(0, 0, Dimx, Dimy));

                    return UIImage.FromImage(bit.ToImage());
                }
            }
        }

        private UIImage Scale(UIImage source)
        {
            UIGraphics.BeginImageContext(new CGSize(Dimx, Dimy));
            var ctx = UIGraphics.GetCurrentContext();

            var img = source.CGImage;
            ctx.TranslateCTM(0, Dimy);
            if (img.Width > img.Height)
                ctx.ScaleCTM(1, -img.Width / Dimy);
            else
                ctx.ScaleCTM(img.Height / Dimx, -1);

            ctx.DrawImage(_rect, source.CGImage);

            var ret = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return ret;
        }

        public ImageElement() : this(null)
        {
        }

        public ImageElement(UIImage image) : base("")
        {
            if (image == null)
            {
                Value = MakeEmpty();
                _scaled = Value;
            }
            else
            {
                Value = image;
                _scaled = Scale(Value);
            }
        }

        protected override NSString CellKey => Ikey;

        protected override UITableViewCell GetCellImpl(UITableView tv)
        {
            var cell = tv.DequeueReusableCell(CellKey) ?? new UITableViewCell(UITableViewCellStyle.Default, CellKey);

            if (_scaled == null)
                return cell;

            UpdateDetailDisplay(cell);
            return cell;
        }

        private void DrawClippedBitmap(bool roundTop, CGBitmapContext bit, bool roundBottom)
        {
            // Clipping path for the image, different on top, middle and bottom.
            if (roundBottom)
            {
                bit.AddArc(Rad, Rad, Rad, (float)Math.PI, (float)(3 * Math.PI / 2), false);
            }
            else
            {
                bit.MoveTo(0, Rad);
                bit.AddLineToPoint(0, 0);
            }
            bit.AddLineToPoint(Dimx, 0);
            bit.AddLineToPoint(Dimx, Dimy);

            if (roundTop)
            {
                bit.AddArc(Rad, Dimy - Rad, Rad, (float)(Math.PI / 2), (float)Math.PI, false);
                bit.AddLineToPoint(0, Rad);
            }
            else
            {
                bit.AddLineToPoint(0, Dimy);
            }
            bit.Clip();
            bit.DrawImage(_rect, _scaled.CGImage);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_scaled != null)
                {
                    _scaled.Dispose();
                    _scaled = null;
                }
                if (Value != null)
                {
                    Value.Dispose();
                    Value = null;
                }
            }
            base.Dispose(disposing);
        }

        private class MyUIImagePickerDelegate : UIImagePickerControllerDelegate
        {
            private readonly ImageElement _container;
            private readonly UITableView _table;
            private readonly NSIndexPath _path;

            public MyUIImagePickerDelegate(ImageElement container, UITableView table, NSIndexPath path)
            {
                this._container = container;
                this._table = table;
                this._path = path;
            }

            public override void FinishedPickingImage(UIImagePickerController picker, UIImage image,
                                                      NSDictionary editingInfo)
            {
                _container.Picked(image);
                _table.ReloadRows(new[] { _path }, UITableViewRowAnimation.None);
            }
        }

        protected virtual void Picked(UIImage image)
        {
            _scaled = Scale(image);
            OnUserValueChanged(image);
            _currentController.DismissViewController(true, () => { });
        }

        private UIViewController _currentController;

        public override void Selected(DialogViewController dvc, UITableView tableView, NSIndexPath path)
        {
            if (_picker == null)
                _picker = new UIImagePickerController();
            _picker.Delegate = new MyUIImagePickerDelegate(this, tableView, path);

            switch (UIDevice.CurrentDevice.UserInterfaceIdiom)
            {
                case UIUserInterfaceIdiom.Pad:
                    _popover = new UIPopoverController(_picker);
                    var cell = tableView.CellAt(path);
                    if (cell != null)
                        _rect = cell.Frame;
                    _popover.PresentFromRect(_rect, dvc.View, UIPopoverArrowDirection.Any, true);
                    break;

                default:
                case UIUserInterfaceIdiom.Phone:
                    dvc.ActivateController(_picker);
                    break;
            }
            _currentController = dvc;
        }

        protected override void UpdateDetailDisplay(UITableViewCell cell)
        {
            if (cell == null || Parent == null)
                return;

            var psection = Parent as Section;
            bool roundTop = psection.Elements[0] == this;
            bool roundBottom = psection.Elements[psection.Elements.Count - 1] == this;

            using (var cs = CGColorSpace.CreateDeviceRGB())
            {
                using (
                    var bit = new CGBitmapContext(IntPtr.Zero, Dimx, Dimy, 8, 0, cs, CGImageAlphaInfo.PremultipliedFirst)
                    )
                {
                    DrawClippedBitmap(roundTop, bit, roundBottom);
                    cell.ImageView.Image = UIImage.FromImage(bit.ToImage());
                }
            }
        }
    }
}