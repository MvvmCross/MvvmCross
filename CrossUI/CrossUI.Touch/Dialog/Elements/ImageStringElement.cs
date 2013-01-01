// ImageStringElement.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace CrossUI.Touch.Dialog.Elements
{
    public class ImageStringElement : StringElement
    {
        private static readonly NSString Skey = new NSString("ImageStringElement");
        private readonly UIImage _image;

#warning Should really try to update the live cell when this changes
        public UITableViewCellAccessory Accessory { get; set; }

        public ImageStringElement() : this("", null)
        {
        }

        public ImageStringElement(string caption, UIImage image) : base(caption)
        {
            this._image = image;
            this.Accessory = UITableViewCellAccessory.None;
        }

        public ImageStringElement(string caption, string value, UIImage image) : base(caption, value)
        {
            this._image = image;
            this.Accessory = UITableViewCellAccessory.None;
        }

        public ImageStringElement(string caption, NSAction tapped, UIImage image) : base(caption, tapped)
        {
            this._image = image;
            this.Accessory = UITableViewCellAccessory.None;
        }

        protected override NSString CellKey
        {
            get { return Skey; }
        }

        protected override UITableViewCell GetCellImpl(UITableView tv)
        {
            var cell = tv.DequeueReusableCell(CellKey);
            if (cell == null)
            {
                cell = new UITableViewCell(
                    Value == null ? UITableViewCellStyle.Default : UITableViewCellStyle.Subtitle, CellKey);
                cell.SelectionStyle = UITableViewCellSelectionStyle.Blue;
            }

            cell.Accessory = Accessory;
            cell.TextLabel.Text = Caption;
            cell.TextLabel.TextAlignment = Alignment;

            cell.ImageView.Image = _image;

            // The check is needed because the cell might have been recycled.
            if (cell.DetailTextLabel != null)
                cell.DetailTextLabel.Text = Value ?? "";

            return cell;
        }
    }
}