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
using System.Windows.Input;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Touch.Interfaces.Views;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Plugins.DownloadCache;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Views
{
	public class GeneralTableViewCell
		: MvxBaseBindableTableViewCell
	{
		private MvxDynamicImageHelper<UIImage> _imageHelper;
		
		public GeneralTableViewCell(string bindingText, IntPtr handle)
			: base(bindingText, handle)
		{
			InitialiseImageHelper();
		}		
		
		public GeneralTableViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions, IntPtr handle)
			: base(bindingDescriptions, handle)
		{
			InitialiseImageHelper();
		}
		
		public GeneralTableViewCell(string bindingText, UITableViewCellStyle cellStyle, NSString cellIdentifier, UITableViewCellAccessory tableViewCellAccessory = UITableViewCellAccessory.None)
			: base(bindingText, cellStyle, cellIdentifier, tableViewCellAccessory)
		{
			InitialiseImageHelper();
		}
		
		public GeneralTableViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions, UITableViewCellStyle cellStyle, NSString cellIdentifier, UITableViewCellAccessory tableViewCellAccessory = UITableViewCellAccessory.None)
			: base(bindingDescriptions, cellStyle, cellIdentifier, tableViewCellAccessory)
		{
			InitialiseImageHelper();
		}
		
		private void InitialiseImageHelper()
		{
			_imageHelper = new MvxDynamicImageHelper<UIImage>();
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
		
		public override void SetSelected(bool selected, bool animated)
		{
			base.SetSelected(selected, animated);
			
			if (selected)
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