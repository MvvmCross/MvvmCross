// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using MvvmCross.Exceptions;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.Platforms.Uap.Presenters.Attributes;
using MvvmCross.Platforms.Uap.Views;
using MvvmCross.ViewModels;
using MvvmCross.Presenters;
using MvvmCross.Views;
using MvvmCross.Presenters.Attributes;
using System.Threading.Tasks;

namespace MvvmCross.Platforms.Uap.Presenters
{
    public class MvxWindowsViewPresenter
        : MvxAttributeViewPresenter, IMvxWindowsViewPresenter
    {
        protected readonly IMvxWindowsFrame _rootFrame;

        public MvxWindowsViewPresenter(IMvxWindowsFrame rootFrame)
        {
            _rootFrame = rootFrame;

            SystemNavigationManager.GetForCurrentView().BackRequested += BackButtonOnBackRequested;
        }

        public override void RegisterAttributeTypes()
        {
            AttributeTypesToActionsDictionary.Add(
                typeof(MvxPagePresentationAttribute),
                new MvxPresentationAttributeAction
                {
                    ShowAction = (view, attribute, request) => ShowPage(view, attribute, request),
                    CloseAction = (viewModel, attribute) => ClosePage(viewModel, attribute)
                });

            AttributeTypesToActionsDictionary.Add(
                typeof(MvxSplitViewPresentationAttribute),
                new MvxPresentationAttributeAction
                {
                    ShowAction = (view, attribute, request) => ShowSplitView(view, (MvxSplitViewPresentationAttribute)attribute, request),
                    CloseAction = (viewModel, attribute) => CloseSplitView(viewModel, (MvxSplitViewPresentationAttribute)attribute)
                });

            AttributeTypesToActionsDictionary.Add(
               typeof(MvxRegionPresentationAttribute),
               new MvxPresentationAttributeAction
               {
                   ShowAction = (view, attribute, request) => ShowRegionView(view, (MvxRegionPresentationAttribute)attribute, request),
                   CloseAction = (viewModel, attribute) => CloseRegionView(viewModel, (MvxRegionPresentationAttribute)attribute)
               });
        }

        public override MvxBasePresentationAttribute CreatePresentationAttribute(Type viewModelType, Type viewType)
        {
            MvxLog.Instance.Trace($"PresentationAttribute not found for {viewType.Name}. Assuming new page presentation");
            return new MvxPagePresentationAttribute() { ViewType = viewType, ViewModelType = viewModelType };
        }

        public override Task<bool> Show(MvxViewModelRequest request)
        {
            return GetPresentationAttributeAction(request, out MvxBasePresentationAttribute attribute).ShowAction.Invoke(attribute.ViewType, attribute, request);
        }

        public override Task<bool> Close(IMvxViewModel viewModel)
        {
            return GetPresentationAttributeAction(new MvxViewModelInstanceRequest(viewModel), out MvxBasePresentationAttribute attribute).CloseAction.Invoke(viewModel, attribute);
        }

        protected virtual async void BackButtonOnBackRequested(object sender, BackRequestedEventArgs backRequestedEventArgs)
        {
            if (backRequestedEventArgs.Handled)
                return;

            var currentView = _rootFrame.Content as IMvxView;
            if (currentView == null)
            {
                MvxLog.Instance.Warn("Ignoring close for viewmodel - rootframe has no current page");
                return;
            }

            var navigationService = Mvx.IoCProvider.Resolve<IMvxNavigationService>();

            backRequestedEventArgs.Handled = await navigationService.Close(currentView.ViewModel);
        }

        protected virtual string GetRequestText(MvxViewModelRequest request)
        {
            var requestTranslator = Mvx.IoCProvider.Resolve<IMvxWindowsViewModelRequestTranslator>();
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

        protected virtual void HandleBackButtonVisibility()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                _rootFrame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }

        protected virtual Task<bool> ShowSplitView(Type viewType, MvxSplitViewPresentationAttribute attribute, MvxViewModelRequest request)
        {
            var viewsContainer = Mvx.IoCProvider.Resolve<IMvxViewsContainer>();

            if (_rootFrame.Content is MvxWindowsPage currentPage)
            {
                var splitView = currentPage.Content.FindControl<SplitView>();
                if (splitView == null)
                {
                    return Task.FromResult(true);
                }

                if (attribute.Position == SplitPanePosition.Content)
                {
                    var nestedFrame = splitView.Content as Frame;
                    if (nestedFrame == null)
                    {
                        nestedFrame = new Frame();
                        splitView.Content = nestedFrame;
                    }
                    var requestText = GetRequestText(request);
                    nestedFrame.Navigate(viewType, requestText);
                }
                else if (attribute.Position == SplitPanePosition.Pane)
                {
                    var nestedFrame = splitView.Pane as Frame;
                    if (nestedFrame == null)
                    {
                        nestedFrame = new Frame();
                        splitView.Pane = nestedFrame;
                    }
                    var requestText = GetRequestText(request);
                    nestedFrame.Navigate(viewType, requestText);
                }
            }
            return Task.FromResult(true);
        }

        protected virtual Task<bool> CloseSplitView(IMvxViewModel viewModel, MvxSplitViewPresentationAttribute attribute)
        {
            return ClosePage(viewModel, attribute);
        }

        protected virtual Task<bool> ShowRegionView(Type viewType, MvxRegionPresentationAttribute attribute, MvxViewModelRequest request)
        {
            if (viewType.HasRegionAttribute())
            {
                var requestText = GetRequestText(request);

                var containerView = _rootFrame.UnderlyingControl.FindControl<Frame>(viewType.GetRegionName());

                if (containerView != null)
                {
                    containerView.Navigate(viewType, requestText);
                    return Task.FromResult(true);
                }
            }
            return Task.FromResult(true);
        }

        protected virtual Task<bool> CloseRegionView(IMvxViewModel viewModel, MvxRegionPresentationAttribute attribute)
        {
            var viewFinder = Mvx.IoCProvider.Resolve<IMvxViewsContainer>();
            var viewType = viewFinder.GetViewType(viewModel.GetType());
            if (viewType.HasRegionAttribute())
            {
                var containerView = _rootFrame.UnderlyingControl?.FindControl<Frame>(viewType.GetRegionName());

                if (containerView == null)
                    throw new MvxException($"Region '{viewType.GetRegionName()}' not found in view '{viewType}'");

                if (containerView.CanGoBack)
                {
                    containerView.GoBack();
                    return Task.FromResult(true);
                }
            }

            return ClosePage(viewModel, attribute);
        }

        protected virtual Task<bool> ClosePage(IMvxViewModel viewModel, MvxBasePresentationAttribute attribute)
        {
            var currentView = _rootFrame.Content as IMvxView;
            if (currentView == null)
            {
                MvxLog.Instance.Warn("Ignoring close for viewmodel - rootframe has no current page");
                return Task.FromResult(false);
            }

            if (currentView.ViewModel != viewModel)
            {
                MvxLog.Instance.Warn("Ignoring close for viewmodel - rootframe's current page is not the view for the requested viewmodel");
                return Task.FromResult(false);
            }

            if (!_rootFrame.CanGoBack)
            {
                MvxLog.Instance.Warn("Ignoring close for viewmodel - rootframe refuses to go back");
                return Task.FromResult(false);
            }

            _rootFrame.GoBack();

            HandleBackButtonVisibility();

            return Task.FromResult(true);
        }

        protected virtual Task<bool> ShowPage(Type viewType, MvxBasePresentationAttribute attribute, MvxViewModelRequest request)
        {
            try
            {
                var requestText = GetRequestText(request);
                var viewsContainer = Mvx.IoCProvider.Resolve<IMvxViewsContainer>();

                _rootFrame.Navigate(viewType, requestText); //Frame won't allow serialization of it's nav-state if it gets a non-simple type as a nav param

                HandleBackButtonVisibility();
                return Task.FromResult(true);
            }
            catch (Exception exception)
            {
                MvxLog.Instance.Trace("Error seen during navigation request to {0} - error {1}", request.ViewModelType.Name,
                    exception.ToLongString());
                return Task.FromResult(false);
            }
        }
    }
}
