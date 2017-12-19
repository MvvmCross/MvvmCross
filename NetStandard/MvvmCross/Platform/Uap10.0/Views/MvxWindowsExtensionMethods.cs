﻿// MvxStoreExtensionMethods.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Uwp.Attributes;
using System;
using System.Linq;
using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace MvvmCross.Uwp.Views
{
    public static class MvxWindowsExtensionMethods
    {
        public static void OnViewCreate(this IMvxWindowsView storeView, string requestText, Func<IMvxBundle> bundleLoader)
        {
            storeView.OnViewCreate(() => { return storeView.LoadViewModel(requestText, bundleLoader()); });
        }

        public static void OnViewCreate(this IMvxWindowsView storeView, Func<IMvxViewModel> viewModelLoader)
        {
            if (storeView.ViewModel != null)
                return;

            var viewModel = viewModelLoader();
            storeView.ViewModel = viewModel;
        }

        public static void OnViewDestroy(this IMvxWindowsView storeView, int key)
        {
            if (key > 0)
            {
                var viewModelLoader = Mvx.Resolve<IMvxWindowsViewModelRequestTranslator>();
                viewModelLoader.RemoveSubViewModelWithKey(key);
            }
        }

        public static bool HasRegionAttribute(this Type view)
        {
            var attributes = view
                .GetCustomAttributes(typeof(MvxRegionPresentationAttribute), true);

            return attributes.Any();
        }

        public static string GetRegionName(this Type view)
        {
            var attributes = view
                .GetCustomAttributes(typeof(MvxRegionPresentationAttribute), true);

            if (!attributes.Any())
                throw new InvalidOperationException("The IMvxWindowsView has no region attribute.");

            return ((MvxRegionPresentationAttribute)attributes.First()).Name;
        }

        public static T FindControl<T>(this UIElement parent, string name = null) where T : FrameworkElement
        {
            if (parent == null)
            {
                return null;
            }

            if (parent is T typedParent &&
                (string.IsNullOrWhiteSpace(name) || parent.GetValue(FrameworkElement.NameProperty).Equals(name)))
            {
                return typedParent;
            }

            T result = null;
            var count = VisualTreeHelper.GetChildrenCount(parent);
            for (var i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i) as UIElement;

                result = FindControl<T>(child);
                if (result != null)
                {
                    return result;
                }
            }

            return result;
        }

        private static IMvxViewModel LoadViewModel(this IMvxWindowsView storeView,
                                                    string requestText,
                                                    IMvxBundle bundle)
        {
#warning ClearingBackStack disabled for now

            //            if (viewModelRequest.ClearTop)
            //            {
            //#warning TODO - BackStack not cleared for WinRT
            //phoneView.ClearBackStack();
            //            }
            var viewModelLoader = Mvx.Resolve<IMvxWindowsViewModelLoader>();
            return viewModelLoader.Load(requestText, bundle);
        }
    }
}
