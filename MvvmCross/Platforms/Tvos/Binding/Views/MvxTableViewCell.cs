// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings;
using ObjCRuntime;

namespace MvvmCross.Platforms.Tvos.Binding.Views
{
    public class MvxTableViewCell
        : UITableViewCell, IMvxBindable
    {
        public IMvxBindingContext BindingContext { get; set; }

        [RequiresUnreferencedCode("This constructor creates bindings which use reflection and may not be preserved by trimming.")]
        public MvxTableViewCell()
            : this(string.Empty)
        {
        }

        [RequiresUnreferencedCode("This constructor creates bindings which use reflection and may not be preserved by trimming.")]
        public MvxTableViewCell(string bindingText)
        {
            this.CreateBindingContext(bindingText);
        }

        [RequiresUnreferencedCode("This constructor creates bindings which use reflection and may not be preserved by trimming.")]
        public MvxTableViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions)
        {
            this.CreateBindingContext(bindingDescriptions);
        }

        [RequiresUnreferencedCode("This constructor creates bindings which use reflection and may not be preserved by trimming.")]
        public MvxTableViewCell(string bindingText, CGRect frame)
            : base(frame)
        {
            this.CreateBindingContext(bindingText);
        }

        [RequiresUnreferencedCode("This constructor creates bindings which use reflection and may not be preserved by trimming.")]
        public MvxTableViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions, CGRect frame)
            : base(frame)
        {
            this.CreateBindingContext(bindingDescriptions);
        }

        [RequiresUnreferencedCode("This constructor creates bindings which use reflection and may not be preserved by trimming.")]
        public MvxTableViewCell(NativeHandle handle)
            : this(string.Empty, handle)
        {
        }

        [RequiresUnreferencedCode("This constructor creates bindings which use reflection and may not be preserved by trimming.")]
        public MvxTableViewCell(string bindingText, NativeHandle handle)
            : base(handle)
        {
            this.CreateBindingContext(bindingText);
        }

        [RequiresUnreferencedCode("This constructor creates bindings which use reflection and may not be preserved by trimming.")]
        public MvxTableViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions, NativeHandle handle)
            : base(handle)
        {
            this.CreateBindingContext(bindingDescriptions);
        }

        [RequiresUnreferencedCode("This constructor creates bindings which use reflection and may not be preserved by trimming.")]
        public MvxTableViewCell(string bindingText, UITableViewCellStyle cellStyle, NSString cellIdentifier,
                                UITableViewCellAccessory tableViewCellAccessory = UITableViewCellAccessory.None)
            : base(cellStyle, cellIdentifier)
        {
            Accessory = tableViewCellAccessory;
            this.CreateBindingContext(bindingText);
        }

        [RequiresUnreferencedCode("This constructor creates bindings which use reflection and may not be preserved by trimming.")]
        public MvxTableViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions,
                                UITableViewCellStyle cellStyle, NSString cellIdentifier,
                                UITableViewCellAccessory tableViewCellAccessory = UITableViewCellAccessory.None)
            : base(cellStyle, cellIdentifier)
        {
            // note that we allow the virtual Accessory property to be set here - but do not seal
            // it. Previous `sealed` code caused odd, unexplained behaviour in MonoTouch
            // - see https://github.com/MvvmCross/MvvmCross/issues/524
            Accessory = tableViewCellAccessory;
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
