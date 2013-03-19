// MvxViewControllerExtensionMethods.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Platform.Diagnostics;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Touch.Views
{
    public static class MvxViewControllerExtensionMethods
    {
        public static void OnViewCreate(this IMvxTouchView touchView)
        {
            //var view = touchView as IMvxView<TViewModel>;
            touchView.OnViewCreate(() => { return touchView.LoadViewModel(); });
        }

        private static IMvxViewModel LoadViewModel(this IMvxTouchView touchView)
        {
#warning NullViewModel needed?
            // how to do N
            //if (typeof (TViewModel) == typeof (MvxNullViewModel))
            //    return new MvxNullViewModel() as TViewModel;

            if (touchView.ShowRequest == null)
            {
                MvxTrace.Trace(
                    "ShowRequest is null - assuming this is a TabBar type situation where ViewDidLoad is called during construction... patching the request now - but watch out for problems with virtual calls during construction");
                touchView.ShowRequest = Mvx.Resolve<IMvxCurrentRequest>().CurrentRequest;
            }

            var instanceRequest = touchView.ShowRequest as MvxShowViewModelInstaceRequest;
            if (instanceRequest != null)
            {
                return instanceRequest.ViewModelInstance;
            }

            var loader = Mvx.Resolve<IMvxViewModelLoader>();
            var viewModel = loader.LoadViewModel(touchView.ShowRequest, null /* no saved state on iOS currently */);
            if (viewModel == null)
                throw new MvxException("ViewModel not loaded for " + touchView.ShowRequest.ViewModelType);
            return viewModel;
        }

        public static IMvxTouchView CreateViewControllerFor<TTargetViewModel>(this IMvxTouchView view,
                                                                              object parameterObject)
            where TTargetViewModel : class, IMvxViewModel
        {
            return
                view.CreateViewControllerFor<TTargetViewModel>(parameterObject == null
                                                                   ? null
                                                                   : parameterObject.ToSimplePropertyDictionary());
        }

        public static IMvxTouchView CreateViewControllerFor<TTargetViewModel>(
            this IMvxTouchView view,
            IDictionary<string, string> parameterValues = null)
            where TTargetViewModel : class, IMvxViewModel
        {
            parameterValues = parameterValues ?? new Dictionary<string, string>();
            var request = new MvxShowViewModelRequest<TTargetViewModel>(parameterValues, false,
                                                                        MvxRequestedBy.UserAction);
            return view.CreateViewControllerFor(request);
        }

        public static IMvxTouchView CreateViewControllerFor<TTargetViewModel>(
            this IMvxTouchView view,
            MvxShowViewModelRequest<TTargetViewModel> request)
            where TTargetViewModel : class, IMvxViewModel
        {
            return Mvx.Resolve<IMvxTouchViewCreator>().CreateView(request);
        }

        public static IMvxTouchView CreateViewControllerFor(
            this IMvxTouchView view,
            MvxShowViewModelRequest request)
        {
            return Mvx.Resolve<IMvxTouchViewCreator>().CreateView(request);
        }

        public static IMvxTouchView CreateViewControllerFor(
            this IMvxTouchView view,
            IMvxViewModel viewModel)
        {
            return Mvx.Resolve<IMvxTouchViewCreator>().CreateView(viewModel);
        }
    }
}