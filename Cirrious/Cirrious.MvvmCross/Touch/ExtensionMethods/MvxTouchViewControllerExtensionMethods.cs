#region Copyright

// <copyright file="MvxTouchViewControllerExtensionMethods.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Author - Stuart Lodge, Cirrious. http://www.cirrious.com

#endregion

using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Views;

#warning DIE
/*
namespace Cirrious.MvvmCross.Touch.ExtensionMethods
{
    internal static class MvxTouchViewControllerExtensionMethods
    {
        public static void OnViewDidLoad<TViewModel>(this IMvxTouchView<TViewModel> touchView)
            where TViewModel : class, IMvxViewModel
        {
            if (touchView.ViewModel != null)
                return;
			
			if (touchView.Role != MvxTouchViewRole.TopLevelView)
				return;
			
            var viewModel = GetViewModelForTopLevelView(touchView);
            touchView.SetViewModel(viewModel);
			GetViewModelForTopLevelView(touchView);
        }

        private static IMvxViewModel GetViewModelForTopLevelView<TViewModel>(IMvxTouchView<TViewModel> touchView)
            where TViewModel : class, IMvxViewModel
        {
            var activity = touchView.ToActivity();
            var translatorService = touchView.GetService<IMvxtouchViewModelRequestTranslator>();
            var viewModelRequest = translatorService.GetRequestFromIntent(activity.Intent);

            if (viewModelRequest == null)
                viewModelRequest = MvxShowViewModelRequest<TViewModel>.GetDefaultRequest();

            var loaderService = touchView.GetService<IMvxViewModelLoader>();
            var viewModel = loaderService.LoadModel(viewModelRequest);
            return viewModel;
        }

        private static IMvxViewModel GetViewModelForSubView<TViewModel>(IMvxTouchView<TViewModel> touchView)
            where TViewModel : class, IMvxViewModel
        {
            var activityServices = touchView.GetService<IMvxAndroidSubViewServices>();
            var viewModel = activityServices.CurrentTopLevelViewModel;
            return viewModel;
        }
    }
}

*/