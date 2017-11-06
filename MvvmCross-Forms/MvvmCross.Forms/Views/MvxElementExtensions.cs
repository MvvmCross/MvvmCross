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
        public static void AdaptForBinding(this IMvxEventSourceElement element, IMvxBindingContextOwner contextOwner)
        {
            if (element is IMvxElement view)
            {
                contextOwner.BindingContext = new MvxBindingContext();
                contextOwner.BindingContext.DataContext = view.ViewModel;
            }
        }

        public static void AdaptForBinding(this IMvxEventSourcePage page)
        {
            var adapter = new MvxPageAdapter(page);
        }

        public static void AdaptForBinding(this IMvxEventSourceCell cell)
        {
            var adapter = new MvxCellAdapter(cell);
        }

        public static void OnViewAppearing(this IMvxElement page)
        {
            var cache = Mvx.Resolve<IMvxChildViewModelCache>();
            var cached = cache.Get(page.FindAssociatedViewModelTypeOrNull());

            page.OnViewCreate(() => cached ?? page.LoadViewModel());
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
            }

            var loader = Mvx.Resolve<IMvxViewModelLoader>();
            var viewModel = loader.LoadViewModel(new MvxViewModelRequest(viewModelType), null);
            if (viewModel == null)
                throw new MvxException("ViewModel not loaded for " + viewModelType);
            return viewModel;
        }
    }
}
