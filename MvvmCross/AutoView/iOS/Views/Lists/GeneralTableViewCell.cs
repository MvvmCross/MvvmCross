// GeneralTableViewCell.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.AutoView.iOS.Views.Lists
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Input;

    using Foundation;

    using MvvmCross.Binding.Bindings;
    using MvvmCross.Binding.Touch.Views;
    using MvvmCross.Platform;
    using MvvmCross.Platform.Core;
    using MvvmCross.Platform.Platform;

    using UIKit;

    public class GeneralTableViewCell
        : MvxTableViewCell
    {
        private IMvxImageHelper<UIImage> _imageHelper;

        public GeneralTableViewCell(string bindingText, IntPtr handle)
            : base(bindingText, handle)
        {
            this.InitializeImageHelper();
        }

        public GeneralTableViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions, IntPtr handle)
            : base(bindingDescriptions, handle)
        {
            this.InitializeImageHelper();
        }

        public GeneralTableViewCell(string bindingText, UITableViewCellStyle cellStyle, NSString cellIdentifier,
                                    UITableViewCellAccessory tableViewCellAccessory = UITableViewCellAccessory.None)
            : base(bindingText, cellStyle, cellIdentifier, tableViewCellAccessory)
        {
            this.InitializeImageHelper();
        }

        public GeneralTableViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions,
                                    UITableViewCellStyle cellStyle, NSString cellIdentifier,
                                    UITableViewCellAccessory tableViewCellAccessory = UITableViewCellAccessory.None)
            : base(bindingDescriptions, cellStyle, cellIdentifier, tableViewCellAccessory)
        {
            this.InitializeImageHelper();
        }

        public GeneralTableViewCell(IntPtr handle)
            : base(handle)
        {
            Mvx.Warning("GeneralTableViewCell IntPtr constructor used - we expect this only to be called during memory leak debugging - see https://github.com/MvvmCross/MvvmCross/pull/467");
            this.InitializeImageHelper();
        }

        private void InitializeImageHelper()
        {
            this._imageHelper = Mvx.Resolve<IMvxImageHelper<UIImage>>();
            this._imageHelper.ImageChanged += this.ImageHelperOnImageChanged;
        }

        public string Title
        {
            get { return TextLabel.Text; }
            set { TextLabel.Text = value; }
        }

        public string SubTitle
        {
            get { return DetailTextLabel.Text; }
            set { DetailTextLabel.Text = value; }
        }

        public string ImageUrl
        {
            get { return this._imageHelper.ImageUrl; }
            set { this._imageHelper.ImageUrl = value; }
        }

        public IMvxImageHelper<UIImage> Image => this._imageHelper;

        private void ImageHelperOnImageChanged(object sender, MvxValueEventArgs<UIImage> mvxValueEventArgs)
        {
            InvokeOnMainThread(() =>
            {
                if (ImageView != null)
                    ImageView.Image = mvxValueEventArgs.Value;
            });
        }

        public ICommand SelectedCommand { get; set; }

        public override void SetSelected(bool selected, bool animated)
        {
            base.SetSelected(selected, animated);

            if (selected)
            {
                this.SelectedCommand?.Execute(null);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._imageHelper.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}