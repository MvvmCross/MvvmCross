using System;
using System.Collections.Generic;
using System.Linq;
using AppKit;
using Foundation;
using MvvmCross.Mac.Views;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;

namespace $rootnamespace$.Views
{
    [MvxViewFor(typeof(Core.ViewModels.MainViewModel))]
    public partial class MainViewController : MvxViewController
    {
        #region Constructors

        // Called when created from unmanaged code
        public MainViewController(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        // Called when created directly from a XIB file
        [Export("initWithCoder:")]
        public MainViewController(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        // Call to load from the XIB/NIB file
        public MainViewController() : base()
        {
            Initialize();
        }

        // Shared initialization code
        void Initialize()
        {
        }

        #endregion

        // strongly typed view accessor
        public new MainView View
        {
            get
            {
                return (MainView)base.View;
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad ();

            var set = this.CreateBindingSet<MainViewController, Core.ViewModels.MainViewModel>();
            //set.Bind(textMain).To(vm => vm.Hello);
            set.Apply();
        }
    }
}
