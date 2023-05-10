// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using CoreGraphics;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings;
using UIKit;

namespace MvvmCross.Platforms.Tvos.Binding.Views
{
    public class MvxCollectionViewCell
        : UICollectionViewCell, IMvxBindable
    {
        public IMvxBindingContext BindingContext { get; set; }

        public MvxCollectionViewCell(string bindingText)
        {
            this.CreateBindingContext(bindingText);
        }

        public MvxCollectionViewCell(IntPtr handle)
            : base(handle)
        {
            this.CreateBindingContext();
        }

        public MvxCollectionViewCell(string bindingText, IntPtr handle)
            : base(handle)
        {
            this.CreateBindingContext(bindingText);
        }

        public MvxCollectionViewCell(CGRect frame)
            : base(frame)
        {
            this.CreateBindingContext();
        }

        public MvxCollectionViewCell(string bindingText, CGRect frame)
            : base(frame)
        {
            this.CreateBindingContext(bindingText);
        }

        public MvxCollectionViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions, CGRect frame)
            : base(frame)
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

        public object DataContext
        {
            get { return BindingContext.DataContext; }
            set { BindingContext.DataContext = value; }
        }
    }
}
