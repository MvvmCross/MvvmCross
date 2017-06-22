using System;
using System.Linq;
using AppKit;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Mac.Views;

namespace MvvmCross.Mac.Views
{
    public class MvxTabViewController : MvxEventSourceTabViewController, IMvxTabViewController, IMvxMacView
    {
        protected MvxTabViewController()
            : base()
        {
            this.Initialize();
        }

        protected MvxTabViewController(NSCoder coder)
            : base(coder)
        {
            this.Initialize();
        }

        protected MvxTabViewController(IntPtr handle)
            : base(handle)
        {
            this.Initialize();
        }

        protected MvxTabViewController(NSObjectFlag flag)
            : base(flag)
        {
            this.Initialize();
        }

        // Shared initialization code
        private void Initialize()
        {
            this.AdaptForBinding();
        }

        public void ShowTabView(NSViewController viewController, string tabTitle)
        {
            AddChildViewController(viewController);

            if (!string.IsNullOrEmpty(tabTitle))
                TabViewItems[ChildViewControllers.Count() - 1].Label = tabTitle;
        }

        public bool CloseTabView(IMvxViewModel viewModel)
        {
            var index = ChildViewControllers.Select(v => (MvxViewController)v).ToList().FindIndex(vc => viewModel == vc.ViewModel);

            if (index >= 0)
            {
                RemoveChildViewController(index);
                return true;
            }

            return false;
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

    public class MvxTabViewController<TViewModel>
        : MvxTabViewController, IMvxMacView<TViewModel> where TViewModel : class, IMvxViewModel
    {
        public MvxTabViewController()
        {
        }

        public MvxTabViewController(IntPtr handle)
            : base(handle)
        {
        }

        protected MvxTabViewController(NSObjectFlag flag)
            : base(flag)
        {
        }

        public MvxTabViewController(NSCoder coder) : base(coder)
        {
        }

        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}
