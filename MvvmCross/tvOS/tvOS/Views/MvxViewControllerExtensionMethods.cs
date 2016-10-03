// MvxViewControllerExtensionMethods.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

namespace MvvmCross.tvOS.Views
{
    using MvvmCross.Core.ViewModels;
    using MvvmCross.Core.Views;
    using MvvmCross.Platform;
    using MvvmCross.Platform.Exceptions;
    using MvvmCross.Platform.Platform;

    public static class MvxViewControllerExtensionMethods
    {
        public static void OnViewCreate(this IMvxIosView iosView)
        {
            //var view = iosView as IMvxView<TViewModel>;
            iosView.OnViewCreate(iosView.LoadViewModel);
        }

        private static IMvxViewModel LoadViewModel(this IMvxIosView iosView)
        {
#warning NullViewModel needed?
            // how to do N
            //if (typeof (TViewModel) == typeof (MvxNullViewModel))
            //    return new MvxNullViewModel() as TViewModel;

            if (iosView.Request == null)
            {
                MvxTrace.Trace(
                    "Request is null - assuming this is a TabBar type situation where ViewDidLoad is called during construction... patching the request now - but watch out for problems with virtual calls during construction");
                iosView.Request = Mvx.Resolve<IMvxCurrentRequest>().CurrentRequest;
            }

            var instanceRequest = iosView.Request as MvxViewModelInstanceRequest;
            if (instanceRequest != null)
            {
                return instanceRequest.ViewModelInstance;
            }

            var loader = Mvx.Resolve<IMvxViewModelLoader>();
            var viewModel = loader.LoadViewModel(iosView.Request, null /* no saved state on tvOS currently */);
            if (viewModel == null)
                throw new MvxException("ViewModel not loaded for " + iosView.Request.ViewModelType);
            return viewModel;
        }
    }
}