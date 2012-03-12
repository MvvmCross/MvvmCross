#region Copyright
// <copyright file="MvxBindableTableViewCell.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Touch.Interfaces.Views;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.Commands;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Platform.Images;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Views
{
    public class MvxBindableTableViewCell
        : UITableViewCell
          , IMvxBindableView
          , IMvxServiceConsumer<IMvxBinder>
    {
        private readonly IList<IMvxUpdateableBinding> _bindings;
        private MvxDynamicImageHelper<UIImage> _imageHelper;
		 
        public MvxBindableTableViewCell(string bindingText, IntPtr handle)
            : base(handle)
        {
            InitialiseImageHelper();
            _bindings = Binder.Bind(null, this, bindingText).ToList();
        }		

        public MvxBindableTableViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions, IntPtr handle)
            : base(handle)
        {
            InitialiseImageHelper();
            _bindings = Binder.Bind(null, this, bindingDescriptions).ToList();
        }

        public MvxBindableTableViewCell(string bindingText, UITableViewCellStyle cellStyle, NSString cellIdentifier, UITableViewCellAccessory tableViewCellAccessory = UITableViewCellAccessory.None)
            : base(cellStyle, cellIdentifier)
        {
            Accessory = tableViewCellAccessory;
            InitialiseImageHelper();
            _bindings = Binder.Bind(null, this, bindingText).ToList();
        }

        public MvxBindableTableViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions, UITableViewCellStyle cellStyle, NSString cellIdentifier, UITableViewCellAccessory tableViewCellAccessory = UITableViewCellAccessory.None)
            : base(cellStyle, cellIdentifier)
        {
            Accessory = tableViewCellAccessory;
            InitialiseImageHelper();
            _bindings = Binder.Bind(null, this, bindingDescriptions).ToList();
        }
		
		private void InitialiseImageHelper()
		{
            _imageHelper = new MvxDynamicImageHelper<UIImage>();
            _imageHelper.ImageChanged += ImageHelperOnImageChanged;
		}

        // we seal Accessory here so that we can use it in the constructor - otherwise virtual issues.
        public sealed override UITableViewCellAccessory Accessory
        {
            get
            {
                return base.Accessory;
            }
            set
            {
                base.Accessory = value;
            }
        }

        private IMvxBinder Binder
        {
            get { return this.GetService<IMvxBinder>(); }
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
		
		public string HttpImageUrl
		{
			get { return _imageHelper.HttpImageUrl; }
			set { _imageHelper.HttpImageUrl = value; }
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

        public IMvxCommand SelectedCommand { get; set; }

        public override void SetSelected(bool selected, bool animated)
        {
            base.SetSelected(selected, animated);
            
            if (selected)
                if (SelectedCommand != null)
                    SelectedCommand.Execute();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _imageHelper.Dispose();

                foreach (var binding in _bindings)
                {
                    binding.Dispose();
                }
                _bindings.Clear();
            }
            base.Dispose(disposing);
        }

        #region IMvxBindableView Members

        public void BindTo(object source)
        {
            foreach (var binding in _bindings)
            {
                binding.DataContext = source;
            }
        }

        #endregion
    }
}