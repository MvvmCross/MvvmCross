// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings;
using ObjCRuntime;

namespace MvvmCross.Platforms.Tvos.Binding.Views
{
    public class MvxCollectionViewListCell : UICollectionViewListCell, IMvxBindable
    {
        public IMvxBindingContext BindingContext { get; set; }

        [RequiresUnreferencedCode("This constructor creates bindings which use reflection and may not be preserved by trimming.")]
        public MvxCollectionViewListCell(string bindingText)
        {
            this.CreateBindingContext(bindingText);
        }

        [RequiresUnreferencedCode("This constructor creates bindings which use reflection and may not be preserved by trimming.")]
        public MvxCollectionViewListCell(NativeHandle handle)
            : base(handle)
        {
            this.CreateBindingContext();
        }

        [RequiresUnreferencedCode("This constructor creates bindings which use reflection and may not be preserved by trimming.")]
        public MvxCollectionViewListCell(string bindingText, NativeHandle handle)
            : base(handle)
        {
            this.CreateBindingContext(bindingText);
        }

        [RequiresUnreferencedCode("This constructor creates bindings which use reflection and may not be preserved by trimming.")]
        public MvxCollectionViewListCell(CGRect frame)
            : base(frame)
        {
            this.CreateBindingContext();
        }

        [RequiresUnreferencedCode("This constructor creates bindings which use reflection and may not be preserved by trimming.")]
        public MvxCollectionViewListCell(string bindingText, CGRect frame)
            : base(frame)
        {
            this.CreateBindingContext(bindingText);
        }

        [RequiresUnreferencedCode("This constructor creates bindings which use reflection and may not be preserved by trimming.")]
        public MvxCollectionViewListCell(IEnumerable<MvxBindingDescription> bindingDescriptions, CGRect frame)
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
