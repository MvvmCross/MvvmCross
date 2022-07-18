// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using AppKit;
using Foundation;

namespace MvvmCross.Platforms.Mac.Views
{
    public class MvxWindowController
        : NSWindowController
    {
        // Called when created from unmanaged code
        public MvxWindowController(IntPtr handle) : base(handle)
        {
        }

        public MvxWindowController(NSWindow window) : base(window)
        {
        }

        // Called when created directly from a XIB file
        public MvxWindowController(NSCoder coder) : base(coder)
        {
        }

        // Call to load from the XIB/NIB file
        public MvxWindowController(string viewName, NSBundle bundle) : base(viewName, bundle)
        {
        }

        // Call to load from the XIB/NIB file
        public MvxWindowController(string viewName) : base(viewName, NSBundle.MainBundle)
        {
        }

        public MvxWindowController() : base()
        {
        }
    }
}
