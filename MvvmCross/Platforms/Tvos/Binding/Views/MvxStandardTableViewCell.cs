// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Windows.Input;
using Foundation;
using MvvmCross.Binding.Bindings;
using UIKit;

namespace MvvmCross.Platforms.Tvos.Binding.Views
{
    public class MvxStandardTableViewCell
        : MvxTableViewCell
    {
        public MvxStandardTableViewCell(IntPtr handle)
            : this("TitleText" /* default binding is ToString() on the passed in item */, handle)
        {
        }

        public MvxStandardTableViewCell(string bindingText, IntPtr handle)
            : base(bindingText, handle)
        {
        }

        public MvxStandardTableViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions, IntPtr handle)
            : base(bindingDescriptions, handle)
        {
        }

        public MvxStandardTableViewCell(string bindingText, UITableViewCellStyle cellStyle, NSString cellIdentifier,
                                        UITableViewCellAccessory tableViewCellAccessory = UITableViewCellAccessory.None)
            : base(bindingText, cellStyle, cellIdentifier, tableViewCellAccessory)
        {
        }

        public MvxStandardTableViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions,
                                        UITableViewCellStyle cellStyle, NSString cellIdentifier,
                                        UITableViewCellAccessory tableViewCellAccessory = UITableViewCellAccessory.None)
            : base(bindingDescriptions, cellStyle, cellIdentifier, tableViewCellAccessory)
        {
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

        public ICommand SelectedCommand { get; set; }

        private bool _isSelected;

        public override void SetSelected(bool selected, bool animated)
        {
            base.SetSelected(selected, animated);

            if (_isSelected == selected)
                return;

            _isSelected = selected;
            if (_isSelected)
            {
                SelectedCommand?.Execute(null);
            }
        }
    }
}
