// MvxView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;

#if __UNIFIED__
using AppKit;
using Foundation;
#else
#endif

namespace Cirrious.MvvmCross.Binding.Mac.Views
{
    [Register("MvxTableColumn")]
    public class MvxTableColumn : NSTableColumn
    {
        // Called when created from unmanaged code
        public MvxTableColumn(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        // Called when created directly from a XIB file
        [Export("initWithCoder:")]
        public MvxTableColumn(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        public MvxTableColumn() : base()
        {
            Initialize();
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
                BindingText = value.ToString();
        }
    }
}