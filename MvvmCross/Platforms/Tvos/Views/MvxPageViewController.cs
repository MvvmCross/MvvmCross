// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Tvos.Presenters.Attributes;
using MvvmCross.ViewModels;
using UIKit;

namespace MvvmCross.Platforms.Tvos.Views
{
    public class MvxPageViewController : MvxBasePageViewController, IMvxPageViewController
    {
        public MvxPageViewController(UIPageViewControllerTransitionStyle style = UIPageViewControllerTransitionStyle.Scroll, UIPageViewControllerNavigationOrientation navigationOrientation = UIPageViewControllerNavigationOrientation.Horizontal, UIPageViewControllerSpineLocation spineLocation = UIPageViewControllerSpineLocation.None) : base(style, navigationOrientation, spineLocation)
        {
        }

        public MvxPageViewController(UIPageViewControllerTransitionStyle style, UIPageViewControllerNavigationOrientation navigationOrientation, UIPageViewControllerSpineLocation spineLocation, float interPageSpacing) : base(style, navigationOrientation, spineLocation, interPageSpacing)
        {
        }

        public MvxPageViewController(UIPageViewControllerTransitionStyle style, UIPageViewControllerNavigationOrientation navigationOrientation) : base(style, navigationOrientation)
        {
        }

        public MvxPageViewController(NSCoder coder) : base(coder)
        {
        }

        protected MvxPageViewController(NSObjectFlag t) : base(t)
        {
        }

        protected internal MvxPageViewController(IntPtr handle) : base(handle)
        {
        }

        public MvxPageViewController(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
        }

        public MvxPageViewController(UIPageViewControllerTransitionStyle style, UIPageViewControllerNavigationOrientation navigationOrientation, NSDictionary options) : base(style, navigationOrientation, options)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            GetNextViewController = (pc, rc) => GetNextViewControllerPage(rc);
            GetPreviousViewController = (pc, rc) => GetPreviousViewControllerPage(rc);
        }

        private List<UIViewController> Pages = new List<UIViewController>();

        public bool IsFirstPage(UIViewController viewController) => Pages.IndexOf(viewController) == 0;

        public bool IsLastPage(UIViewController viewController) => Pages.IndexOf(viewController) == Pages.Count - 1;

        protected UIViewController GetNextViewControllerPage(UIViewController rc) => IsLastPage(rc) ? null : Pages[Pages.IndexOf(rc) + 1];

        protected UIViewController GetPreviousViewControllerPage(UIViewController rc) => IsFirstPage(rc) ? null : Pages[Pages.IndexOf(rc) - 1];

        public void AddPage(UIViewController viewController, MvxPagePresentationAttribute attribute)
        {
            // add Page
            Pages.Add(viewController);

            // Start the ui page view controller when we add the first page
            if (Pages.Count == 1)
            {
                SetViewControllers(Pages.ToArray(), UIPageViewControllerNavigationDirection.Forward, true, null);
            }
        }

        public bool RemovePage(IMvxViewModel viewModel)
        {
            if (Pages == null || !Pages.Any())
                return false;

            var pageToClose = Pages.Where(v => !(v is UINavigationController))
                                              .Select(v => v.GetIMvxTvosView())
                                              .FirstOrDefault(mvxView => mvxView.ViewModel == viewModel);

            if (pageToClose != null)
            {
                Pages = Pages.Where(v => v != pageToClose).ToList();
                return true;
            }

            return false;
        }
    }

    public class MvxPageViewController<TViewModel> : MvxPageViewController, IMvxTvosView<TViewModel> where TViewModel : class, IMvxViewModel
    {
        public MvxPageViewController(UIPageViewControllerTransitionStyle style = UIPageViewControllerTransitionStyle.Scroll, UIPageViewControllerNavigationOrientation orientation = UIPageViewControllerNavigationOrientation.Horizontal, UIPageViewControllerSpineLocation spine = UIPageViewControllerSpineLocation.None) : base(style, orientation, spine)
        {
        }

        public MvxPageViewController(IntPtr handle) : base(handle)
        {
        }

        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public MvxFluentBindingDescriptionSet<IMvxTvosView<TViewModel>, TViewModel> CreateBindingSet()
        {
            return this.CreateBindingSet<IMvxTvosView<TViewModel>, TViewModel>();
        }
    }
}
