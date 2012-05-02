#region Copyright
// <copyright file="MvxTouchViewControllerExtensionMethods.cs" company="Cirrious">
// (c) Copyright Cirrious. http://www.cirrious.com
// This source is subject to the Microsoft Public License (Ms-PL)
// Please see license.txt on http://opensource.org/licenses/ms-pl.html
// All other rights reserved.
// </copyright>
// 
// Project Lead - Stuart Lodge, Cirrious. http://www.cirrious.com
#endregion

using System.Collections.Generic;
using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Touch.ExtensionMethods
{
    public static class MvxTouchViewControllerExtensionMethods
    {
        public static void OnViewCreate<TViewModel>(this IMvxTouchView<TViewModel> touchView)
            where TViewModel : class, IMvxViewModel
        {
            var view = touchView as IMvxView<TViewModel>;
            view.OnViewCreate(() => { return touchView.LoadViewModel(); });
        }

        private static TViewModel LoadViewModel<TViewModel>(this IMvxTouchView<TViewModel> touchView)
            where TViewModel : class, IMvxViewModel
        {
            if (typeof(TViewModel) == typeof(MvxNullViewModel))
                return new MvxNullViewModel() as TViewModel;

            var loader = touchView.GetService<IMvxViewModelLoader>();
            var viewModel = loader.LoadViewModel(touchView.ShowRequest);
            if (viewModel == null)
                throw new MvxException("ViewModel not loaded for " + touchView.ShowRequest.ViewModelType);
            return (TViewModel) viewModel;
        }

        public static IMvxTouchView CreateViewControllerFor<TTargetViewModel>(this IMvxTouchView view, object parameterObject)
            where TTargetViewModel : class, IMvxViewModel
        {
            return view.CreateViewControllerFor<TTargetViewModel>(parameterObject== null ? null : parameterObject.ToSimplePropertyDictionary());
        }

        public static IMvxTouchView CreateViewControllerFor<TTargetViewModel>(
            this IMvxTouchView view,
            IDictionary<string, string> parameterValues = null)
            where TTargetViewModel : class, IMvxViewModel
        {
            parameterValues = parameterValues ?? new Dictionary<string, string>();
            var request = new MvxShowViewModelRequest<TTargetViewModel>(parameterValues, false,
                                                                        MvxRequestedBy.UserAction);
            return view.CreateViewControllerFor<TTargetViewModel>(request);
        }

        public static IMvxTouchView CreateViewControllerFor<TTargetViewModel>(
            this IMvxTouchView view,
            MvxShowViewModelRequest<TTargetViewModel> request)
            where TTargetViewModel : class, IMvxViewModel
        {
            return MvxServiceProviderExtensions.GetService<IMvxTouchViewCreator>().CreateView(request);
        }
		
        public static IMvxTouchView CreateViewControllerFor(
            this IMvxTouchView view,
            MvxShowViewModelRequest request)
        {
            return MvxServiceProviderExtensions.GetService<IMvxTouchViewCreator>().CreateView(request);
        }

        public static IMvxTouchView CreateViewControllerFor(
            this IMvxTouchView view,
            IMvxViewModel viewModel)
        {
            return MvxServiceProviderExtensions.GetService<IMvxTouchViewCreator>().CreateView(viewModel);
        }
    }
}