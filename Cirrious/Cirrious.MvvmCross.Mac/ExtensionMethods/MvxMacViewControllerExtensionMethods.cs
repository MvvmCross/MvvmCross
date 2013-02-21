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
using Cirrious.CrossCore.Interfaces.ServiceProvider;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Touch.Interfaces;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Touch.ExtensionMethods
{
    public static class MvxMacViewControllerExtensionMethods
    {
        public static void OnViewCreate(this IMvxMacView view)
        {
            view.OnViewCreate(() => { return view.LoadViewModel(); });
        }

        public static IMvxMacView CreateViewControllerFor<TTargetViewModel>(this IMvxMacView view, object parameterObject)
            where TTargetViewModel : class, IMvxViewModel
        {
            return view.CreateViewControllerFor<TTargetViewModel>(parameterObject== null ? null : parameterObject.ToSimplePropertyDictionary());
        }

        public static IMvxMacView CreateViewControllerFor<TTargetViewModel>(
            this IMvxMacView view,
            IDictionary<string, string> parameterValues = null)
            where TTargetViewModel : class, IMvxViewModel
        {
            parameterValues = parameterValues ?? new Dictionary<string, string>();
            var request = new MvxShowViewModelRequest<TTargetViewModel>(parameterValues, false,
                                                                        MvxRequestedBy.UserAction);
			var viewController = view.CreateViewControllerFor<TTargetViewModel>(request);
			return viewController;
        }

        public static IMvxMacView CreateViewControllerFor<TTargetViewModel>(
            this IMvxMacView view,
            MvxShowViewModelRequest<TTargetViewModel> request)
            where TTargetViewModel : class, IMvxViewModel
        {
            return MvxServiceProviderExtensions.GetService<IMvxMacViewCreator>().CreateView(request);
        }
		
        public static IMvxMacView CreateViewControllerFor(
            this IMvxMacView view,
            MvxShowViewModelRequest request)
        {
            return MvxServiceProviderExtensions.GetService<IMvxMacViewCreator>().CreateView(request);
        }

        public static IMvxMacView CreateViewControllerFor(
            this IMvxMacView view,
            IMvxViewModel viewModel)
        {
            return MvxServiceProviderExtensions.GetService<IMvxMacViewCreator>().CreateView(viewModel);
        }
    }
}