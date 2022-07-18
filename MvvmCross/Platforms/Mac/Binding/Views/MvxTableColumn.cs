// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using AppKit;
using Foundation;

namespace MvvmCross.Platforms.Mac.Binding.Views
{
    [Register("MvxTableColumn")]
    public class MvxTableColumn : NSTableColumn
    {
        // Called when created from unmanaged code
        public MvxTableColumn(IntPtr handle) : base(handle)
        {
            this.Initialize();
        }

        // Called when created directly from a XIB file
        [Export("initWithCoder:")]
        public MvxTableColumn(NSCoder coder) : base(coder)
        {
            this.Initialize();
        }

        public MvxTableColumn() : base()
        {
            this.Initialize();
        }

        // Shared initialization code
        private void Initialize()
        {
        }

        public string BindingText
        {
            get;
            set;
        }

        public override void SetValueForKey(NSObject value, NSString key)
        {
            if (key == "bindingText")
                this.BindingText = value.ToString();
        }
    }
}
