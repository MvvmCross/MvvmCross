// MvxStoreExtensionMethods.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Linq;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross.WindowsUWP.Views
{
    public static class MvxWindowsExtensionMethods
    {
        public static void OnViewCreate(this IMvxWindowsView storeView, MvxViewModelRequest viewModelRequest, Func<IMvxBundle> bundleLoader)
        {
            storeView.OnViewCreate(() => { return storeView.LoadViewModel(viewModelRequest, bundleLoader()); });
        }

        private static IMvxViewModel LoadViewModel(this IMvxWindowsView storeView,
                                                   MvxViewModelRequest viewModelRequest,
                                                   IMvxBundle bundle)
        {
#warning ClearingBackStack disabled for now
//            if (viewModelRequest.ClearTop)
//            {
//#warning TODO - BackStack not cleared for WinRT
                //phoneView.ClearBackStack();
//            }

            var loaderService = Mvx.Resolve<IMvxViewModelLoader>();
            var viewModel = loaderService.LoadViewModel(viewModelRequest, bundle);

            return viewModel;
        }

        public static void OnViewCreate(this IMvxWindowsView storeView, Func<IMvxViewModel> viewModelLoader)
        {
            if (storeView.ViewModel != null)
                return;

            var viewModel = viewModelLoader();
            storeView.ViewModel = viewModel;
        }

        public static void OnViewDestroy(this IMvxWindowsView storeView)
        {
            // nothing to do currently
        }

        public static bool HasRegionAttribute(this Type view)
        {
            var attributes = view
                .GetCustomAttributes(typeof(MvxRegionAttribute), true);

            return attributes.Any();
        }

        public static string GetRegionName(this Type view)
        {
            var attributes = view
                .GetCustomAttributes(typeof(MvxRegionAttribute), true);

            if (!attributes.Any())
                throw new InvalidOperationException("The IMvxWindowsView has no region attribute.");

            return ((MvxRegionAttribute)attributes.First()).Name;
        }
    }
}