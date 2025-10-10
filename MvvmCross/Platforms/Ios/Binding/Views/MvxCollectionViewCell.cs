// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CoreGraphics;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings;
using ObjCRuntime;
using UIKit;

namespace MvvmCross.Platforms.Ios.Binding.Views
{
    public class MvxCollectionViewCell
        : UICollectionViewCell, IMvxBindable
    {
        public IMvxBindingContext BindingContext { get; set; }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public MvxCollectionViewCell()
            : this(string.Empty)
        {
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public MvxCollectionViewCell(string bindingText)
        {
            this.CreateBindingContext(bindingText);
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public MvxCollectionViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions)
        {
            this.CreateBindingContext(bindingDescriptions);
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public MvxCollectionViewCell(string bindingText, CGRect frame)
            : base(frame)
        {
            this.CreateBindingContext(bindingText);
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public MvxCollectionViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions, CGRect frame)
            : base(frame)
        {
            this.CreateBindingContext(bindingDescriptions);
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public MvxCollectionViewCell(NativeHandle handle)
            : this(string.Empty, handle)
        {
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public MvxCollectionViewCell(string bindingText, NativeHandle handle)
            : base(handle)
        {
            this.CreateBindingContext(bindingText);
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        public MvxCollectionViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions, NativeHandle handle)
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
