using System;
using System.Linq;

using Foundation;
using UIKit;

using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;

namespace MvvmCross.tvOS.Views
{
    public class MvxSplitViewController: 
        MvxEventSourceSplitViewController, IMvxTvosView
    {

        public object DataContext
        {
            get { return BindingContext.DataContext; }
            set { BindingContext.DataContext = value; }
        }

        public IMvxViewModel ViewModel
        {
            get { return DataContext as IMvxViewModel; }
            set { DataContext = value; }
        }

        public MvxViewModelRequest Request { get; set; }

        public IMvxBindingContext BindingContext { get; set; }

        public MvxSplitViewController()
        {
            this.AdaptForBinding();
        }

        public MvxSplitViewController(IntPtr handle)
            : base(handle)
        {
            this.AdaptForBinding();
        }

        protected MvxSplitViewController(string nibName, NSBundle bundle)
            : base(nibName, bundle)
        {
            this.AdaptForBinding();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ViewModel?.ViewCreated();

            PreferredDisplayMode = UISplitViewControllerDisplayMode.AllVisible;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            ViewModel?.ViewAppearing();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            ViewModel?.ViewAppeared();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            ViewModel?.ViewDisappearing();
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            ViewModel?.ViewDisappeared();
        }

        public virtual void ShowDetailView(UIViewController viewController, bool wrapInNavigationController)
        {
            viewController = wrapInNavigationController ? 
                new MvxNavigationController(viewController) : viewController;

            ShowDetailViewController(viewController, this);
        }

        public virtual void ShowMasterView(UIViewController viewController, bool wrapInNavigationController)
        {
            var stack = ViewControllers.ToList();

            viewController = wrapInNavigationController 
                ? new MvxNavigationController(viewController) : viewController;

            if (stack.Any())
                stack.RemoveAt(0);

            stack.Insert(0, viewController);

            ViewControllers = stack.ToArray();
        }

        public override void DidMoveToParentViewController(UIViewController parent)
        {
            base.DidMoveToParentViewController(parent);
            if (parent == null)
            {
                ViewModel?.ViewDestroy();
            }
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
            this.ViewModelRequestForSegue(segue, sender);
        }

        public virtual bool CloseChildViewModel(IMvxViewModel viewModel)
        {
            if (!ViewControllers.Any())
                return false;

            var toClose = ViewControllers.ToList()
                                         .Select(v => v.GetIMvxTvosView())
                                         .FirstOrDefault(mvxView => mvxView.ViewModel == viewModel);
            if (toClose != null)
            {
                var newStack = ViewControllers.Where(v => v.GetIMvxTvosView() != toClose);
                ViewControllers = newStack.ToArray();

                return true;
            }

            return false;
        }
    }

    public class MvxSplitViewController<TViewModel> : MvxViewController, IMvxTvosView<TViewModel>
       where TViewModel : class, IMvxViewModel
    {
        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public MvxSplitViewController()
        {
        }

        public MvxSplitViewController(IntPtr handle)
            : base(handle)
        {
        }

        protected MvxSplitViewController(string nibName, NSBundle bundle)
            : base(nibName, bundle)
        {
        }
    }
}
