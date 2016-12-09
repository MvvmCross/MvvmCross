// MvxPresenterHelpers.cs
// 2015 (c) Copyright Cheesebaron. http://ostebaronen.dk
// MvvmCross.Forms.Presenter is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Tomasz Cielecki, @cheesebaron, mvxplugins@ostebaronen.dk
// Contributor - Marcos Cobeña Morián, @CobenaMarcos, marcoscm@me.com

using MvvmCross.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Binding.BindingContext;
using Xamarin.Forms;

namespace MvvmCross.Forms.Presenter.Core
{
    public static class MvxPresenterHelpers
    {
        public static IMvxViewModel LoadViewModel(MvxViewModelRequest request)
        {
            var viewModelLoader = Mvx.Resolve<IMvxViewModelLoader>();
            var viewModel = viewModelLoader.LoadViewModel(request, null);
            return viewModel;
        }

        public static Page CreatePage(MvxViewModelRequest request)
        {
            IMvxFormsPageLoader viewPageLoader;
            Mvx.TryResolve(out viewPageLoader);
            if (viewPageLoader == null)
            {
                // load default instead
                viewPageLoader = new MvxFormsPageLoader();
                Mvx.RegisterSingleton(viewPageLoader);
            }
            var page = viewPageLoader.LoadPage(request);
            return page;
        }

        public static void AdaptForBinding(VisualElement element, IMvxBindingContextOwner contextOwner)
        {
            var mvxPage = element as IMvxContentPage;
            if (mvxPage != null) {
                contextOwner.BindingContext = new MvxBindingContext();
                contextOwner.BindingContext.DataContext = mvxPage.ViewModel;
            }
        }

    }
}