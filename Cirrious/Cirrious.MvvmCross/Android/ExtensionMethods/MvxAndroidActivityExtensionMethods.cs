#region Copyright

// <copyright file="MvxAndroidActivityExtensionMethods.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using Android.App;
using Cirrious.MvvmCross.Android.Interfaces;
using Cirrious.MvvmCross.Android.Views;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ViewModel;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Android.ExtensionMethods
{
    internal static class MvxAndroidActivityExtensionMethods
    {
        public static void OnViewCreate<TViewModel>(this IMvxAndroidView<TViewModel> androidView)
            where TViewModel : class, IMvxViewModel
        {
            if (androidView.ViewModel != null)
                return;

            var viewModel = GetViewModel(androidView);
            UpdateActivityTracker(androidView, viewModel);
            androidView.SetViewModel(viewModel);
        }

        private static void UpdateActivityTracker<TViewModel>(IMvxAndroidView<TViewModel> androidView,
                                                              IMvxViewModel viewModel)
            where TViewModel : class, IMvxViewModel
        {
            var activityTracker = androidView.GetService<IMvxAndroidActivityTracker>();
            var activity = androidView.ToActivity();
            switch (androidView.Role)
            {
                case MvxAndroidViewRole.TopLevelView:
                    activityTracker.OnTopLevelAndroidActivity(activity, viewModel);
                    break;
                case MvxAndroidViewRole.SubView:
                    activityTracker.OnSubViewAndroidActivity(activity);
                    break;
                default:
                    throw new MvxException("What on earth is a view with role {0}", androidView.Role);
            }
        }

        public static Activity ToActivity<TViewModel>(this IMvxAndroidView<TViewModel> androidView)
            where TViewModel : class, IMvxViewModel
        {
            var activity = androidView as Activity;
            if (activity == null)
                throw new MvxException("OnViewCreate called from an IMvxView which is not an Android Activity");
            return activity;
        }

        private static IMvxViewModel GetViewModel<TViewModel>(IMvxAndroidView<TViewModel> androidView)
            where TViewModel : class, IMvxViewModel
        {
            IMvxViewModel viewModel;
            switch (androidView.Role)
            {
                case MvxAndroidViewRole.TopLevelView:
                    viewModel = GetViewModelForTopLevelView(androidView);
                    break;
                case MvxAndroidViewRole.SubView:
                    viewModel = GetViewModelForSubView(androidView);
                    break;
                default:
                    throw new MvxException("What on earth is a view with role {0}", androidView.Role);
            }
            return viewModel;
        }

        private static IMvxViewModel GetViewModelForTopLevelView<TViewModel>(IMvxAndroidView<TViewModel> androidView)
            where TViewModel : class, IMvxViewModel
        {
            var activity = androidView.ToActivity();
            var translatorService = androidView.GetService<IMvxAndroidViewModelRequestTranslator>();
            var viewModelRequest = translatorService.GetRequestFromIntent(activity.Intent);

            if (viewModelRequest == null)
                viewModelRequest = MvxShowViewModelRequest<TViewModel>.GetDefaultRequest();

            var loaderService = androidView.GetService<IMvxViewModelLoader>();
            var viewModel = loaderService.LoadModel(viewModelRequest);
            return viewModel;
        }

        private static IMvxViewModel GetViewModelForSubView<TViewModel>(IMvxAndroidView<TViewModel> androidView)
            where TViewModel : class, IMvxViewModel
        {
            var activityServices = androidView.GetService<IMvxAndroidSubViewServices>();
            var viewModel = activityServices.CurrentTopLevelViewModel;
            return viewModel;
        }
    }
}