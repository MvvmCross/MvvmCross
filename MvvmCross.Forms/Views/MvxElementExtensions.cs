// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using MvvmCross.Exceptions;
using MvvmCross.Forms.Bindings;
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

        private static async ValueTask LoadViewModelForElementAsync(IMvxElement element)
        {
            IMvxViewModel? cached = null;
            if (!MvxDesignTimeChecker.IsDesignTime)
            {
                var cache = Mvx.IoCProvider.Resolve<IMvxChildViewModelCache>();
                cached = cache.Get(element.FindAssociatedViewModelTypeOrNull());
            }

            await element.OnViewCreate(async () =>
                {
                    return cached ?? await element.LoadViewModel().ConfigureAwait(false);
                }
            ).ConfigureAwait(false);
        }

        public static ValueTask OnBindingContextChanged(this IMvxElement element)
        {
            return LoadViewModelForElementAsync(element);
        }

        public static ValueTask OnViewAppearing(this IMvxElement element)
        {
            return LoadViewModelForElementAsync(element);
        }

        private static async ValueTask<IMvxViewModel?> LoadViewModel(this IMvxElement element)
        {
            if (MvxDesignTimeChecker.IsDesignTime)
                return new MvxNullViewModel();

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
            var viewModel = await loader.LoadViewModel(new MvxViewModelRequest(viewModelType), null).ConfigureAwait(false);

            if (viewModel == null)
                throw new MvxException("ViewModel not loaded for " + viewModelType);

            return viewModel;
        }
    }
}
