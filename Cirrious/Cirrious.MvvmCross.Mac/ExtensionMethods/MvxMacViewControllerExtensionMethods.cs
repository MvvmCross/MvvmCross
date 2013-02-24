// MvxMacViewControllerExtensionMethods.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using Cirrious.CrossCore.Interfaces.IoC;
using Cirrious.MvvmCross.Interfaces.ViewModels;
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

        public static IMvxMacView CreateViewControllerFor<TTargetViewModel>(this IMvxMacView view,
                                                                            object parameterObject)
            where TTargetViewModel : class, IMvxViewModel
        {
            return
                view.CreateViewControllerFor<TTargetViewModel>(parameterObject == null
                                                                   ? null
                                                                   : parameterObject.ToSimplePropertyDictionary());
        }

        public static IMvxMacView CreateViewControllerFor<TTargetViewModel>(
            this IMvxMacView view,
            IDictionary<string, string> parameterValues = null)
            where TTargetViewModel : class, IMvxViewModel
        {
            parameterValues = parameterValues ?? new Dictionary<string, string>();
            var request = new MvxShowViewModelRequest<TTargetViewModel>(parameterValues, false,
                                                                        MvxRequestedBy.UserAction);
            var viewController = view.CreateViewControllerFor(request);
            return viewController;
        }

        public static IMvxMacView CreateViewControllerFor<TTargetViewModel>(
            this IMvxMacView view,
            MvxShowViewModelRequest<TTargetViewModel> request)
            where TTargetViewModel : class, IMvxViewModel
        {
            return MvxIoCExtensions.Resolve<IMvxMacViewCreator>().CreateView(request);
        }

        public static IMvxMacView CreateViewControllerFor(
            this IMvxMacView view,
            MvxShowViewModelRequest request)
        {
            return MvxIoCExtensions.Resolve<IMvxMacViewCreator>().CreateView(request);
        }

        public static IMvxMacView CreateViewControllerFor(
            this IMvxMacView view,
            IMvxViewModel viewModel)
        {
            return MvxIoCExtensions.Resolve<IMvxMacViewCreator>().CreateView(viewModel);
        }
    }
}