// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings;
using UIKit;

namespace MvvmCross.Platforms.Ios.Binding.Views
{
    public class MvxTableViewCell
        : UITableViewCell, IMvxBindable
    {
        public IMvxBindingContext BindingContext { get; set; }

        public MvxTableViewCell()
            : this(string.Empty)
        {
        }

        public MvxTableViewCell(string bindingText)
        {
            this.CreateBindingContext(bindingText);
        }

        public MvxTableViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions)
        {
            this.CreateBindingContext(bindingDescriptions);
        }

        public MvxTableViewCell(string bindingText, CGRect frame)
            : base(frame)
        {
            this.CreateBindingContext(bindingText);
        }

        public MvxTableViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions, CGRect frame)
            : base(frame)
        {
            this.CreateBindingContext(bindingDescriptions);
        }

        public MvxTableViewCell(IntPtr handle)
            : this(string.Empty, handle)
        {
        }

        public MvxTableViewCell(string bindingText, IntPtr handle)
            : base(handle)
        {
            this.CreateBindingContext(bindingText);
        }

        public MvxTableViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions, IntPtr handle)
            : base(handle)
        {
            this.CreateBindingContext(bindingDescriptions);
        }

        public MvxTableViewCell(string bindingText, UITableViewCellStyle cellStyle, NSString cellIdentifier,
                                UITableViewCellAccessory tableViewCellAccessory = UITableViewCellAccessory.None)
            : base(cellStyle, cellIdentifier)
        {
            Accessory = tableViewCellAccessory;
            this.CreateBindingContext(bindingText);
        }

        public MvxTableViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions,
                                UITableViewCellStyle cellStyle, NSString cellIdentifier,
                                UITableViewCellAccessory tableViewCellAccessory = UITableViewCellAccessory.None)
            : base(cellStyle, cellIdentifier)
        {
            // note that we allow the virtual Accessory property to be set here - but do not seal
            // it. Previous `sealed` code caused odd, unexplained behaviour in MonoTouch
            // - see https://github.com/MvvmCross/MvvmCross/issues/524
            Accessory = tableViewCellAccessory;
            this.CreateBindingContext(bindingDescriptions);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                BindingContext.ClearAllBindings();
            }
            base.Dispose(disposing);
        }

        public object DataContext
        {
            get { return BindingContext.DataContext; }
            set { BindingContext.DataContext = value; }
        }
    }
}
