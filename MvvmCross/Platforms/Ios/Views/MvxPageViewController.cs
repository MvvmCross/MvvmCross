// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Foundation;
using MvvmCross.Logging;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Views.Base;
using MvvmCross.ViewModels;
using UIKit;

namespace MvvmCross.Platforms.Ios.Views
{
    public class MvxPageViewController : MvxEventSourcePageViewController, IMvxIosView
    {
        public MvxPageViewController(UIPageViewControllerTransitionStyle style = UIPageViewControllerTransitionStyle.Scroll, UIPageViewControllerNavigationOrientation navigationOrientation = UIPageViewControllerNavigationOrientation.Horizontal, UIPageViewControllerSpineLocation spineLocation = UIPageViewControllerSpineLocation.None) : base(style, navigationOrientation, spineLocation)
        {
            this.AdaptForBinding();
        }

        public MvxPageViewController(UIPageViewControllerTransitionStyle style, UIPageViewControllerNavigationOrientation navigationOrientation, UIPageViewControllerSpineLocation spineLocation, float interPageSpacing) : base(style, navigationOrientation, spineLocation, interPageSpacing)
        {
            this.AdaptForBinding();
        }

        public MvxPageViewController(UIPageViewControllerTransitionStyle style, UIPageViewControllerNavigationOrientation navigationOrientation) : base(style, navigationOrientation)
        {
            this.AdaptForBinding();
        }

        public MvxPageViewController(NSCoder coder) : base(coder)
        {
            this.AdaptForBinding();
        }

        protected MvxPageViewController(NSObjectFlag t) : base(t)
        {
            this.AdaptForBinding();
        }

        protected internal MvxPageViewController(IntPtr handle) : base(handle)
        {
            this.AdaptForBinding();
        }

        public MvxPageViewController(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
            this.AdaptForBinding();
        }

        public MvxPageViewController(UIPageViewControllerTransitionStyle style, UIPageViewControllerNavigationOrientation navigationOrientation, NSDictionary options) : base(style, navigationOrientation, options)
        {
            this.AdaptForBinding();
        }

        private Dictionary<string, UIViewController> _pagedViewControllerCache = new Dictionary<string, UIViewController>();

        public MvxViewModelRequest Request { get; set; }
        public IMvxBindingContext BindingContext { get; set; }

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

        public override void DidMoveToParentViewController(UIViewController parent)
        {
            base.DidMoveToParentViewController(parent);
            if (parent == null)
                ViewModel?.ViewDestroy();
        }

        public IMvxViewModel ViewModel
        {
            get
            {
                return DataContext as IMvxViewModel;
            }
            set
            {
                DataContext = value;
                //Verify ViewModel is IMvxPageViewModel
                if (DataContext != null && !(DataContext is IMvxPageViewModel))
                    MvxLog.Instance.Error("Error - MvxPageViewController must be given an instance of IMvxPageViewModel");
            }
        }

        public object DataContext
        {
            get { return BindingContext.DataContext; }
            set { BindingContext.DataContext = value; }
        }

        protected virtual void InitializePaging()
        {
            IMvxPageViewModel pageVM = ViewModel as IMvxPageViewModel;
            if (pageVM == null)
                return;
            IMvxPagedViewModel defaultVM = pageVM.GetDefaultViewModel();
            UIViewController defaultVC = GetViewControllerForViewModel(defaultVM);
            SetViewControllers(new UIViewController[] { defaultVC }, UIPageViewControllerNavigationDirection.Forward, true, null);
            GetNextViewController = delegate (UIPageViewController pc, UIViewController rc)
            {
                IMvxIosView rcTV = rc as IMvxIosView;
                if (rcTV == null)
                    return null;
                IMvxPagedViewModel currentVM = rcTV.ViewModel as IMvxPagedViewModel;
                if (currentVM == null)
                    return null;
                IMvxPagedViewModel nextVM = pageVM.GetNextViewModel(currentVM);
                if (nextVM == null)
                    return null;
                UIViewController nextVC = GetViewControllerForViewModel(nextVM);
                return nextVC;
            };
            GetPreviousViewController = delegate (UIPageViewController pc, UIViewController rc)
            {
                IMvxIosView rcTV = rc as IMvxIosView;
                if (rcTV == null)
                    return null;
                IMvxPagedViewModel currentVM = rcTV.ViewModel as IMvxPagedViewModel;
                if (currentVM == null)
                    return null;
                IMvxPagedViewModel prevVM = pageVM.GetPreviousViewModel(currentVM);
                if (prevVM == null)
                    return null;
                UIViewController prevVC = GetViewControllerForViewModel(prevVM);
                return prevVC;
            };
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ViewModel?.ViewCreated();
            InitializePaging();
        }

        public virtual void NavigateToViewModel(IMvxPagedViewModel targetVM, UIPageViewControllerNavigationDirection direction, bool animated = true)
        {
            UIViewController targetVC = GetViewControllerForViewModel(targetVM);
            SetViewControllers(new UIViewController[] { targetVC }, direction, animated, null);
        }

        public virtual UIViewController GetViewControllerForViewModel(IMvxPagedViewModel queryVM)
        {
            UIViewController retVal = null;
            if (_pagedViewControllerCache.ContainsKey(queryVM.PagedViewId))
                retVal = _pagedViewControllerCache[queryVM.PagedViewId];
            else
            {
                retVal = this.CreateViewControllerFor(queryVM) as UIViewController;
                _pagedViewControllerCache[queryVM.PagedViewId] = retVal;
            }
            return retVal;
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
            this.ViewModelRequestForSegue(segue, sender);
        }
    }

    public class MvxPageViewController<TViewModel> : MvxPageViewController, IMvxIosView<TViewModel> where TViewModel : class, IMvxPageViewModel
    {
        public MvxPageViewController()
        {
        }

        public MvxPageViewController(NSCoder coder) : base(coder)
        {
        }

        public MvxPageViewController(UIPageViewControllerTransitionStyle style, UIPageViewControllerNavigationOrientation navigationOrientation) : base(style, navigationOrientation)
        {
        }

        public MvxPageViewController(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
        }

        public MvxPageViewController(UIPageViewControllerTransitionStyle style, UIPageViewControllerNavigationOrientation navigationOrientation, UIPageViewControllerSpineLocation spineLocation) : base(style, navigationOrientation, spineLocation)
        {
        }

        public MvxPageViewController(UIPageViewControllerTransitionStyle style, UIPageViewControllerNavigationOrientation navigationOrientation, NSDictionary options) : base(style, navigationOrientation, options)
        {
        }

        public MvxPageViewController(UIPageViewControllerTransitionStyle style, UIPageViewControllerNavigationOrientation navigationOrientation, UIPageViewControllerSpineLocation spineLocation, float interPageSpacing) : base(style, navigationOrientation, spineLocation, interPageSpacing)
        {
        }

        protected MvxPageViewController(NSObjectFlag t) : base(t)
        {
        }

        protected internal MvxPageViewController(IntPtr handle) : base(handle)
        {
        }

        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }
    }
}
