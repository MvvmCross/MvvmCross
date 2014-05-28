using System;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross.Silverlight.Views
{
    public static class MvxSilverlightExtensionMethods
    {
        public static void OnViewCreate(this IMvxSilverlightView view, Uri navigationUri, IMvxBundle savedStateBundle)
        {
            view.OnViewCreate(() => view.LoadViewModel(navigationUri, savedStateBundle));
        }

        private static IMvxViewModel LoadViewModel(this IMvxSilverlightView view,
                                                   Uri navigationUri,
                                                   IMvxBundle savedStateBundle)
        {
            var translatorService = Mvx.Resolve<IMvxSilverlightViewModelRequestTranslator>();
            var viewModelRequest = translatorService.GetRequestFromXamlUri(navigationUri);


            var loaderService = Mvx.Resolve<IMvxViewModelLoader>();
            var viewModel = loaderService.LoadViewModel(viewModelRequest, savedStateBundle);

            return viewModel;
        }

        public static void OnViewCreate(this IMvxSilverlightView silverlightView, Func<IMvxViewModel> viewModelLoader)
        {
            if (silverlightView.ViewModel != null)
                return;

            var viewModel = viewModelLoader();
            silverlightView.ViewModel = viewModel;
        }

        public static void OnViewDestroy(this IMvxSilverlightView silverlightView)
        {
            // nothing to do currently
        }
    }
}