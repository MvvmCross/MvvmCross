// MvxView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.MvvmCross.Binding.BindingContext;
using System;
using System.Drawing;

#if __UNIFIED__
using AppKit;
using Foundation;
#else
#endif

namespace Cirrious.MvvmCross.Binding.Mac.Views
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