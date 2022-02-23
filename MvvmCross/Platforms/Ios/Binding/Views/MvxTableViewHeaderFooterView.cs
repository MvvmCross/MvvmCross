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
    public class MvxTableViewHeaderFooterView
        : UITableViewHeaderFooterView, IMvxBindable
    {
        public IMvxBindingContext BindingContext { get; set; }

        public MvxTableViewHeaderFooterView()
            : this(string.Empty)
        {
        }

        public MvxTableViewHeaderFooterView(string bindingText)
        {
            this.CreateBindingContext(bindingText);
        }

        public MvxTableViewHeaderFooterView(IEnumerable<MvxBindingDescription> bindingDescriptions)
        {
            this.CreateBindingContext(bindingDescriptions);
        }

        public MvxTableViewHeaderFooterView(string bindingText, CGRect frame)
            : base(frame)
        {
            this.CreateBindingContext(bindingText);
        }

        public MvxTableViewHeaderFooterView(IEnumerable<MvxBindingDescription> bindingDescriptions, CGRect frame)
            : base(frame)
        {
            this.CreateBindingContext(bindingDescriptions);
        }

        public MvxTableViewHeaderFooterView(IntPtr handle)
            : this(string.Empty, handle)
        {
        }

        public MvxTableViewHeaderFooterView(string bindingText, IntPtr handle)
            : base(handle)
        {
            this.CreateBindingContext(bindingText);
        }

        public MvxTableViewHeaderFooterView(IEnumerable<MvxBindingDescription> bindingDescriptions, IntPtr handle)
            : base(handle)
        {
            this.CreateBindingContext(bindingDescriptions);
        }

        public MvxTableViewHeaderFooterView(NSString reuseIdentifier)
            : this(string.Empty, reuseIdentifier)
        {
        }

        public MvxTableViewHeaderFooterView(string bindingText, NSString reuseIdentifier)
            : base(reuseIdentifier)
        {
            this.CreateBindingContext(bindingText);
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
