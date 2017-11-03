// MvxStoreViewPresenter.cs

// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Exceptions;
using MvvmCross.Platform.Platform;
using MvvmCross.Uwp.Attributes;

namespace MvvmCross.Uwp.Views
{
    public class MvxWindowsViewPresenter
        : MvxViewPresenter, IMvxWindowsViewPresenter, IMvxAttributeViewPresenter
    {
        protected readonly IMvxWindowsFrame _rootFrame;

        public MvxWindowsViewPresenter(IMvxWindowsFrame rootFrame)
        {
            _rootFrame = rootFrame;

            SystemNavigationManager.GetForCurrentView().BackRequested += BackButtonOnBackRequested;
        }

        private IMvxViewModelTypeFinder _viewModelTypeFinder;
        public IMvxViewModelTypeFinder ViewModelTypeFinder
        {
            get
            {
                if (_viewModelTypeFinder == null)
                    _viewModelTypeFinder = Mvx.Resolve<IMvxViewModelTypeFinder>();
                return _viewModelTypeFinder;
            }
            set
            {
                _viewModelTypeFinder = value;
            }
        }

        private IMvxViewsContainer _viewsContainer;
        public IMvxViewsContainer ViewsContainer
        {
            get
            {
                if (_viewsContainer == null)
                    _viewsContainer = Mvx.Resolve<IMvxViewsContainer>();
                return _viewsContainer;
            }
            set
            {
                _viewsContainer = value;
            }
        }

        private Dictionary<Type, MvxPresentationAttributeAction> _attributeTypesActionsDictionary;
        public Dictionary<Type, MvxPresentationAttributeAction> AttributeTypesToActionsDictionary {
            get {
                if (_attributeTypesActionsDictionary == null)
                {
                    _attributeTypesActionsDictionary = new Dictionary<Type, MvxPresentationAttributeAction>();
                    RegisterAttributeTypes();
                }
                return _attributeTypesActionsDictionary;
            }
        }

        public void RegisterAttributeTypes()
        {
            AttributeTypesToActionsDictionary.Add(
                typeof(MvxPagePresentationAttribute),
                new MvxPresentationAttributeAction
                {
                    ShowAction = (view, attribute, request) => ShowPage(view, (MvxPagePresentationAttribute)attribute, request),
                    CloseAction = (viewModel, attribute) => ClosePage(viewModel, (MvxPagePresentationAttribute)attribute)
                });

            AttributeTypesToActionsDictionary.Add(
                typeof(MvxSplitViewPresentationAttribute),
                new MvxPresentationAttributeAction
                {
                    ShowAction = (view, attribute, request) => ShowSplitView(view, (MvxSplitViewPresentationAttribute)attribute, request),
                    CloseAction = (viewModel, attribute) => CloseSplitView(viewModel, (MvxSplitViewPresentationAttribute)attribute)
                });
        }

       

        private void ShowSplitView(Type view, MvxSplitViewPresentationAttribute attribute, MvxViewModelRequest request)
        {
            var viewsContainer = Mvx.Resolve<IMvxViewsContainer>();
            var viewType = viewsContainer.GetViewType(request.ViewModelType);

            if (_rootFrame.Content is MvxWindowsPage currentPage)
            {
                var splitView = FindControl<SplitView>(currentPage.Content, typeof(SplitView));
                if (splitView == null)
                {
                    splitView = new SplitView() {DisplayMode = SplitViewDisplayMode.Inline, IsPaneOpen = true};
                    currentPage.Content = splitView;
                }

                if (attribute.Position == SplitPanePosition.Content)
                {
                    splitView.Content = (UIElement)Activator.CreateInstance(viewType);
                }
                else if (attribute.Position == SplitPanePosition.Pane)
                {
                    splitView.Pane = (UIElement) Activator.CreateInstance(viewType);
                }

            }
        }

        private T FindControl<T>(UIElement parent, Type targetType) where T : FrameworkElement
        {

            if (parent == null) return null;

            if (parent.GetType() == targetType)
            {
                return (T)parent;
            }

            T result = null;
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                UIElement child = (UIElement)VisualTreeHelper.GetChild(parent, i);

                if (FindControl<T>(child, targetType) != null)
                {
                    result = FindControl<T>(child, targetType);
                    break;
                }
            }
            return result;
        }

        private bool CloseSplitView(IMvxViewModel viewModel, MvxSplitViewPresentationAttribute attribute)
        {
            return ClosePage(viewModel, attribute);
        }

        private bool ClosePage(IMvxViewModel viewModel, MvxBasePresentationAttribute attribute)
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

        private void ShowPage(Type view, MvxPagePresentationAttribute attribute, MvxViewModelRequest request)
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

        public MvxBasePresentationAttribute GetPresentationAttribute(Type viewModelType)
        {
            var viewType = ViewsContainer.GetViewType(viewModelType);
            var attributes = viewType.GetCustomAttributes(typeof(MvxBasePresentationAttribute), true).ToList();
            var attribute = attributes.OfType<MvxBasePresentationAttribute>().FirstOrDefault();
            return attribute;
        }

        public MvxBasePresentationAttribute CreatePresentationAttribute(Type viewModelType, Type viewType)
        {
            return null;
        }

        public MvxBasePresentationAttribute GetOverridePresentationAttribute(Type viewModelType, Type viewType)
        {
            return null;
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

        public override void Show(MvxViewModelRequest request)
        {
            var attribute = GetPresentationAttribute(request.ViewModelType);
            attribute.ViewModelType = request.ViewModelType;
            var viewType = attribute.ViewType;
            var attributeType = attribute.GetType();

            if (AttributeTypesToActionsDictionary.TryGetValue(
                attributeType,
                out MvxPresentationAttributeAction attributeAction))
            {
                if (attributeAction.ShowAction == null)
                    throw new NullReferenceException($"attributeAction.ShowAction is null for attribute: {attributeType.Name}");

                attributeAction.ShowAction.Invoke(viewType, attribute, request);
                return;
            }

            throw new KeyNotFoundException($"The type {attributeType.Name} is not configured in the presenter dictionary");
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
            var attribute = GetPresentationAttribute(viewModel.GetType());
            var attributeType = attribute.GetType();

            if (AttributeTypesToActionsDictionary.TryGetValue(
                attributeType,
                out MvxPresentationAttributeAction attributeAction))
            {
                if (attributeAction.CloseAction == null)
                    throw new NullReferenceException($"attributeAction.CloseAction is null for attribute: {attributeType.Name}");

                attributeAction.CloseAction.Invoke(viewModel, attribute);
                return;
            }

            throw new KeyNotFoundException($"The type {attributeType.Name} is not configured in the presenter dictionary");
        }

        protected virtual void HandleBackButtonVisibility()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                _rootFrame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }
    }
}