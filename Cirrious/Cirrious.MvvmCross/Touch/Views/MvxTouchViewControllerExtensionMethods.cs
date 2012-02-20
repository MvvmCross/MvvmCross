using Cirrious.MvvmCross.Exceptions;
using Cirrious.MvvmCross.ExtensionMethods;
using Cirrious.MvvmCross.Interfaces.ViewModels;
using Cirrious.MvvmCross.Interfaces.Views;
using Cirrious.MvvmCross.Touch.Interfaces;

namespace Cirrious.MvvmCross.Touch.Views
{
    public static class MvxTouchViewControllerExtensionMethods
    {
        public static void OnViewCreate<TViewModel>(this IMvxTouchView<TViewModel> touchView)
            where TViewModel : class, IMvxViewModel
        {
            var view = touchView as IMvxTrackedView<TViewModel>;
            view.OnViewCreate(() => { return touchView.LoadViewModel(); });
        }

        private static TViewModel LoadViewModel<TViewModel>(this IMvxTouchView<TViewModel> touchView)
            where TViewModel : class, IMvxViewModel
        {
            var loader = touchView.GetService<IMvxViewModelLoader>();
            var viewModel = loader.LoadModel(touchView.ShowRequest);
            if (viewModel == null)
                throw new MvxException("ViewModel not loaded for " + touchView.ShowRequest.ViewModelType);
            return (TViewModel) viewModel;
        }
    }
}