// MvxBaseTableViewCell.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Drawing;
using Cirrious.MvvmCross.Binding.Interfaces;
using Cirrious.MvvmCross.Binding.Interfaces.BindingContext;
using Cirrious.MvvmCross.Binding.Touch.BindingContext;
using Cirrious.MvvmCross.Binding.Touch.Interfaces;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Views
{
    public class MvxBaseTableViewCell
        : UITableViewCell
        , IMvxBindableView
    {
        public IMvxBaseBindingContext<UIView> BindingContext { get; set; }
        
        public MvxBaseTableViewCell(string bindingText)
        {
            BindingContext = new MvxBindingContext(this, bindingText);
        }

        public MvxBaseTableViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions)
        {
            BindingContext = new MvxBindingContext(this, bindingDescriptions);
        }

        public MvxBaseTableViewCell(RectangleF frame, string bindingText)
            : base(frame)
        {
            BindingContext = new MvxBindingContext(this, bindingText);
        }

        public MvxBaseTableViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions, RectangleF frame)
            : base(frame)
        {
            BindingContext = new MvxBindingContext(this, bindingDescriptions);
        }

        public MvxBaseTableViewCell(string bindingText, IntPtr handle)
            : base(handle)
        {
            BindingContext = new MvxBindingContext(this, bindingText);
        }

        public MvxBaseTableViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions, IntPtr handle)
            : base(handle)
        {
            BindingContext = new MvxBindingContext(this, bindingDescriptions);
        }

        public MvxBaseTableViewCell(string bindingText, UITableViewCellStyle cellStyle, NSString cellIdentifier,
                                    UITableViewCellAccessory tableViewCellAccessory =
                                        UITableViewCellAccessory.None)
            : base(cellStyle, cellIdentifier)
        {
            Accessory = tableViewCellAccessory;
            BindingContext = new MvxBindingContext(this, bindingText);
        }

        public MvxBaseTableViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions,
                                    UITableViewCellStyle cellStyle, NSString cellIdentifier,
                                    UITableViewCellAccessory tableViewCellAccessory =
                                        UITableViewCellAccessory.None)
            : base(cellStyle, cellIdentifier)
        {
            Accessory = tableViewCellAccessory;
            BindingContext = new MvxBindingContext(this, bindingDescriptions);
        }

        // we seal Accessory here so that we can use it in the constructor - otherwise virtual issues.
        public override sealed UITableViewCellAccessory Accessory
        {
            get { return base.Accessory; }
            set { base.Accessory = value; }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
#warning ClearAllBindings is better as Dispose?
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