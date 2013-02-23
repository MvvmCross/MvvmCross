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
using Cirrious.MvvmCross.Binding.Touch.Interfaces;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Cirrious.MvvmCross.Binding.Touch.Views
{
    public class MvxBaseTableViewCell
        : UITableViewCell
          , IMvxBindableView
    {
        public IList<IMvxUpdateableBinding> Bindings { get; set; }
        public Action CallOnNextDataContextChange { get; set; }

        public MvxBaseTableViewCell(string bindingText)
        {
            this.CreateFirstBindAction(bindingText);
        }

        public MvxBaseTableViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions)
        {
            this.CreateFirstBindAction(bindingDescriptions);
        }

        public MvxBaseTableViewCell(RectangleF frame, string bindingText)
            : base(frame)
        {
            this.CreateFirstBindAction(bindingText);
        }

        public MvxBaseTableViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions, RectangleF frame)
            : base(frame)
        {
            this.CreateFirstBindAction(bindingDescriptions);
        }

        public MvxBaseTableViewCell(string bindingText, IntPtr handle)
            : base(handle)
        {
            this.CreateFirstBindAction(bindingText);
        }

        public MvxBaseTableViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions, IntPtr handle)
            : base(handle)
        {
            this.CreateFirstBindAction(bindingDescriptions);
        }

        public MvxBaseTableViewCell(string bindingText, UITableViewCellStyle cellStyle, NSString cellIdentifier,
                                    UITableViewCellAccessory tableViewCellAccessory =
                                        UITableViewCellAccessory.None)
            : base(cellStyle, cellIdentifier)
        {
            Accessory = tableViewCellAccessory;
            this.CreateFirstBindAction(bindingText);
        }

        public MvxBaseTableViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions,
                                    UITableViewCellStyle cellStyle, NSString cellIdentifier,
                                    UITableViewCellAccessory tableViewCellAccessory =
                                        UITableViewCellAccessory.None)
            : base(cellStyle, cellIdentifier)
        {
            Accessory = tableViewCellAccessory;
            this.CreateFirstBindAction(bindingDescriptions);
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
                this.DisposeBindings();
            }
            base.Dispose(disposing);
        }

        private object _dataContext;

        public object DataContext
        {
            get { return _dataContext; }
            set
            {
                if (_dataContext == value
                    && CallOnNextDataContextChange == null)
                    return;

                _dataContext = value;
                this.OnDataContextChanged();
            }
        }
    }
}