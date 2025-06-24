// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using CoreGraphics;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings;
using ObjCRuntime;
using UIKit;

namespace MvvmCross.Platforms.Ios.Binding.Views
{
    public class MvxCollectionViewListCell
        : UICollectionViewListCell, IMvxBindable
    {
        public IMvxBindingContext BindingContext { get; set; }

        public MvxCollectionViewListCell()
            : this(string.Empty)
        {
        }

        public MvxCollectionViewListCell(string bindingText)
        {
            this.CreateBindingContext(bindingText);
        }

        public MvxCollectionViewListCell(IEnumerable<MvxBindingDescription> bindingDescriptions)
        {
            this.CreateBindingContext(bindingDescriptions);
        }

        public MvxCollectionViewListCell(string bindingText, CGRect frame)
            : base(frame)
        {
            this.CreateBindingContext(bindingText);
        }

        public MvxCollectionViewListCell(IEnumerable<MvxBindingDescription> bindingDescriptions, CGRect frame)
            : base(frame)
        {
            this.CreateBindingContext(bindingDescriptions);
        }

        public MvxCollectionViewListCell(NativeHandle handle)
            : this(string.Empty, handle)
        {
        }

        public MvxCollectionViewListCell(string bindingText, NativeHandle handle)
            : base(handle)
        {
            this.CreateBindingContext(bindingText);
        }

        public MvxCollectionViewListCell(IEnumerable<MvxBindingDescription> bindingDescriptions, NativeHandle handle)
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

        public object DataContext
        {
            get { return BindingContext.DataContext; }
            set { BindingContext.DataContext = value; }
        }
    }
}
