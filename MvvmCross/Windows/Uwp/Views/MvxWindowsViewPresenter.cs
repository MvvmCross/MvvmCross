// MvxStoreViewPresenter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
//
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Platform;
using MvvmCross.Uwp.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MvvmCross.Uwp.Views
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
            MvxTrace.Trace($"PresentationAttribute not found for {viewType.Name}. Assuming new page presentation");
            return new MvxPagePresentationAttribute();
        }

        public override MvxBasePresentationAttribute GetOverridePresentationAttribute(Type viewModelType, Type viewType)
        {
            if (viewType != null && viewType.GetInterfaces().Contains(typeof(IMvxOverridePresentationAttribute)))
            {
                var viewInstance = Activator.CreateInstance(viewType) as IMvxOverridePresentationAttribute;
                var presentationAttribute = (viewInstance as IMvxOverridePresentationAttribute)?.PresentationAttribute();
                if (presentationAttribute == null)
                {
                    MvxTrace.Warning("Override PresentationAttribute null. Falling back to existing attribute.");
                }
                else
                {
                    if (presentationAttribute.ViewType == null)
                        presentationAttribute.ViewType = viewType;

                    if (presentationAttribute.ViewModelType == null)
                        presentationAttribute.ViewModelType = viewModelType;

                    return presentationAttribute;
                }
            }

            return null;
        }

        public override void Show(MvxViewModelRequest request)
        {
            GetPresentationAttributeAction(request, out MvxBasePresentationAttribute attribute).ShowAction.Invoke(attribute.ViewType, attribute, request);
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
            var attribute = GetPresentationAttribute(viewModel.GetType());
            var attributeType = attribute.GetType();

            if (AttributeTypesToActionsDictionary.TryGetValue(
                attributeType,
                out MvxPresentationAttributeAction attributeAction))
            {
                if (attributeAction.CloseAction == null)
                {
                    throw new NullReferenceException($"attributeAction.CloseAction is null for attribute: {attributeType.Name}");
                }

                attributeAction.CloseAction.Invoke(viewModel, attribute);
                return;
            }

            throw new KeyNotFoundException($"The type {attributeType.Name} is not configured in the presenter dictionary");
        }

        protected virtual async void BackButtonOnBackRequested(object sender, BackRequestedEventArgs backRequestedEventArgs)
        {
            if (backRequestedEventArgs.Handled)
                return;

            var currentView = _rootFrame.Content as IMvxView;
            if (currentView == null)
            {
                Mvx.Warning("Ignoring close for viewmodel - rootframe has no current page");
                return;
            }

            var navigationService = Mvx.Resolve<IMvxNavigationService>();

            backRequestedEventArgs.Handled = await navigationService.Close(currentView.ViewModel);
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

        protected virtual void HandleBackButtonVisibility()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                _rootFrame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }

        protected virtual void ShowSplitView(Type viewType, MvxSplitViewPresentationAttribute attribute, MvxViewModelRequest request)
        {
            var viewsContainer = Mvx.Resolve<IMvxViewsContainer>();

            if (_rootFrame.Content is MvxWindowsPage currentPage)
            {
                var splitView = currentPage.Content.FindControl<SplitView>();
                if (splitView == null)
                {
                    throw new MvxException($"Failed to find a SplitView in the visual tree of {viewType.Name}");
                }

                if (attribute.Position == SplitPanePosition.Content)
                {
                    splitView.Content = (UIElement)Activator.CreateInstance(viewType);
                }
                else if (attribute.Position == SplitPanePosition.Pane)
                {
                    splitView.Pane = (UIElement)Activator.CreateInstance(viewType);
                }
            }
        }

        protected virtual bool CloseSplitView(IMvxViewModel viewModel, MvxSplitViewPresentationAttribute attribute)
        {
            return ClosePage(viewModel, attribute);
        }

        protected virtual void ShowRegionView(Type viewType, MvxRegionPresentationAttribute attribute, MvxViewModelRequest request)
        {
            if (viewType.HasRegionAttribute())
            {
                var requestText = GetRequestText(request);

                var containerView = _rootFrame.UnderlyingControl.FindControl<Frame>(viewType.GetRegionName());

                if (containerView != null)
                {
                    containerView.Navigate(viewType, requestText);
                    return;
                }
            }
        }

        protected virtual bool CloseRegionView(IMvxViewModel viewModel, MvxRegionPresentationAttribute attribute)
        {
            var viewFinder = Mvx.Resolve<IMvxViewsContainer>();
            var viewType = viewFinder.GetViewType(viewModel.GetType());
            if (viewType.HasRegionAttribute())
            {
                var containerView = _rootFrame.UnderlyingControl?.FindControl<Frame>( viewType.GetRegionName());

                if (containerView == null)
                    throw new MvxException($"Region '{viewType.GetRegionName()}' not found in view '{viewType}'");

                if (containerView.CanGoBack)
                {
                    containerView.GoBack();
                    return true;
                }
            }

            return ClosePage(viewModel, attribute);
        }

        protected virtual bool ClosePage(IMvxViewModel viewModel, MvxBasePresentationAttribute attribute)
        {
            var currentView = _rootFrame.Content as IMvxView;
            if (currentView == null)
            {
                Mvx.Warning("Ignoring close for viewmodel - rootframe has no current page");
                return false;
            }

            if (currentView.ViewModel != viewModel)
            {
                Mvx.Warning("Ignoring close for viewmodel - rootframe's current page is not the view for the requested viewmodel");
                return false;
            }

            if (!_rootFrame.CanGoBack)
            {
                Mvx.Warning("Ignoring close for viewmodel - rootframe refuses to go back");
                return false;
            }

            _rootFrame.GoBack();

            HandleBackButtonVisibility();

            return true;
        }

        protected virtual void ShowPage(Type viewType, MvxBasePresentationAttribute attribute, MvxViewModelRequest request)
        {
            try
            {
                var requestText = GetRequestText(request);
                var viewsContainer = Mvx.Resolve<IMvxViewsContainer>();

                _rootFrame.Navigate(viewType, requestText); //Frame won't allow serialization of it's nav-state if it gets a non-simple type as a nav param

                HandleBackButtonVisibility();
            }
            catch (Exception exception)
            {
                MvxTrace.Trace("Error seen during navigation request to {0} - error {1}", request.ViewModelType.Name,
                    exception.ToLongString());
            }
        }
    }
}