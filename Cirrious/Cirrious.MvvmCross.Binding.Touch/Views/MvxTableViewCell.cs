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
using Cirrious.MvvmCross.Binding.Touch.Interfaces;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Cirrious.MvvmCross.Binding.BindingContext;

namespace Cirrious.MvvmCross.Binding.Touch.Views
{
    public class MvxTableViewCell
        : UITableViewCell
          , IMvxBindableView
    {
        public IMvxBaseBindingContext BindingContext { get; set; }

        public MvxTableViewCell(string bindingText)
        {
			this.CreateBindingContext(bindingText);
        }

        public MvxTableViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions)
        {
			this.CreateBindingContext(bindingDescriptions);
        }

		public MvxTableViewCell(string bindingText, RectangleF frame)
            : base(frame)
        {
			this.CreateBindingContext(bindingText);
        }

        public MvxTableViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions, RectangleF frame)
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
                                    UITableViewCellAccessory tableViewCellAccessory =
                                        UITableViewCellAccessory.None)
            : base(cellStyle, cellIdentifier)
        {
            Accessory = tableViewCellAccessory;
			this.CreateBindingContext(bindingText);
        }

        public MvxTableViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions,
                                    UITableViewCellStyle cellStyle, NSString cellIdentifier,
                                    UITableViewCellAccessory tableViewCellAccessory =
                                        UITableViewCellAccessory.None)
            : base(cellStyle, cellIdentifier)
        {
            Accessory = tableViewCellAccessory;
			this.CreateBindingContext(bindingDescriptions);        
		}

		public override void MovedToSuperview ()
		{
			base.MovedToSuperview ();
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

        public virtual object DataContext
        {
            get { return BindingContext.DataContext; }
            set { BindingContext.DataContext = value; }
        }
    }
}