using System;
using System.Collections.Generic;
using System.Linq;
using AppKit;
using Foundation;
using MvvmCross.Binding.Mac.Views;

namespace $rootnamespace$.Views
{
    public partial class MainView : MvxView
    {
        #region Constructors

        // Called when created from unmanaged code
        public MainView(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        // Called when created directly from a XIB file
        [Export("initWithCoder:")]
        public MainView(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        // Shared initialization code
        void Initialize()
        {
        }

        #endregion
    }
}
