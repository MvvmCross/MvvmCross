#nullable enable

using Microsoft.Extensions.Logging;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using MvvmCross.Base;
using MvvmCross.Exceptions;
using MvvmCross.Localization;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using MvvmCross.Platforms.WinUi.Presenters.Attributes;
using MvvmCross.Platforms.WinUi.Presenters.Models;
using MvvmCross.Platforms.WinUi.Presenters.Utils;
using MvvmCross.Platforms.WinUi.Views;
using MvvmCross.Presenters;
using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using Windows.Graphics;
using Windows.UI.Core;
using Control = Microsoft.UI.Xaml.Controls.Control;
using HorizontalAlignment = Microsoft.UI.Xaml.HorizontalAlignment;
using MvxApplication = MvvmCross.Platforms.WinUi.Views.MvxApplication;
using Window = Microsoft.UI.Xaml.Window;

namespace MvvmCross.Platforms.WinUi.Presenters;

/// <summary>
///     Defines a view presenter with multi-windows support.
/// </summary>
public class MvxMultiWindowViewPresenter
    : MvxAttributeViewPresenter, IMvxWindowsViewPresenter, IMvxMultiWindowsService
{
    private const int DefaultWindowHeight = 456;
    private const int DefaultWindowWidth = 786;

    private const string WindowTitle = "WindowTitle";
    private readonly ILogger<MvxWindowsViewPresenter>? _logger;
    private readonly WindowInformation _mainFrame;
    private readonly List<WindowInformation> _windowInformation = new();

    private readonly object _windowInformationLock = new();

    private IMvxViewModelLoader? _viewModelLoader;

    /// <summary>
    ///     Initializes a new instance of <see cref="MvxMultiWindowViewPresenter" />.
    /// </summary>
    /// <param name="rootFrame">The root frame.</param>
    public MvxMultiWindowViewPresenter(IMvxWindowsFrame rootFrame)
    {
        var window = (Microsoft.UI.Xaml.Application.Current as MvxApplication)?.MainWindow;
        if (window != null)
        {
            window.AppWindow.Closing += (_, __) => CloseAllWindows();
        }

        _mainFrame = new WindowInformation(window!, rootFrame, null);
        _logger = MvxLogHost.GetLog<MvxWindowsViewPresenter>();

        if (Window.Current != null)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested += BackButtonOnBackRequested;
        }
    }

    /// <summary>
    ///     Get or sets the viewmodel loader instance.
    /// </summary>
    public IMvxViewModelLoader? ViewModelLoader
    {
        get => _viewModelLoader ??= Mvx.IoCProvider?.Resolve<IMvxViewModelLoader>();
        set => _viewModelLoader = value;
    }

    /// <summary>
    ///     Creates a presentation attribute.
    /// </summary>
    /// <param name="viewModelType"></param>
    /// <param name="viewType"></param>
    /// <returns></returns>
    public override MvxBasePresentationAttribute CreatePresentationAttribute(Type viewModelType, Type viewType)
    {
        _logger?.LogInformation("PresentationAttribute not found for {ViewTypeName}. Assuming new page presentation",
            viewType.Name);
        return new MvxPagePresentationAttribute { ViewType = viewType, ViewModelType = viewModelType };
    }

    /// <summary>
    ///     Registers default attribute types.
    /// </summary>
    public override void RegisterAttributeTypes()
    {
        AttributeTypesToActionsDictionary.Register<MvxPagePresentationAttribute>(ShowPage,
            ClosePage);
        AttributeTypesToActionsDictionary.Register<MvxSplitViewPresentationAttribute>(ShowSplitView,
            CloseSplitView);
        AttributeTypesToActionsDictionary.Register<MvxRegionPresentationAttribute>(ShowRegionView,
            CloseRegionView);
        AttributeTypesToActionsDictionary.Register<MvxDialogViewPresentationAttribute>(ShowDialogAsync,
            CloseDialog);
        AttributeTypesToActionsDictionary.Add(
            typeof(MvxNewWindowPresentationAttribute),
            new MvxPresentationAttributeAction
            {
                ShowAction = async (_, attribute, request) =>
                {
                    if (attribute is not MvxNewWindowPresentationAttribute presentationAttribute)
                    {
                        return false;
                    }

                    return await ShowNewWindowAsync(request, presentationAttribute);
                },
                CloseAction = (viewModel, _) =>
                {
                    viewModel.ViewDisappearing();
                    viewModel.ViewDisappeared();
                    viewModel.ViewDestroy();
                    viewModel.DisposeIfDisposable();
                    return Task.FromResult(true);
                }
            });
    }

    /// <summary>
    ///     Closes all windows, except the main window, and the view models belonging to those windows.
    /// </summary>
    public void CloseAllWindows()
    {
        List<WindowInformation> windows;
        lock (_windowInformation)
        {
            windows = _windowInformation.ToList();
        }

        foreach (var wi in windows)
        {
            try
            {
                CloseWindow(wi.Window);
            }
            catch (Exception)
            {
                // Swallow all exceptions.
            }

            wi.Window.Close();
        }
    }

    /// <summary>
    ///     Creates a control for the given view type.
    /// </summary>
    /// <param name="viewType">The view type.</param>
    /// <param name="request">The request.</param>
    /// <param name="attribute">Any attributes.</param>
    /// <returns></returns>
    /// <exception cref="MvxException"></exception>
    public virtual Control? CreateControl(Type viewType, MvxViewModelRequest request,
        MvxBasePresentationAttribute attribute)
    {
        try
        {
            var control = Activator.CreateInstance(viewType) as Control;
            if (control is IMvxView mvxControl)
            {
                if (request is MvxViewModelInstanceRequest instanceRequest)
                {
                    mvxControl.ViewModel = instanceRequest.ViewModelInstance;
                }
                else
                {
                    mvxControl.ViewModel = ViewModelLoader?.LoadViewModel(request, null);
                }
            }

            return control;
        }
        catch (Exception ex)
        {
            throw new MvxException(ex,
                $"Cannot create Control '{viewType.FullName}'. Are you use the wrong base class?");
        }
    }

    /// <summary>
    ///     Executed when the onBack is requested.
    /// </summary>
    /// <param name="sender">The sender of the event.</param>
    /// <param name="backRequestedEventArgs">The event arguments.</param>
    protected virtual async void BackButtonOnBackRequested(object? sender,
        BackRequestedEventArgs backRequestedEventArgs)
    {
        if (backRequestedEventArgs.Handled)
        {
            return;
        }

        var currentView = GetWindowInformation(Window.Current).RootFrame.Content as IMvxView;
        if (currentView == null)
        {
            _logger?.LogWarning("Ignoring close for viewmodel - root frame has no current page");
            return;
        }

        var navigationService = Mvx.IoCProvider?.Resolve<IMvxNavigationService>();
        if (navigationService != null && currentView.ViewModel != null)
        {
            backRequestedEventArgs.Handled = await navigationService.Close(currentView.ViewModel);
        }
    }

    /// <summary>
    ///     CLoses the dialog belonging to the given viewmodel.
    /// </summary>
    /// <param name="viewModel">The viewmodel to close the dialog for.</param>
    /// <param name="attribute">The presentation attributes.</param>
    /// <returns>True upon success, false otherwise.</returns>
    protected virtual Task<bool> CloseDialog(IMvxViewModel viewModel, MvxBasePresentationAttribute attribute)
    {
        var windowInformation = GetWindowInformation(viewModel);
        if (windowInformation.RootFrame.UnderlyingControl is not Frame frame)
        {
            return Task.FromResult(false);
        }

        var popups = VisualTreeHelper.GetOpenPopupsForXamlRoot(frame.XamlRoot).FirstOrDefault(p =>
        {
            if (attribute.ViewType != null && attribute.ViewType.IsInstanceOfType(p.Child)
                                           && p.Child is IMvxWindowsContentDialog dialog)
            {
                return dialog.ViewModel == viewModel;
            }

            return false;
        });

        (popups?.Child as ContentDialog)?.Hide();
        windowInformation.UnregisterSubViewModel(viewModel);
        return Task.FromResult(true);
    }

    /// <summary>
    ///     Closes teh page for the given view model.
    /// </summary>
    /// <param name="viewModel">The viewmodel to close the page for.</param>
    /// <param name="attribute">The presentation attributes</param>
    /// <returns>True if closed, false otherwise.</returns>
    protected virtual Task<bool> ClosePage(IMvxViewModel viewModel, MvxBasePresentationAttribute attribute)
    {
        var windowInformation = GetWindowInformation(viewModel);
        var currentView = windowInformation.RootFrame.Content as IMvxView;
        if (currentView == null)
        {
            _logger?.LogWarning("Ignoring close for viewmodel - root frame has no current page");
            return Task.FromResult(false);
        }

        if (currentView.ViewModel != viewModel)
        {
            _logger?.LogWarning(
                "Ignoring close for viewmodel - root frame's current page is not the view for the requested viewmodel");
            return Task.FromResult(false);
        }

        if (!windowInformation.RootFrame.CanGoBack)
        {
            _logger?.LogWarning("Ignoring close for viewmodel - root frame refuses to go back");
            return Task.FromResult(false);
        }

        windowInformation.RootFrame.GoBack();

        HandleBackButtonVisibility();
        windowInformation.UnregisterSubViewModel(viewModel);

        return Task.FromResult(true);
    }

    /// <summary>
    ///     Closes the region view for the given viewmodel.
    /// </summary>
    /// <param name="viewModel">The viewmodel to close the region for.</param>
    /// <param name="attribute">Any presentation attribute.</param>
    /// <returns>True if successful. False otherwise.</returns>
    /// <exception cref="MvxException">If no region is found for the given viewmodel.</exception>
    protected virtual Task<bool> CloseRegionView(IMvxViewModel viewModel, MvxRegionPresentationAttribute attribute)
    {
        var windowInformation = GetWindowInformation(viewModel);
        var viewFinder = Mvx.IoCProvider?.Resolve<IMvxViewsContainer>();
        if (viewFinder == null)
        {
            return Task.FromResult(false);
        }

        var viewType = viewFinder.GetViewType(viewModel.GetType());
        if (viewType.HasRegionAttribute())
        {
            var containerView =
                windowInformation.RootFrame.UnderlyingControl?.FindControl<Frame>(viewType.GetRegionName());

            if (containerView == null)
            {
                // This can happen if a parent view is already removed.
                return Task.FromResult(false);
            }

            if (containerView.CanGoBack)
            {
                containerView.GoBack();
                if (containerView.BackStackDepth == 0)
                {
                    if (containerView.ContentTransitions.Any(ct
                            => ((ct as NavigationThemeTransition)?.DefaultNavigationTransitionInfo as
                                   SlideNavigationTransitionInfo)?.Effect ==
                               SlideNavigationTransitionEffect.FromLeft))
                    {
                        containerView.HorizontalAlignment = HorizontalAlignment.Right;
                    }

                    containerView.HorizontalAlignment = HorizontalAlignment.Left;
                }

                windowInformation.UnregisterSubViewModel(viewModel);
                return Task.FromResult(true);
            }
        }

        return ClosePage(viewModel, attribute);
    }

    /// <summary>
    ///     Closes the splitview.
    /// </summary>
    /// <param name="viewModel">The viewmodel to close the splitview for.</param>
    /// <param name="attribute">Any presentation attribute.</param>
    /// <returns>True if successful, false otherwise.</returns>
    protected virtual Task<bool> CloseSplitView(IMvxViewModel viewModel,
        MvxSplitViewPresentationAttribute attribute)
    {
        return ClosePage(viewModel, attribute);
    }

    /// <summary>
    ///     Converts a request to a string format.
    ///     A Frame won't allow serialization of it's nav-state if it gets a non-simple type as a nav param
    /// </summary>
    /// <param name="request">The request to convert.</param>
    /// <returns>A text representation of the request.</returns>
    protected virtual string GetRequestText(MvxViewModelRequest request)
    {
        var requestTranslator = Mvx.IoCProvider?.Resolve<IMvxWindowsViewModelRequestTranslator>();
        if (requestTranslator == null)
        {
            return "Request translator is not found";
        }

        string requestText;
        requestText = request is MvxViewModelInstanceRequest
            ? requestTranslator.GetRequestTextWithKeyFor(((MvxViewModelInstanceRequest)request).ViewModelInstance)
            : requestTranslator.GetRequestTextFor(request);

        return requestText;
    }

    /// <summary>
    ///     Gets the correct root frame for the request.
    ///     There is no window or viewmodel for the original root frame.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <returns>The root frame, if no special root frame from a window is found the mainframe is returned.</returns>
    protected WindowInformation GetWindowInformation(MvxViewModelRequest request)
    {
        lock (_windowInformationLock)
        {
            var frame = _mainFrame;
            if (request is MvxViewModelInstanceRequestWithSource targetRequest)
            {
                frame = _windowInformation.Find(wi => wi.IsFor(targetRequest.Source)) ??
                        _mainFrame;
            }

            return frame;
        }
    }

    /// <summary>
    ///     Gets the correct root frame for the request.
    ///     There is no window or viewmodel for the original root frame.
    /// </summary>
    /// <param name="viewModel">The viewmodel to get the root frame for.</param>
    /// <returns>The root frame, if no special root frame from a window is found the mainframe is returned.</returns>
    protected WindowInformation GetWindowInformation(IMvxViewModel viewModel)
    {
        lock (_windowInformationLock)
        {
            return _windowInformation.Find(wi => wi.IsFor(viewModel)) ?? _mainFrame;
        }
    }

    /// <summary>
    ///     Gets the correct root frame for the request.
    ///     There is no window or viewmodel for the original root frame.
    /// </summary>
    /// <param name="window">The window to get the root frame for.</param>
    /// <returns>The root frame, if no special root frame from a window is found the mainframe is returned.</returns>
    protected WindowInformation GetWindowInformation(Window window)
    {
        lock (_windowInformationLock)
        {
            return _windowInformation.Find(wi => wi.IsFor(window)) ?? _mainFrame;
        }
    }

    /// <summary>
    ///     Updates the visibility state of the back button.
    /// </summary>
    protected virtual void HandleBackButtonVisibility()
    {
        if (Window.Current == null)
        {
            return;
        }

        var rootFrame = GetWindowInformation(Window.Current).RootFrame;

        SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
            rootFrame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
    }

    /// <summary>
    ///     Shows a dialog for the given request.
    /// </summary>
    /// <param name="viewType">The type of the content.</param>
    /// <param name="attribute">Any presentation attribute.</param>
    /// <param name="request">The request to show the dialog for.</param>
    /// <returns>True if successful, false otherwise.</returns>
    protected virtual async Task<bool> ShowDialogAsync(Type viewType, MvxDialogViewPresentationAttribute attribute,
        MvxViewModelRequest request)
    {
        try
        {
            var contentDialog = CreateControl(viewType, request, attribute) as ContentDialog;

            if (contentDialog != null)
            {
                var windowInfo = GetWindowInformation(request);
                if (windowInfo.RootFrame.UnderlyingControl is Frame frame)
                {
                    contentDialog.XamlRoot = frame.XamlRoot;
                }

                await contentDialog.ShowAsync(attribute.Placement);
                if (contentDialog is IMvxView mvxControl && mvxControl.ViewModel != null)
                {
                    windowInfo.RegisterSubViewModel(mvxControl.ViewModel);
                }

                return true;
            }

            return false;
        }
        catch (Exception exception)
        {
            _logger?.LogError(exception, "Error seen during navigation request to {ViewModelTypeName}",
                request.ViewModelType?.Name);
            return false;
        }
    }

    /// <summary>
    ///     Shows a page for the given request.
    /// </summary>
    /// <param name="viewType">The type of the content.</param>
    /// <param name="attribute">Any presentation attribute.</param>
    /// <param name="request">The request to show the page.</param>
    /// <returns>True if successful, false otherwise.</returns>
    protected virtual Task<bool> ShowPage(Type viewType, MvxBasePresentationAttribute attribute,
        MvxViewModelRequest request)
    {
        return ShowPage(GetWindowInformation(request).RootFrame, viewType, request);
    }

    /// <summary>
    ///     Shows content in a region.
    /// </summary>
    /// <param name="viewType">The type of content to show.</param>
    /// <param name="attribute">Any presentation attributes.</param>
    /// <param name="request">The request.</param>
    /// <returns>True if successful, false otherwise.</returns>
    protected virtual Task<bool> ShowRegionView(Type viewType, MvxRegionPresentationAttribute attribute,
        MvxViewModelRequest request)
    {
        if (viewType.HasRegionAttribute())
        {
            var windowInformation = GetWindowInformation(request);
            var requestText = GetRequestText(request);
            var containerView =
                windowInformation.RootFrame.UnderlyingControl.FindControl<Frame>(viewType.GetRegionName());
            if (request is MvxViewModelInstanceRequestWithSource targetRequest &&
                targetRequest.ViewModelInstance != null)
            {
                windowInformation.RegisterSubViewModel(targetRequest.ViewModelInstance);
            }

            if (containerView != null)
            {
                containerView.Navigate(viewType, requestText);

                containerView.HorizontalAlignment = HorizontalAlignment.Stretch;
                return Task.FromResult(true);
            }
        }

        return Task.FromResult(true);
    }

    /// <summary>
    ///     Shows a splitview.
    /// </summary>
    /// <param name="viewType">The type of content to show.</param>
    /// <param name="attribute">Any presentation attributes.</param>
    /// <param name="request">The request.</param>
    /// <returns>True if successful, false otherwise.</returns>
    protected virtual Task<bool> ShowSplitView(Type viewType, MvxSplitViewPresentationAttribute attribute,
        MvxViewModelRequest request)
    {
        var windowInformation = GetWindowInformation(request);
        if (windowInformation.RootFrame.Content is MvxWindowsPage currentPage)
        {
            var splitView = currentPage.Content.FindControl<SplitView>();
            if (splitView == null)
            {
                return Task.FromResult(false);
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

                if (request is MvxViewModelInstanceRequest instanceReq && instanceReq.ViewModelInstance != null)
                {
                    windowInformation.RegisterSubViewModel(instanceReq.ViewModelInstance);
                }
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

                if (request is MvxViewModelInstanceRequest instanceReq && instanceReq.ViewModelInstance != null)
                {
                    windowInformation.RegisterSubViewModel(instanceReq.ViewModelInstance);
                }
            }
        }

        return Task.FromResult(true);
    }

    /// <summary>
    ///     Invokes a given function on the target <see cref="DispatcherQueue" /> and returns a
    ///     <see cref="Task" /> that completes when the invocation of the function is completed.
    /// </summary>
    /// <param name="dispatcher">The target <see cref="DispatcherQueue" /> to invoke the code on.</param>
    /// <param name="function">The <see cref="Action" /> to invoke.</param>
    /// <param name="priority">The priority level for the function to invoke.</param>
    /// <returns>A <see cref="Task" /> that completes when the invocation of <paramref name="function" /> is over.</returns>
    /// <remarks>
    ///     If the current thread has access to <paramref name="dispatcher" />, <paramref name="function" /> will be
    ///     invoked directly.
    /// </remarks>
    private static Task EnqueueAsync(DispatcherQueue dispatcher, Action function,
        DispatcherQueuePriority priority = DispatcherQueuePriority.Normal)
    {
        // Run the function directly when we have thread access.
        // Also reuse Task.CompletedTask in case of success,
        // to skip an unnecessary heap allocation for every invocation.
        if (dispatcher.HasThreadAccess)
        {
            try
            {
                function();

                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                return Task.FromException(e);
            }
        }

        static Task TryEnqueue(DispatcherQueue dispatcher, Action function, DispatcherQueuePriority priority)
        {
            var taskCompletionSource =
                new TaskCompletionSource<object?>(TaskCreationOptions.RunContinuationsAsynchronously);

            if (!dispatcher.TryEnqueue(
                    priority,
                    () =>
                    {
                        try
                        {
                            function();

                            taskCompletionSource.SetResult(null);
                        }
                        catch (Exception e)
                        {
                            taskCompletionSource.SetException(e);
                        }
                    }))
            {
                taskCompletionSource.SetException(new Exception("Failed to enqueue the operation"));
            }

            return taskCompletionSource.Task;
        }

        return TryEnqueue(dispatcher, function, priority);
    }

    private void CloseWindow(Window newWindow)
    {
        var windowInformation = GetWindowInformation(newWindow);
        if (windowInformation.ViewModel != null)
        {
            Close(windowInformation.ViewModel);
        }
    }

    protected virtual async Task<bool> ShowNewWindowAsync(MvxViewModelRequest request, MvxNewWindowPresentationAttribute attribute)
    {
        var newWindow = new Window();
        var viewsContainer = Mvx.IoCProvider!.Resolve<IMvxViewsContainer>();
        var viewType = viewsContainer?.GetViewType(request.ViewModelType);
        if (viewType == null)
        {
            _logger?.LogError("Could not find View for ViewModelType: {ViewModelType}", request.ViewModelType);
            return false;
        }

        var frame = new MvxWrappedFrame(new Frame());
        await ShowPage(frame, viewType, request);

        newWindow.Content = frame.UnderlyingControl;

        var page = (Page)frame.Content;
        Microsoft.UI.Windowing.AppWindow appWindow = AppWindowUtils.GetAppWindowForCurrentWindow(newWindow);

        if (page.DataContext is IMvxLocalizedTextSourceOwner viewModel)
        {
            appWindow.Title = viewModel.LocalizedTextSource.GetText(WindowTitle);
        }

        // Set size of new window based on the main window.
        if (!AppWindowUtils.TryGetAppWindow(out Microsoft.UI.Windowing.AppWindow? mainWindow) || mainWindow == null)
        {
            _logger?.LogWarning("Failed to get App Window");
            return false;
        }

        SizeInt32 size = GetScaledWindowSize(attribute, newWindow, appWindow);
        appWindow.ResizeClient(size);

        // NOTE: This line comes from the community toolkit which is not installed. So we Copied it in.
        await EnqueueAsync(newWindow.DispatcherQueue, newWindow.Activate);

        var model = (IMvxViewModel)page.DataContext;

        lock (_windowInformationLock)
        {
            _windowInformation.Add(new WindowInformation(newWindow, frame, model));
        }

        if (page is IMvxNeedWindow needWindow)
        {
            needWindow.SetWindow(newWindow, appWindow);
        }

        // Closing will be handled before Closed of the Window.
        appWindow.Closing += (_, e) =>
        {
            if (page is IMvxNeedWindow needWindow2 && !needWindow2.CanClose())
            {
                e.Cancel = true;
            }

        };

        newWindow.Closed += (_, _) =>
        {
            CloseWindow(newWindow);
            page.DisposeIfDisposable();

            lock (_windowInformationLock)
            {
                _windowInformation.Remove(GetWindowInformation(model));
            }
        };

        return true;
    }

    private static SizeInt32 GetScaledWindowSize(
        MvxNewWindowPresentationAttribute attribute,
        Window newWindow,
        Microsoft.UI.Windowing.AppWindow appWindow)
    {
        double scaleFactor = appWindow.ClientSize.Width / newWindow.Bounds.Width;

        var height = attribute.Height.HasValue
            ? attribute.Height.Value * scaleFactor
            : DefaultWindowHeight * scaleFactor;
        var width = attribute.Width.HasValue
            ? attribute.Width.Value * scaleFactor
            : DefaultWindowWidth * scaleFactor;

        var size = new SizeInt32((int)width, (int)height);
        return size;
    }

    /// <summary>
    ///     Shows a page for the given request.
    /// </summary>
    /// <param name="rootFrame">The root frame to show the page in.</param>
    /// <param name="viewType">The type of the content.</param>
    /// <param name="request">The request to show the page.</param>
    /// <returns>True if successful, false otherwise.</returns>
    // ReSharper disable once UnusedParameter.Local
    private Task<bool> ShowPage(IMvxWindowsFrame rootFrame, Type viewType, MvxViewModelRequest request)
    {
        try
        {
            var requestText = GetRequestText(request);

            //Frame won't allow serialization of it's nav-state if it gets a non-simple type as a nav param
            rootFrame.Navigate(viewType, requestText);

            HandleBackButtonVisibility();
            return Task.FromResult(true);
        }
        catch (Exception exception)
        {
            _logger?.LogTrace(exception, "Error seen during navigation request to {viewModelTypeName}",
                request.ViewModelType?.Name ?? "No view model type specified.");
            return Task.FromResult(false);
        }
    }

    /// <inheritdoc />
    public Window GetWindow(IMvxViewModel viewModel)
    {
        return GetWindowInformation(viewModel).Window;
    }
}
