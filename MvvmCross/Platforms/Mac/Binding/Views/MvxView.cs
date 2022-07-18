// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Drawing;
using AppKit;
using Foundation;
using MvvmCross.Binding.BindingContext;

namespace MvvmCross.Platforms.Mac.Binding.Views
{
    public class MvxView
        : NSView
        , IMvxBindable
    {
        public IMvxBindingContext BindingContext { get; set; }

        public MvxView()
        {
            this.CreateBindingContext();
        }

        public MvxView(IntPtr handle)
            : base(handle)
        {
            this.CreateBindingContext();
        }

        public MvxView(NSCoder coder)
            : base(coder)
        {
            this.CreateBindingContext();
        }

        public MvxView(RectangleF frame)
            : base(frame)
        {
            this.CreateBindingContext();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.BindingContext.ClearAllBindings();
            }
            base.Dispose(disposing);
        }

        public object DataContext
        {
            get { return this.BindingContext.DataContext; }
            set { this.BindingContext.DataContext = value; }
        }
    }
}
