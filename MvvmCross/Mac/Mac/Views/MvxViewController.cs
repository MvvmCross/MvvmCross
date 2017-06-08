namespace MvvmCross.Mac.Views
{
    using System;

    using AppKit;
    using Foundation;

    using Binding.BindingContext;
    using Core.ViewModels;

    using MvvmCross.Platform.Mac.Views;

    public class MvxViewController
        : MvxEventSourceViewController
            , IMvxMacView
    {
        // Called when created from unmanaged code
        public MvxViewController(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        // Called when created directly from a XIB file
        [Export("initWithCoder:")]
        public MvxViewController(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        // Call to load from the XIB/NIB file
        public MvxViewController(string viewName, NSBundle bundle) : base(viewName, bundle)
        {
            Initialize();
        }

        // Call to load from the XIB/NIB file
        public MvxViewController(string viewName) : base(viewName, NSBundle.MainBundle)
        {
            Initialize();
        }

        public MvxViewController() : base()
        {
            Initialize();
        }

        // Shared initialization code
        private void Initialize()
        {
            this.AdaptForBinding();
        }

        public object DataContext
        {
            get { return BindingContext.DataContext; }
            set { BindingContext.DataContext = value; }
        }

        public IMvxViewModel ViewModel
        {
            get { return (IMvxViewModel)DataContext; }
            set { DataContext = value; }
        }

        public MvxViewModelRequest Request { get; set; }

        public IMvxBindingContext BindingContext { get; set; }
    }
}