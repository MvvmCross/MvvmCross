// MvxStandardTableViewCell.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Touch.Views
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Input;

    using Foundation;

    using MvvmCross.Binding.Bindings;

    using UIKit;

    public class MvxStandardTableViewCell
        : MvxTableViewCell
    {
        private MvxImageViewLoader _imageLoader;

        public MvxStandardTableViewCell(IntPtr handle)
            : this("TitleText" /* default binding is ToString() on the passed in item */, handle)
        {
            this.InitializeImageLoader();
        }

        public MvxStandardTableViewCell(string bindingText, IntPtr handle)
            : base(bindingText, handle)
        {
            this.InitializeImageLoader();
        }

        public MvxStandardTableViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions, IntPtr handle)
            : base(bindingDescriptions, handle)
        {
            this.InitializeImageLoader();
        }

        public MvxStandardTableViewCell(string bindingText, UITableViewCellStyle cellStyle, NSString cellIdentifier,
                                        UITableViewCellAccessory tableViewCellAccessory = UITableViewCellAccessory.None)
            : base(bindingText, cellStyle, cellIdentifier, tableViewCellAccessory)
        {
            this.InitializeImageLoader();
        }

        public MvxStandardTableViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions,
                                        UITableViewCellStyle cellStyle, NSString cellIdentifier,
                                        UITableViewCellAccessory tableViewCellAccessory = UITableViewCellAccessory.None)
            : base(bindingDescriptions, cellStyle, cellIdentifier, tableViewCellAccessory)
        {
            this.InitializeImageLoader();
        }

        private void InitializeImageLoader()
        {
            this._imageLoader = new MvxImageViewLoader(() => this.ImageView, this.SetNeedsLayout);
        }

        public MvxImageViewLoader ImageLoader => this._imageLoader;

        public string TitleText
        {
            get { return this.TextLabel.Text; }
            set { this.TextLabel.Text = value; }
        }

        public string DetailText
        {
            get { return this.DetailTextLabel.Text; }
            set { this.DetailTextLabel.Text = value; }
        }

        public string ImageUrl
        {
            get { return this._imageLoader.ImageUrl; }
            set { this._imageLoader.ImageUrl = value; }
        }

        public ICommand SelectedCommand { get; set; }

        private bool _isSelected;

        public override void SetSelected(bool selected, bool animated)
        {
            base.SetSelected(selected, animated);

            if (this._isSelected == selected)
                return;

            this._isSelected = selected;
            if (this._isSelected)
            {
                this.SelectedCommand?.Execute(null);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._imageLoader.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}