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
    [MvxViewFor(typeof(Core.ViewModels.FirstViewModel))]
    public partial class FirstViewController : MvxViewController
    {
        #region Constructors

        // Called when created from unmanaged code
        public FirstViewController(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        // Called when created directly from a XIB file
        [Export("initWithCoder:")]
        public FirstViewController(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        // Call to load from the XIB/NIB file
        public FirstViewController() : base()
        {
            Initialize();
        }

        // Shared initialization code
        void Initialize()
        {
        }

        #endregion

        // strongly typed view accessor
        public new FirstView View
        {
            get
            {
                return (FirstView)base.View;
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad ();

            var set = this.CreateBindingSet<FirstViewController, Core.ViewModels.FirstViewModel>();
            //set.Bind(textFirst).To(vm => vm.Hello);
            set.Apply();
        }
    }
}
