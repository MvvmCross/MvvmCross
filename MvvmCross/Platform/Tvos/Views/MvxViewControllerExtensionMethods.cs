// MvxViewControllerExtensionMethods.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Logging;
using MvvmCross.Platform.Platform;

namespace MvvmCross.tvOS.Views
{
    public static class MvxViewControllerExtensionMethods
    {
        public static void OnViewCreate(this IMvxTvosView tvOSView)
        {
            //var view = tvOSView as IMvxView<TViewModel>;
            tvOSView.OnViewCreate(tvOSView.LoadViewModel);
        }

        private static IMvxViewModel LoadViewModel(this IMvxTvosView tvOSView)
        {
            if(tvOSView.Request == null)
            {
                MvxLog.Instance.Trace(
                    "Request is null - assuming this is a TabBar type situation where ViewDidLoad is called during construction... patching the request now - but watch out for problems with virtual calls during construction");
                tvOSView.Request = Mvx.Resolve<IMvxCurrentRequest>().CurrentRequest;
            }

            var instanceRequest = tvOSView.Request as MvxViewModelInstanceRequest;
            if(instanceRequest != null)
            {
                return instanceRequest.ViewModelInstance;
            }

            var loader = Mvx.Resolve<IMvxViewModelLoader>();
            var viewModel = loader.LoadViewModel(tvOSView.Request, null /* no saved state on tvOS currently */);
            if(viewModel == null)
                throw new MvxException("ViewModel not loaded for " + tvOSView.Request.ViewModelType);
            return viewModel;
        }
    }
}
