// MvxBindableTableViewCell.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Windows.Input;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Plugins.DownloadCache;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Views
{
    public class MvxBindableTableViewCell
        : MvxBaseBindableTableViewCell
    {
        private MvxDynamicImageHelper<UIImage> _imageHelper;

        public MvxBindableTableViewCell(string bindingText, IntPtr handle)
            : base(bindingText, handle)
        {
            InitialiseImageHelper();
        }

        public MvxBindableTableViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions, IntPtr handle)
            : base(bindingDescriptions, handle)
        {
            InitialiseImageHelper();
        }

        public MvxBindableTableViewCell(string bindingText, UITableViewCellStyle cellStyle, NSString cellIdentifier,
                                        UITableViewCellAccessory tableViewCellAccessory = UITableViewCellAccessory.None)
            : base(bindingText, cellStyle, cellIdentifier, tableViewCellAccessory)
        {
            InitialiseImageHelper();
        }

        public MvxBindableTableViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions,
                                        UITableViewCellStyle cellStyle, NSString cellIdentifier,
                                        UITableViewCellAccessory tableViewCellAccessory = UITableViewCellAccessory.None)
            : base(bindingDescriptions, cellStyle, cellIdentifier, tableViewCellAccessory)
        {
            InitialiseImageHelper();
        }

        private void InitialiseImageHelper()
        {
            _imageHelper = new MvxDynamicImageHelper<UIImage>();
            _imageHelper.ImageChanged += ImageHelperOnImageChanged;
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
			get { return _imageHelper.ImageUrl; }
			set { _imageHelper.ImageUrl = value; }
		}

        [Obsolete]
        public string HttpImageUrl
        {
            get { return _imageHelper.ImageUrl; }
            set { _imageHelper.ImageUrl = value; }
        }

        public MvxDynamicImageHelper<UIImage> Image
        {
            get { return _imageHelper; }
        }

        private void ImageHelperOnImageChanged(object sender, MvxValueEventArgs<UIImage> mvxValueEventArgs)
        {
            if (ImageView != null)
                ImageView.Image = mvxValueEventArgs.Value;
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
                _imageHelper.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}