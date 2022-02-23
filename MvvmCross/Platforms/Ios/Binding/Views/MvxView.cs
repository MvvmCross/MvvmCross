// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.Attributes;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace MvvmCross.Platforms.Ios.Binding.Views
{
    public class MvxView
        : UIView
        , IMvxBindable
    {
        public IMvxBindingContext BindingContext { get; set; }

        // Constructor that will bind managed object to its unmanaged counterpart. This constructor 
        // should not have any implementation and is only used for types that can be created by the
        // interface builder (or Xamarin iOS designer). More documentation can be found:
        // - here: https://developer.xamarin.com/guides/ios/user_interface/designer/ios_designable_controls_overview/
        // - and here: https://developer.xamarin.com/guides/ios/under_the_hood/api_design/#Types_and_Interface_Builder
        public MvxView(IntPtr handle) : base(handle) { }

        public MvxView()
        {
            this.CreateBindingContext();
        }

        public MvxView(CGRect frame)
            : base(frame)
        {
            this.CreateBindingContext();
        }

        public MvxView(NSCoder coder)
            : base(coder)
        {
            this.CreateBindingContext();
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            if (BindingContext == null)
            {
                this.CreateBindingContext();
            }
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
