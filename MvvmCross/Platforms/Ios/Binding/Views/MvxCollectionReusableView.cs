// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using CoreGraphics;
using MvvmCross.Binding.Attributes;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings;
using UIKit;

namespace MvvmCross.Platforms.Ios.Binding.Views
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
