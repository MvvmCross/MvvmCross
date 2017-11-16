using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Forms.Views.EventSource;
using MvvmCross.Platform;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Platform;
using Xamarin.Forms;

namespace MvvmCross.Forms.Views
{
    public static class MvxElementExtensions
    {
        public static void AdaptForBinding(this IMvxEventSourceElement element)
        {
            var adapter = new MvxElementAdapter(element);
        }

        public static void AdaptForBinding(this IMvxEventSourcePage page)
        {
            var adapter = new MvxPageAdapter(page);
        }

        public static void AdaptForBinding(this IMvxEventSourceCell cell)
        {
            var adapter = new MvxCellAdapter(cell);
        }

        public static void OnBindingContextChanged(this IMvxElement element)
        {
            var cache = Mvx.Resolve<IMvxChildViewModelCache>();
            var cached = cache.Get(element.FindAssociatedViewModelTypeOrNull());

            element.OnViewCreate(() => cached ?? element.LoadViewModel());
        }

        public static void OnViewAppearing(this IMvxElement element)
        {
            var cache = Mvx.Resolve<IMvxChildViewModelCache>();
            var cached = cache.Get(element.FindAssociatedViewModelTypeOrNull());

            element.OnViewCreate(() => cached ?? element.LoadViewModel());
        }

        private static IMvxViewModel LoadViewModel(this IMvxElement element)
        {
            var viewModelType = element.FindAssociatedViewModelTypeOrNull();
            if (viewModelType == typeof(MvxNullViewModel))
                return new MvxNullViewModel();

            if (viewModelType == null
                || viewModelType == typeof(IMvxViewModel))
            {
                MvxTrace.Trace("No ViewModel class specified for {0} in LoadViewModel",
                               element.GetType().Name);
                return null;
            }

            var loader = Mvx.Resolve<IMvxViewModelLoader>();
            var viewModel = loader.LoadViewModel(new MvxViewModelRequest(viewModelType), null);
            if (viewModel == null)
                throw new MvxException("ViewModel not loaded for " + viewModelType);
            return viewModel;
        }
    }
}