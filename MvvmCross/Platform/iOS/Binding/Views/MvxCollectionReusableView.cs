// MvxCollectionReusableView.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using CoreGraphics;
using MvvmCross.Binding.Attributes;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings;
using UIKit;

namespace MvvmCross.Binding.iOS.Views
{
    public class MvxCollectionReusableView
        : UICollectionReusableView, IMvxBindable
    {
        public IMvxBindingContext BindingContext { get; set; }

        public MvxCollectionReusableView()
            : this(string.Empty)
        {
        }

        public MvxCollectionReusableView(string bindingText)
        {
            this.CreateBindingContext(bindingText);
        }

        public MvxCollectionReusableView(IEnumerable<MvxBindingDescription> bindingDescriptions)
        {
            this.CreateBindingContext(bindingDescriptions);
        }

        public MvxCollectionReusableView(string bindingText, CGRect frame)
            : base(frame)
        {
            this.CreateBindingContext(bindingText);
        }

        public MvxCollectionReusableView(IEnumerable<MvxBindingDescription> bindingDescriptions, CGRect frame)
            : base(frame)
        {
            this.CreateBindingContext(bindingDescriptions);
        }

        public MvxCollectionReusableView(IntPtr handle)
            : this(string.Empty, handle)
        {
        }

        public MvxCollectionReusableView(string bindingText, IntPtr handle)
            : base(handle)
        {
            this.CreateBindingContext(bindingText);
        }

        public MvxCollectionReusableView(IEnumerable<MvxBindingDescription> bindingDescriptions, IntPtr handle)
            : base(handle)
        {
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

        [MvxSetToNullAfterBinding]
        public object DataContext
        {
            get { return BindingContext.DataContext; }
            set { BindingContext.DataContext = value; }
        }
    }
}