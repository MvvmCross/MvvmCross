﻿// MvxPageViewExtensionMethods.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.iOS.Views
{
    using MvvmCross.Binding.iOS.Views;
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Platform;
    using System;
    using System.Collections.Generic;
    using UIKit;
    using MvvmCross.Core.Platform;

    public static class MvxPageViewExtensionMethods
    {

        public static UIViewController CreateViewController<T>(this MvxPageViewSource self, object parameterValuesObject, int pageIndex = -1)
        {
            return CreateViewController<T>(self, parameterValuesObject.ToSimplePropertyDictionary(), pageIndex);
        }

        public static UIViewController CreateViewController<T>(this MvxPageViewSource self, IDictionary<string, string> parameterValues, int pageIndex = -1)
        {
            return CreateViewController<T>(self, new MvxBundle(parameterValues), pageIndex);
        }

        public static UIViewController CreateViewController<T>(this MvxPageViewSource self, IMvxBundle parameterBundle, int pageIndex = -1)
        {
            return CreateViewControllerImpl(self, typeof(T), parameterBundle, pageIndex);
        }

        private static UIViewController CreateViewControllerImpl(this MvxPageViewSource self, Type viewModelType, IMvxBundle parameterBundle, int pageIndex)
        {
            var request = new MvxViewModelRequest(viewModelType, parameterBundle, null);
            var viewController = (self.PageView as IMvxIosView)?.CreateViewControllerFor(request) as UIViewController;

            if (pageIndex >= 0)
                SetPageIndexForController(viewController, pageIndex);
            return viewController;
        }

        private static void SetPageIndexForController(UIViewController referenceViewController, int index) {
            var mvxPageView = referenceViewController as IMvxPageViewController;
            if (mvxPageView != null)
            {
                var prop = mvxPageView.GetType().GetProperty("PageIndex", BindingFlags.Public | BindingFlags.Instance);
                if (prop?.GetSetMethod() != null)
                    prop.SetValue(mvxPageView, index);
            }
        }
    }
}
