// GeneralTableViewCell.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.Bindings;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Foundation;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using UIKit;

namespace Cirrious.MvvmCross.AutoView.Touch.Views.Lists
{
    public class GeneralTableViewCell
        : MvxTableViewCell
    {
        private IMvxImageHelper<UIImage> _imageHelper;

        public GeneralTableViewCell(string bindingText, IntPtr handle)
            : base(bindingText, handle)
        {
            InitializeImageHelper();
        }

        public GeneralTableViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions, IntPtr handle)
            : base(bindingDescriptions, handle)
        {
            InitializeImageHelper();
        }

        public GeneralTableViewCell(string bindingText, UITableViewCellStyle cellStyle, NSString cellIdentifier,
                                    UITableViewCellAccessory tableViewCellAccessory = UITableViewCellAccessory.None)
            : base(bindingText, cellStyle, cellIdentifier, tableViewCellAccessory)
        {
            InitializeImageHelper();
        }

        public GeneralTableViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions,
                                    UITableViewCellStyle cellStyle, NSString cellIdentifier,
                                    UITableViewCellAccessory tableViewCellAccessory = UITableViewCellAccessory.None)
            : base(bindingDescriptions, cellStyle, cellIdentifier, tableViewCellAccessory)
        {
            InitializeImageHelper();
        }

        public GeneralTableViewCell(IntPtr handle)
            : base(handle)
        {
            Mvx.Warning("GeneralTableViewCell IntPtr constructor used - we expect this only to be called during memory leak debugging - see https://github.com/MvvmCross/MvvmCross/pull/467");
            InitializeImageHelper();
        }

        private void InitializeImageHelper()
        {
            _imageHelper = Mvx.Resolve<IMvxImageHelper<UIImage>>();
            _imageHelper.ImageChanged += ImageHelperOnImageChanged;
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
            get { return _imageHelper.ImageUrl; }
            set { _imageHelper.ImageUrl = value; }
        }

        public IMvxImageHelper<UIImage> Image => _imageHelper;

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
                SelectedCommand?.Execute(null);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _imageHelper.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}