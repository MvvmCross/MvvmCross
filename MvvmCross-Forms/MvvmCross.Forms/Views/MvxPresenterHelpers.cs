// MvxPresenterHelpers.cs
// 2015 (c) Copyright Cheesebaron. http://ostebaronen.dk
// MvvmCross.Forms is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Tomasz Cielecki, @cheesebaron, mvxplugins@ostebaronen.dk
// Contributor - Marcos Cobeña Morián, @CobenaMarcos, marcoscm@me.com

using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Forms.Platform;
using MvvmCross.Platform;
using Xamarin.Forms;

namespace MvvmCross.Forms.Views
{
    public static class MvxPresenterHelpers
    {
        public static IMvxViewModel LoadViewModel(MvxViewModelRequest request)
        {
            if(request is MvxViewModelInstanceRequest instanceRequest)
                return instanceRequest.ViewModelInstance;
            
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