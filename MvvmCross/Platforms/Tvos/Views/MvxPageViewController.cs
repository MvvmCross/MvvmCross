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

            GetNextViewController = GetNextViewControllerPage;
            GetPreviousViewController = GetPreviousViewControllerPage;
        }

        protected UIPageViewGetViewController GetNextViewControllerPage = delegate (UIPageViewController pc, UIViewController rc)
        {
            return pc.ViewControllers?.ElementAtOrDefault(pc.ViewControllers.ToList().IndexOf(rc) + 1);
        };

        protected UIPageViewGetViewController GetPreviousViewControllerPage = delegate (UIPageViewController pc, UIViewController rc)
        {
            return pc.ViewControllers?.ElementAtOrDefault(pc.ViewControllers.ToList().IndexOf(rc) - 1);
        };

        public void AddPage(UIViewController viewController, MvxPagePresentationAttribute attribute)
        {
            // add Page
            var currentTabs = new List<UIViewController>();
            if (ViewControllers != null)
            {
                currentTabs = ViewControllers.ToList();
            }

            currentTabs.Add(viewController);

            // update current Tabs
            SetViewControllers(currentTabs.ToArray(), UIPageViewControllerNavigationDirection.Forward, true, null);
        }

        public bool RemovePage(IMvxViewModel viewModel)
        {
            if (ViewControllers == null || !ViewControllers.Any())
                return false;

            // loop through plain Tabs
            var plainToClose = ViewControllers.Where(v => !(v is UINavigationController))
                                              .Select(v => v.GetIMvxTvosView())
                                              .FirstOrDefault(mvxView => mvxView.ViewModel == viewModel);
            if (plainToClose != null)
            {
                RemovePageViewController((UIViewController)plainToClose);
                return true;
            }

            return false;
        }

        protected virtual void RemovePageViewController(UIViewController toClose)
        {
            var newPages = ViewControllers.Where(v => v != toClose);
            SetViewControllers(newPages.ToArray(), UIPageViewControllerNavigationDirection.Forward, true, null);
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
    }
}
