// MvxCollectionReusableView.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using CoreGraphics;
using MvvmCross.Binding.Attributes;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MvvmCross.Binding.tvOS.Views
{
    public class MvxCollectionReusableView
        : UICollectionReusableView
            , IMvxBindable
    {
        public MvxCollectionReusableView()
        {
            this.CreateBindingContext();
        }

        public MvxCollectionReusableView(IntPtr handle)
            : base(handle)
        {
            this.CreateBindingContext();
        }

        public MvxCollectionReusableView(CGRect frame)
            : base(frame)
        {
            this.CreateBindingContext();
        }

        public IMvxBindingContext BindingContext { get; set; }

        [MvxSetToNullAfterBinding]
        public object DataContext
        {
            get => BindingContext.DataContext;
            set => BindingContext.DataContext = value;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                BindingContext.ClearAllBindings();
            base.Dispose(disposing);
        }
    }
}