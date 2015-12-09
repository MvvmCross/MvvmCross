// MvxView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.Binding.Touch.Views
{
    using System;

    using CoreGraphics;

    using UIKit;

    public class MvxView
        : UIView
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

        public MvxView(CGRect frame)
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

        [MvxSetToNullAfterBinding]
        public object DataContext
        {
            get { return this.BindingContext.DataContext; }
            set { this.BindingContext.DataContext = value; }
        }
    }
}