// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using AppKit;
using CoreGraphics;
using Foundation;
using MvvmCross.Base;
using MvvmCross.Binding.BindingContext;

namespace MvvmCross.Platforms.Mac.Binding.Views
{
    [Register("MvxTableCellView")]
    public class MvxTableCellView : NSTableCellView, IMvxBindingContextOwner, IMvxDataConsumer
    {
        // Called when created from unmanaged code
        public MvxTableCellView(IntPtr handle) : base(handle)
        {
            this.Initialize(string.Empty);
        }

        // Called when created directly from a XIB file
        [Export("initWithCoder:")]
        public MvxTableCellView(NSCoder coder) : base(coder)
        {
            this.Initialize(string.Empty);
        }

        public MvxTableCellView(string bindingText)
        {
            this.Frame = new CGRect(0, 0, 100, 17);
            TextField = new NSTextField(new CGRect(0, 0, 100, 17))
            {
                Editable = false,
                Bordered = false,
                BackgroundColor = NSColor.Clear,
            };

            AddSubview(TextField);
            this.Initialize(bindingText);
        }

        public override CGRect Frame
        {
            get
            {
                return base.Frame;
            }
            set
            {
                base.Frame = value;
                if (TextField != null)
                    TextField.Frame = value;
            }
        }

        public event EventHandler Click
        {
            add
            {
                TextField.Activated += value;
            }
            remove
            {
                TextField.Activated -= value;
            }
        }

        // Shared initialization code
        private void Initialize(string bindingText)
        {
            this.CreateBindingContext(bindingText);
        }

        public IMvxBindingContext BindingContext
        {
            get;
            set;
        }

        public object DataContext
        {
            get { return this.BindingContext.DataContext; }
            set { this.BindingContext.DataContext = value; }
        }

        public string Text
        {
            get
            {
                return this.TextField.StringValue;
            }
            set
            {
                this.TextField.StringValue = value;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.BindingContext.ClearAllBindings();
            }
            base.Dispose(disposing);
        }
    }
}
