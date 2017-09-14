// MvxStoreViewPresenter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Windows.UI.Core;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Platform;

namespace MvvmCross.Uwp.Views
{
    public class MvxWindowsViewPresenter
        : MvxViewPresenter, IMvxWindowsViewPresenter
    {
        protected readonly IMvxWindowsFrame _rootFrame;

        public MvxWindowsViewPresenter(IMvxWindowsFrame rootFrame)
        {
            _rootFrame = rootFrame;

            SystemNavigationManager.GetForCurrentView().BackRequested += BackButtonOnBackRequested;
        }

        protected virtual void BackButtonOnBackRequested(object sender, BackRequestedEventArgs backRequestedEventArgs)
        {
            var currentView = _rootFrame.Content as IMvxView;
            if (currentView == null)
            {
                Mvx.Warning("Ignoring close for viewmodel - rootframe has no current page");
                return;
            }

            var navigationService = Mvx.Resolve<IMvxNavigationService>();
            navigationService.Close(currentView.ViewModel);
        }

        public override void Show(MvxViewModelRequest request)
        {
            try
            {
                var requestText = GetRequestText(request);
                var viewsContainer = Mvx.Resolve<IMvxViewsContainer>();
                var viewType = viewsContainer.GetViewType(request.ViewModelType);

                _rootFrame.Navigate(viewType, requestText); //Frame won't allow serialization of it's nav-state if it gets a non-simple type as a nav param

                HandleBackButtonVisibility();
            }
            catch (Exception exception)
            {
                MvxTrace.Trace("Error seen during navigation request to {0} - error {1}", request.ViewModelType.Name,
                               exception.ToLongString());
            }
        }

        protected virtual string GetRequestText(MvxViewModelRequest request)
        {
            var requestTranslator = Mvx.Resolve<IMvxWindowsViewModelRequestTranslator>();
            string requestText = string.Empty;
            if (request is MvxViewModelInstanceRequest)
            {
                requestText = requestTranslator.GetRequestTextWithKeyFor(((MvxViewModelInstanceRequest)request).ViewModelInstance);
            }
            else
            {
                requestText = requestTranslator.GetRequestTextFor(request);
            }

            return requestText;
        }

        public override void ChangePresentation(MvxPresentationHint hint)
        {
            if (HandlePresentationChange(hint)) return;

            if (hint is MvxClosePresentationHint)
            {
                Close((hint as MvxClosePresentationHint).ViewModelToClose);
                return;
            }

            MvxTrace.Warning("Hint ignored {0}", hint.GetType().Name);
        }

        public override void Close(IMvxViewModel viewModel)
        {
            var currentView = _rootFrame.Content as IMvxView;
            if (currentView == null)
            {
                Mvx.Warning("Ignoring close for viewmodel - rootframe has no current page");
                return;
            }

            if (currentView.ViewModel != viewModel)
            {
                Mvx.Warning("Ignoring close for viewmodel - rootframe's current page is not the view for the requested viewmodel");
                return;
            }

            if (!_rootFrame.CanGoBack)
            {
                Mvx.Warning("Ignoring close for viewmodel - rootframe refuses to go back");
                return;
            }

            _rootFrame.GoBack();

            HandleBackButtonVisibility();
        }

        protected virtual void HandleBackButtonVisibility()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                _rootFrame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }
    }
}