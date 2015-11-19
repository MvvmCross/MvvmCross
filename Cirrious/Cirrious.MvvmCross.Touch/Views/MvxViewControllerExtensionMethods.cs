// MvxViewControllerExtensionMethods.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Touch.Views
{
    public static class MvxViewControllerExtensionMethods
    {
        public static void OnViewCreate(this IMvxTouchView touchView)
        {
            //var view = touchView as IMvxView<TViewModel>;
            touchView.OnViewCreate(touchView.LoadViewModel);
        }

        private static IMvxViewModel LoadViewModel(this IMvxTouchView touchView)
        {
#warning NullViewModel needed?
            // how to do N
            //if (typeof (TViewModel) == typeof (MvxNullViewModel))
            //    return new MvxNullViewModel() as TViewModel;

            if (touchView.Request == null)
            {
                MvxTrace.Trace(
                    "Request is null - assuming this is a TabBar type situation where ViewDidLoad is called during construction... patching the request now - but watch out for problems with virtual calls during construction");
                touchView.Request = Mvx.Resolve<IMvxCurrentRequest>().CurrentRequest;
            }

            var instanceRequest = touchView.Request as MvxViewModelInstanceRequest;
            if (instanceRequest != null)
            {
                return instanceRequest.ViewModelInstance;
            }

            var loader = Mvx.Resolve<IMvxViewModelLoader>();
            var viewModel = loader.LoadViewModel(touchView.Request, null /* no saved state on iOS currently */);
            if (viewModel == null)
                throw new MvxException("ViewModel not loaded for " + touchView.Request.ViewModelType);
            return viewModel;
        }
    }
}