namespace MvvmCross.Mac.Views
{
    using System;

    using AppKit;
    using Foundation;

    using global::MvvmCross.Binding.BindingContext;
    using global::MvvmCross.Core.ViewModels;

    using MvvmCross.Platform.Mac.Views;

    public class MvxViewController
        : MvxEventSourceViewController
            , IMvxMacView
    {
        // Called when created from unmanaged code
        public MvxViewController(IntPtr handle) : base(handle)
        {
            this.Initialize();
        }

        // Called when created directly from a XIB file
        [Export("initWithCoder:")]
        public MvxViewController(NSCoder coder) : base(coder)
        {
            this.Initialize();
        }

        // Call to load from the XIB/NIB file
        public MvxViewController(string viewName, NSBundle bundle) : base(viewName, bundle)
        {
            this.Initialize();
        }

        // Call to load from the XIB/NIB file
        public MvxViewController(string viewName) : base(viewName, NSBundle.MainBundle)
        {
            this.Initialize();
        }

        public MvxViewController() : base()
        {
            this.Initialize();
        }

        // Shared initialization code
        private void Initialize()
        {
            this.AdaptForBinding();
        }

        public object DataContext
        {
            get { return this.BindingContext.DataContext; }
            set { this.BindingContext.DataContext = value; }
        }

        public IMvxViewModel ViewModel
        {
            get { return (IMvxViewModel)this.DataContext; }
            set { this.DataContext = value; }
        }

        public MvxViewModelRequest Request { get; set; }

        public IMvxBindingContext BindingContext { get; set; }
    }
}