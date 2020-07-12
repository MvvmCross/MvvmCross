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
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;
using System.Linq;

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

        private IMvxViewModelLoader? _viewModelLoader;
        public IMvxViewModelLoader ViewModelLoader
        {
            get
            {
                if (_viewModelLoader == null)
                    _viewModelLoader = Mvx.IoCProvider.Resolve<IMvxViewModelLoader>();
                return _viewModelLoader;
            }
            set
            {
                _viewModelLoader = value;
            }
        }

        public override void RegisterAttributeTypes()
        {
            AttributeTypesToActionsDictionary.Register<MvxPagePresentationAttribute>(ShowPage, ClosePage);
            AttributeTypesToActionsDictionary.Register<MvxSplitViewPresentationAttribute>(ShowSplitView, CloseSplitView);
            AttributeTypesToActionsDictionary.Register<MvxRegionPresentationAttribute>(ShowRegionView, CloseRegionView);
            AttributeTypesToActionsDictionary.Register<MvxDialogViewPresentationAttribute>(ShowDialog, CloseDialog);
        }

        public override ValueTask<MvxBasePresentationAttribute?> CreatePresentationAttribute(Type? viewModelType, Type? viewType)
        {
            MvxLog.Instance.Trace($"PresentationAttribute not found for {viewType?.Name}. Assuming new page presentation");
            var attribute = new MvxPagePresentationAttribute() { ViewType = viewType, ViewModelType = viewModelType };
            return new ValueTask<MvxBasePresentationAttribute?>(attribute);
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

            backRequestedEventArgs.Handled = await navigationService.Close(currentView.ViewModel).ConfigureAwait(false);
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

        protected virtual ValueTask<bool> ShowSplitView(Type viewType, MvxSplitViewPresentationAttribute? attribute, MvxViewModelRequest request)
        {
            //var viewsContainer = Mvx.IoCProvider.Resolve<IMvxViewsContainer>();

            if (_rootFrame.Content is MvxWindowsPage currentPage)
            {
                var splitView = currentPage.Content.FindControl<SplitView>();
                if (splitView == null)
                {
                    return new ValueTask<bool>(true);
                }

                if (attribute!.Position == SplitPanePosition.Content)
                {
                    if (!(splitView.Content is Frame nestedFrame))
                    {
                        nestedFrame = new Frame();
                        splitView.Content = nestedFrame;
                    }
                    var requestText = GetRequestText(request);
                    nestedFrame.Navigate(viewType, requestText);
                }
                else if (attribute.Position == SplitPanePosition.Pane)
                {
                    if (!(splitView.Pane is Frame nestedFrame))
                    {
                        nestedFrame = new Frame();
                        splitView.Pane = nestedFrame;
                    }
                    var requestText = GetRequestText(request);
                    nestedFrame.Navigate(viewType, requestText);
                }
            }
            return new ValueTask<bool>(true);
        }

        protected virtual ValueTask<bool> CloseSplitView(IMvxViewModel viewModel, MvxSplitViewPresentationAttribute? attribute)
        {
            return ClosePage(viewModel, attribute);
        }

        protected virtual ValueTask<bool> ShowRegionView(Type viewType, MvxRegionPresentationAttribute? attribute, MvxViewModelRequest request)
        {
            if (viewType.HasRegionAttribute())
            {
                var requestText = GetRequestText(request);

                var containerView = _rootFrame.UnderlyingControl.FindControl<Frame>(viewType.GetRegionName());

                if (containerView != null)
                {
                    containerView.Navigate(viewType, requestText);
                    return new ValueTask<bool>(true);
                }
            }
            return new ValueTask<bool>(true);
        }

        protected virtual ValueTask<bool> CloseRegionView(IMvxViewModel viewModel, MvxRegionPresentationAttribute? attribute)
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
                    return new ValueTask<bool>(true);
                }
            }

            return ClosePage(viewModel, attribute);
        }

        protected virtual ValueTask<bool> ClosePage(IMvxViewModel viewModel, MvxBasePresentationAttribute? attribute)
        {
            var currentView = _rootFrame.Content as IMvxView;
            if (currentView == null)
            {
                MvxLog.Instance.Warn("Ignoring close for viewmodel - rootframe has no current page");
                return new ValueTask<bool>(false);
            }

            if (currentView.ViewModel != viewModel)
            {
                MvxLog.Instance.Warn("Ignoring close for viewmodel - rootframe's current page is not the view for the requested viewmodel");
                return new ValueTask<bool>(false);
            }

            if (!_rootFrame.CanGoBack)
            {
                MvxLog.Instance.Warn("Ignoring close for viewmodel - rootframe refuses to go back");
                return new ValueTask<bool>(false);
            }

            _rootFrame.GoBack();

            HandleBackButtonVisibility();

            return new ValueTask<bool>(true);
        }

        protected virtual ValueTask<bool> ShowPage(Type viewType, MvxBasePresentationAttribute? attribute, MvxViewModelRequest request)
        {
            try
            {
                var requestText = GetRequestText(request);
                var viewsContainer = Mvx.IoCProvider.Resolve<IMvxViewsContainer>();

                var result = _rootFrame.Navigate(viewType, requestText); //Frame won't allow serialization of it's nav-state if it gets a non-simple type as a nav param

                HandleBackButtonVisibility();
                return new ValueTask<bool>(result);
            }
            catch (Exception exception)
            {
                MvxLog.Instance.Trace("Error seen during navigation request to {0} - error {1}", request.ViewModelType!.Name,
                    exception.ToLongString());
                return new ValueTask<bool>(false);
            }
        }

        protected virtual async ValueTask<bool> ShowDialog(Type viewType, MvxDialogViewPresentationAttribute? attribute, MvxViewModelRequest request)
        {
            try
            {
                var contentDialog = (ContentDialog)await CreateControl(viewType, request, attribute).ConfigureAwait(false);
                if (contentDialog != null)
                {
                    await contentDialog.ShowAsync(attribute.Placement);
                    return true;
                }

                return false;
            }
            catch (Exception exception)
            {
                MvxLog.Instance.Trace("Error seen during navigation request to {0} - error {1}", request.ViewModelType!.Name,
                    exception.ToLongString());
                return false;
            }
        }

        public virtual async ValueTask<Control> CreateControl(Type viewType, MvxViewModelRequest request, MvxBasePresentationAttribute attribute)
        {
            try
            {
                var control = (Control)Activator.CreateInstance(viewType);
                if (control is IMvxView mvxControl)
                {
                    if (request is MvxViewModelInstanceRequest instanceRequest)
                        mvxControl.ViewModel = instanceRequest.ViewModelInstance;
                    else
                        mvxControl.ViewModel = await ViewModelLoader.LoadViewModel(request, null).ConfigureAwait(false);
                }

                return control;
            }
            catch (Exception ex)
            {
                throw new MvxException(ex, $"Cannot create Control '{viewType.FullName}'. Are you use the wrong base class?");
            }
        }

        protected virtual ValueTask<bool> CloseDialog(IMvxViewModel viewModel, MvxBasePresentationAttribute? attribute)
        {
            var popups = VisualTreeHelper.GetOpenPopups(Window.Current).FirstOrDefault(p =>
            {
                if (attribute!.ViewType!.IsAssignableFrom(p.Child.GetType())
                    && p.Child is IMvxWindowsContentDialog dialog)
                {
                    return dialog.ViewModel == viewModel;
                }
                return false;
            });

            (popups?.Child as ContentDialog)?.Hide();

            return new ValueTask<bool>(true);
        }
    }
}
