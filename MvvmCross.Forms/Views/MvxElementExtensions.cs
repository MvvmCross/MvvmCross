// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using MvvmCross.Exceptions;
using MvvmCross.Forms.Views.Base;
using MvvmCross.Logging;
using MvvmCross.ViewModels;
using MvvmCross.Views;

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
            var cache = Mvx.IoCProvider.Resolve<IMvxChildViewModelCache>();
            var cached = cache.Get(element.FindAssociatedViewModelTypeOrNull());

            element.OnViewCreate(() => cached ?? element.LoadViewModel());
        }

        public static void OnViewAppearing(this IMvxElement element)
        {
            var cache = Mvx.IoCProvider.Resolve<IMvxChildViewModelCache>();
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
                MvxFormsLog.Instance.Trace("No ViewModel class specified for {0} in LoadViewModel",
                               element.GetType().Name);
                return null;
            }

            var loader = Mvx.IoCProvider.Resolve<IMvxViewModelLoader>();
            var viewModel = loader.LoadViewModel(new MvxViewModelRequest(viewModelType), null);
            if (viewModel == null)
                throw new MvxException("ViewModel not loaded for " + viewModelType);
            return viewModel;
        }
    }
}
