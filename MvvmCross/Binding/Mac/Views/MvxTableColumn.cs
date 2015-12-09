// MvxView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com


#if __UNIFIED__
using AppKit;
using Foundation;
#else
#endif

namespace MvvmCross.Binding.Mac.Views
{
    using System;

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