// MvxStandardTableViewCell.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Windows.Input;
using Cirrious.MvvmCross.Binding.Bindings;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Views
{
    public class MvxStandardTableViewCell
        : MvxTableViewCell
    {
        private MvxImageViewLoader _imageLoader;

		public MvxStandardTableViewCell(IntPtr handle)
			: this("TitleText" /* default binding is ToString() on the passed in item */, handle)
		{
			InitialiseImageLoader();
		}

		public MvxStandardTableViewCell(string bindingText, IntPtr handle)
			: base(bindingText, handle)
		{
			InitialiseImageLoader();
		}

        public MvxStandardTableViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions, IntPtr handle)
            : base(bindingDescriptions, handle)
        {
            InitialiseImageLoader();
        }

        public MvxStandardTableViewCell(string bindingText, UITableViewCellStyle cellStyle, NSString cellIdentifier,
                                        UITableViewCellAccessory tableViewCellAccessory = UITableViewCellAccessory.None)
            : base(bindingText, cellStyle, cellIdentifier, tableViewCellAccessory)
        {
            InitialiseImageLoader();
        }

        public MvxStandardTableViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions,
                                        UITableViewCellStyle cellStyle, NSString cellIdentifier,
                                        UITableViewCellAccessory tableViewCellAccessory = UITableViewCellAccessory.None)
            : base(bindingDescriptions, cellStyle, cellIdentifier, tableViewCellAccessory)
        {
            InitialiseImageLoader();
        }

        private void InitialiseImageLoader()
        {
            _imageLoader = new MvxImageViewLoader(() => ImageView, SetNeedsLayout);
        }

        public MvxImageViewLoader ImageLoader
        {
            get { return _imageLoader; }
        }

        public string TitleText
        {
            get { return TextLabel.Text; }
            set { TextLabel.Text = value; }
        }

        public string DetailText
        {
            get { return DetailTextLabel.Text; }
            set { DetailTextLabel.Text = value; }
        }

        public string ImageUrl
        {
            get { return _imageLoader.ImageUrl; }
            set { _imageLoader.ImageUrl = value; }
        }

        public ICommand SelectedCommand { get; set; }

        private bool _isSelected;

        public override void SetSelected(bool selected, bool animated)
        {
            base.SetSelected(selected, animated);

            if (_isSelected == selected)
                return;

            _isSelected = selected;
            if (_isSelected)
                if (SelectedCommand != null)
                    SelectedCommand.Execute(null);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _imageLoader.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}