// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;
using UIKit;

namespace MvvmCross.Platforms.Ios.Views
{
    public class MvxSplitViewController : MvxBaseSplitViewController, IMvxSplitViewController
    {
        public MvxSplitViewController() : base()
        {
        }

        public MvxSplitViewController(NSCoder coder) : base(coder)
        {
        }

        protected MvxSplitViewController(NSObjectFlag t) : base(t)
        {
        }

        protected internal MvxSplitViewController(IntPtr handle) : base(handle)
        {
        }

        public MvxSplitViewController(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            PreferredDisplayMode = UISplitViewControllerDisplayMode.AllVisible;
        }

        public virtual void ShowDetailView(UIViewController viewController, MvxSplitViewPresentationAttribute attribute)
        {
            viewController = attribute.WrapInNavigationController ? new MvxNavigationController(viewController) : viewController;

            ShowDetailViewController(viewController, this);
        }

        public virtual void ShowMasterView(UIViewController viewController, MvxSplitViewPresentationAttribute attribute)
        {
            var newStack = ViewControllers.ToList();

            viewController = attribute.WrapInNavigationController ? new MvxNavigationController(viewController) : viewController;

            if (newStack.Any())
                newStack.RemoveAt(0);

            newStack.Insert(0, viewController);

            ViewControllers = newStack.ToArray();
        }

        public virtual bool CloseChildViewModel(IMvxViewModel viewModel, MvxBasePresentationAttribute attribute)
        {
            if (!ViewControllers.Any())
                return false;

            var toClose = ViewControllers.ToList()
                                         .Select(v => v.GetIMvxIosView())
                                         .FirstOrDefault(mvxView => mvxView?.ViewModel == viewModel);
            if (toClose != null)
            {
                var newStack = ViewControllers.Where(v => v.GetIMvxIosView() != toClose);
                ViewControllers = newStack.ToArray();

                return true;
            }

            return false;
        }
    }

    public class MvxSplitViewController<TViewModel> : MvxSplitViewController, IMvxIosView<TViewModel>
        where TViewModel : class, IMvxViewModel
    {
        public MvxSplitViewController()
        {
        }

        public MvxSplitViewController(NSCoder coder) : base(coder)
        {
        }

        public MvxSplitViewController(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
        }

        protected MvxSplitViewController(NSObjectFlag t) : base(t)
        {
        }

        protected internal MvxSplitViewController(IntPtr handle) : base(handle)
        {
        }

        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public MvxFluentBindingDescriptionSet<IMvxIosView<TViewModel>, TViewModel> CreateBindingSet()
        {
            return this.CreateBindingSet<IMvxIosView<TViewModel>, TViewModel>();
        }
    }
}
