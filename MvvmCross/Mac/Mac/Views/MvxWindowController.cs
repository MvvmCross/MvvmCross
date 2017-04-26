using System;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Mac.Views;

namespace MvvmCross.Mac.Views
{
    public class MvxWindowController
        : MvxEventSourceViewController
            , IMvxMacView
    {
        // Called when created from unmanaged code
        public MvxWindowController(IntPtr handle) : base(handle)
        {
            this.Initialize();
        }

        // Called when created directly from a XIB file
        [Export("initWithCoder:")]
        public MvxWindowController(NSCoder coder) : base(coder)
        {
            this.Initialize();
        }

        // Call to load from the XIB/NIB file
        public MvxWindowController(string viewName, NSBundle bundle) : base(viewName, bundle)
        {
            this.Initialize();
        }

        // Call to load from the XIB/NIB file
        public MvxWindowController(string viewName) : base(viewName, NSBundle.MainBundle)
        {
            this.Initialize();
        }

        public MvxWindowController() : base()
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

    public class MvxWindowController<TViewModel>
        : MvxWindowController
          , IMvxMacView<TViewModel> where TViewModel : class, IMvxViewModel
    {
        public MvxWindowController()
        {
        }

        public MvxWindowController(IntPtr handle)
            : base(handle)
        {
        }

        protected MvxWindowController(string nibName, NSBundle bundle)
            : base(nibName, bundle)
        {
        }

        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}
