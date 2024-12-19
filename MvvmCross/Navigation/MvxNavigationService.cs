// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.
#nullable enable

using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using MvvmCross.Core;
using MvvmCross.Exceptions;
using MvvmCross.IoC;
using MvvmCross.Logging;
using MvvmCross.Navigation.EventArguments;
using MvvmCross.Presenters.Hints;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Navigation;

/// <inheritdoc cref="IMvxNavigationService"/>
public class MvxNavigationService : IMvxNavigationService
{
    private readonly IMvxIoCProvider _iocProvider;

    private readonly Lazy<ILogger?> _log = new(() =>
        MvxLogHost.GetLog<MvxNavigationService>());

    public IMvxViewDispatcher ViewDispatcher { get; }

    protected Lazy<IMvxViewsContainer?> ViewsContainer { get; }

    protected Dictionary<Regex, Type> Routes { get; } = new();

    protected IMvxViewModelLoader ViewModelLoader { get; set; }

    public event EventHandler<IMvxNavigateEventArgs>? WillNavigate;

    public event EventHandler<IMvxNavigateEventArgs>? DidNavigate;

    public event EventHandler<IMvxNavigateEventArgs>? WillClose;

    public event EventHandler<IMvxNavigateEventArgs>? DidClose;

    public event EventHandler<ChangePresentationEventArgs>? WillChangePresentation;

    public event EventHandler<ChangePresentationEventArgs>? DidChangePresentation;

    public MvxNavigationService(
        IMvxViewModelLoader viewModelLoader,
        IMvxViewDispatcher viewDispatcher,
        IMvxIoCProvider iocProvider)
    {
        _iocProvider = iocProvider;

        ViewModelLoader = viewModelLoader;
        ViewDispatcher = viewDispatcher;
        ViewsContainer = new Lazy<IMvxViewsContainer?>(() => _iocProvider.Resolve<IMvxViewsContainer>());
    }

    public void LoadRoutes(IEnumerable<Assembly> assemblies)
    {
        ArgumentNullException.ThrowIfNull(assemblies);

        Routes.Clear();
        foreach (var routeAttr in
                 assemblies.SelectMany(a => a.GetCustomAttributes<MvxNavigationAttribute>()))
        {
            Routes.Add(new Regex(routeAttr.UriRegex,
                    RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline),
                routeAttr.ViewModelOrFacade);
        }
    }

    protected virtual bool TryGetRoute(string path, out KeyValuePair<Regex, Type> entry)
    {
        ArgumentNullException.ThrowIfNull(path);

        try
        {
            var matches = Routes.Where(t => t.Key.IsMatch(path)).ToList();

            switch (matches.Count)
            {
                case 0:
                    entry = default;
                    _log.Value?.Log(LogLevel.Trace, "Unable to find routing for {Path}", path);
                    return false;

                case 1:
                    entry = matches[0];
                    return true;
            }

            var directMatch = matches.Where(t => t.Key.Match(path).Groups.Count == 1).ToList();

            if (directMatch.Count == 1)
            {
                entry = directMatch[0];
                return true;
            }

            _log.Value?.Log(LogLevel.Warning, "The following regular expressions match the provided url ({Count}), each RegEx must be unique (otherwise try using IMvxRoutingFacade): {Matches}",
                matches.Count - 1,
                string.Join(", ", matches.Select(t => t.Key.ToString())));

            // there is more than one match
            entry = default;
            return false;
        }
        catch (Exception ex)
        {
            _log.Value?.Log(LogLevel.Error, ex, "Unable to determine routability");
            entry = default;
            return false;
        }
    }

    protected virtual IDictionary<string, string> BuildParamDictionary(Regex regex, Match match)
    {
        ArgumentNullException.ThrowIfNull(regex);
        ArgumentNullException.ThrowIfNull(match);

        var paramDict = new Dictionary<string, string>();

        for (var i = 1 /* 0 == Match itself */; i < match.Groups.Count; i++)
        {
            var group = match.Groups[i];
            var name = regex.GroupNameFromNumber(i);
            var value = group.Value;
            paramDict.Add(name, value);
        }
        return paramDict;
    }

    protected virtual async Task<MvxViewModelInstanceRequest> NavigationRouteRequest(
        string path, IMvxBundle? presentationBundle = null)
    {
        ArgumentNullException.ThrowIfNull(path);

        if (!TryGetRoute(path, out var entry))
        {
            throw new MvxException($"Navigation route request could not be obtained for path: {path}");
        }

        var regex = entry.Key;
        var match = regex.Match(path);
        var paramDict = BuildParamDictionary(regex, match);
        var parameterValues = new MvxBundle(paramDict);

        var viewModelType = entry.Value;

        var request = new MvxViewModelInstanceRequest(viewModelType)
        {
            PresentationValues = presentationBundle?.SafeGetData(),
            ParameterValues = parameterValues.SafeGetData()
        };

        if (viewModelType.GetInterfaces().Contains(typeof(IMvxNavigationFacade)))
        {
            var facade = (IMvxNavigationFacade)_iocProvider.IoCConstruct(viewModelType);

            try
            {
                var facadeRequest = await facade.BuildViewModelRequest(path, paramDict).ConfigureAwait(false);
                if (facadeRequest == null)
                {
                    throw new MvxException($"{nameof(MvxNavigationService)}: Facade did not return a valid {nameof(MvxViewModelRequest)}.");
                }

                request.ViewModelType = facadeRequest.ViewModelType;

                if (facadeRequest.ParameterValues != null)
                {
                    request.ParameterValues = facadeRequest.ParameterValues;
                }

                if (facadeRequest is MvxViewModelInstanceRequest instanceRequest)
                {
                    request.ViewModelInstance = instanceRequest.ViewModelInstance ?? ViewModelLoader.LoadViewModel(request, null);
                }
                else
                {
                    request.ViewModelInstance = ViewModelLoader.LoadViewModel(request, null);
                }
            }
            catch (Exception ex)
            {
                throw ex.MvxWrap($"{nameof(MvxNavigationService)}: Exception thrown while processing URL: {path} with RoutingFacade: {viewModelType}");
            }
        }
        else
        {
            request.ViewModelInstance = ViewModelLoader.LoadViewModel(request, null);
        }

        return request;
    }

    protected async Task<MvxViewModelInstanceRequest> NavigationRouteRequest<TParameter>(
        string path, TParameter param, IMvxBundle? presentationBundle = null)
    {
        ArgumentNullException.ThrowIfNull(path);
        ArgumentNullException.ThrowIfNull(param);

        if (!TryGetRoute(path, out var entry))
        {
            throw new MvxException($"Navigation route request could not be obtained for path: {path}");
        }

        var regex = entry.Key;
        var match = regex.Match(path);
        var paramDict = BuildParamDictionary(regex, match);
        var parameterValues = new MvxBundle(paramDict);

        var viewModelType = entry.Value;

        var request = new MvxViewModelInstanceRequest(viewModelType)
        {
            PresentationValues = presentationBundle?.SafeGetData(),
            ParameterValues = parameterValues.SafeGetData()
        };

        if (viewModelType.GetInterfaces().Contains(typeof(IMvxNavigationFacade)))
        {
            var facade = (IMvxNavigationFacade)_iocProvider.IoCConstruct(viewModelType);

            try
            {
                var facadeRequest = await facade.BuildViewModelRequest(path, paramDict).ConfigureAwait(false);
                if (facadeRequest == null)
                {
                    throw new MvxException($"{nameof(MvxNavigationService)}: Facade did not return a valid {nameof(MvxViewModelRequest)}.");
                }

                request.ViewModelType = facadeRequest.ViewModelType;

                if (facadeRequest.ParameterValues != null)
                {
                    request.ParameterValues = facadeRequest.ParameterValues;
                }

                request.ViewModelInstance = ViewModelLoader.LoadViewModel(request, param, null);
            }
            catch (Exception ex)
            {
                ex.MvxWrap($"{nameof(MvxNavigationService)}: Exception thrown while processing URL: {path} with RoutingFacade: {viewModelType}");
            }
        }
        else
        {
            request.ViewModelInstance = ViewModelLoader.LoadViewModel(request, param, null);
        }

        return request;
    }

    public virtual Task<bool> CanNavigate(string path)
    {
        return Task.FromResult(TryGetRoute(path, out _));
    }

    public virtual Task<bool> CanNavigate<TViewModel>()
        where TViewModel : IMvxViewModel
    {
        return Task.FromResult(ViewsContainer.Value?.GetViewType(typeof(TViewModel)) != null);
    }

    public virtual Task<bool> CanNavigate(Type viewModelType)
    {
        return Task.FromResult(ViewsContainer.Value?.GetViewType(viewModelType) != null);
    }

    protected virtual async Task<bool> Navigate(MvxViewModelRequest request, IMvxViewModel viewModel,
        IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(viewModel);

        var args = new MvxNavigateEventArgs(viewModel, NavigationMode.Show, cancellationToken);
        OnWillNavigate(this, args);

        if (args.Cancel)
            return false;

        var hasNavigated = await ViewDispatcher.ShowViewModel(request).ConfigureAwait(false);
        if (!hasNavigated)
            return false;

        if (viewModel.InitializeTask?.Task != null)
            await viewModel.InitializeTask.Task.ConfigureAwait(false);

        OnDidNavigate(this, args);
        return true;
    }

    public virtual async Task<bool> Navigate(
        string path, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default)
    {
        var request = await NavigationRouteRequest(path, presentationBundle).ConfigureAwait(false);
        if (request.ViewModelInstance == null)
        {
            _log.Value?.Log(LogLevel.Warning, "Navigation Route Request doesn't have a ViewModelInstance");
            return false;
        }

        return await Navigate(request, request.ViewModelInstance, presentationBundle, cancellationToken).ConfigureAwait(false);
    }

    public virtual async Task<bool> Navigate<TParameter>(
            string path,
            TParameter param,
            IMvxBundle? presentationBundle = null,
            CancellationToken cancellationToken = default)
    {
        var request = await NavigationRouteRequest(path, param, presentationBundle).ConfigureAwait(false);
        if (request.ViewModelInstance == null)
        {
            _log.Value?.Log(LogLevel.Warning, "Navigation Route Request doesn't have a ViewModelInstance");
            return false;
        }
        return await Navigate(request, request.ViewModelInstance, presentationBundle, cancellationToken).ConfigureAwait(false);
    }

    public virtual Task<bool> Navigate(Type viewModelType, IMvxBundle? presentationBundle = null,
        CancellationToken cancellationToken = default)
    {
        var request = new MvxViewModelInstanceRequest(viewModelType)
        {
            PresentationValues = presentationBundle?.SafeGetData()
        };
        request.ViewModelInstance = ViewModelLoader.LoadViewModel(request, null);
        return Navigate(request, request.ViewModelInstance, presentationBundle, cancellationToken);
    }

    public virtual Task<bool> Navigate<TParameter>(Type viewModelType, TParameter param,
        IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default)
    {
        var request = new MvxViewModelInstanceRequest(viewModelType)
        {
            PresentationValues = presentationBundle?.SafeGetData()
        };
        request.ViewModelInstance = ViewModelLoader.LoadViewModel(request, param, null);
        return Navigate(request, request.ViewModelInstance, presentationBundle, cancellationToken);
    }

    public virtual Task<bool> Navigate<TViewModel>(
        IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default)
        where TViewModel : IMvxViewModel
    {
        return Navigate(typeof(TViewModel), presentationBundle, cancellationToken);
    }

    public virtual Task<bool> Navigate<TViewModel, TParameter>(
        TParameter param, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default)
        where TViewModel : IMvxViewModel<TParameter>
    {
        return Navigate(typeof(TViewModel), param, presentationBundle, cancellationToken);
    }

    public virtual Task<bool> Navigate(
        IMvxViewModel viewModel, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default)
    {
        var request = new MvxViewModelInstanceRequest(viewModel) { PresentationValues = presentationBundle?.SafeGetData() };
        ViewModelLoader.ReloadViewModel(viewModel, request, null);
        return Navigate(request, viewModel, presentationBundle, cancellationToken);
    }

    public virtual Task<bool> Navigate<TParameter>(IMvxViewModel<TParameter> viewModel, TParameter param,
        IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default)
    {
        var request = new MvxViewModelInstanceRequest(viewModel) { PresentationValues = presentationBundle?.SafeGetData() };
        ViewModelLoader.ReloadViewModel(viewModel, param, request, null);
        return Navigate(request, viewModel, presentationBundle, cancellationToken);
    }

    public virtual async Task<bool> ChangePresentation(
        MvxPresentationHint hint, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(hint);

        _log.Value?.Log(LogLevel.Trace, "Requesting presentation change");
        var args = new ChangePresentationEventArgs(hint, cancellationToken);
        OnWillChangePresentation(this, args);

        if (args.Cancel)
            return false;

        var result = await ViewDispatcher.ChangePresentation(hint).ConfigureAwait(false);

        args.Result = result;
        OnDidChangePresentation(this, args);

        return result;
    }

    public virtual async Task<bool> Close(IMvxViewModel viewModel, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(viewModel);

        var args = new MvxNavigateEventArgs(viewModel, NavigationMode.Close, cancellationToken);
        OnWillClose(this, args);

        if (args.Cancel)
            return false;

        var close = await ViewDispatcher.ChangePresentation(new MvxClosePresentationHint(viewModel)).ConfigureAwait(false);
        OnDidClose(this, args);

        return close;
    }

    protected virtual void OnWillNavigate(object sender, IMvxNavigateEventArgs e)
    {
        WillNavigate?.Invoke(sender, e);
    }

    protected virtual void OnDidNavigate(object sender, IMvxNavigateEventArgs e)
    {
        DidNavigate?.Invoke(sender, e);
    }

    protected virtual void OnWillClose(object sender, IMvxNavigateEventArgs e)
    {
        WillClose?.Invoke(sender, e);
    }

    protected virtual void OnDidClose(object sender, IMvxNavigateEventArgs e)
    {
        DidClose?.Invoke(sender, e);
    }

    protected virtual void OnWillChangePresentation(object sender, ChangePresentationEventArgs e)
    {
        WillChangePresentation?.Invoke(sender, e);
    }

    protected virtual void OnDidChangePresentation(object sender, ChangePresentationEventArgs e)
    {
        DidChangePresentation?.Invoke(sender, e);
    }

    /// <summary>
    ///     Loads a view model targeting the window for the given source.
    /// </summary>
    /// <typeparam name="TViewModel">The viewmodel type.</typeparam>
    /// <typeparam name="TParameter">The parameter type.</typeparam>
    /// <param name="param">The parameter value.</param>
    /// <param name="source">
    ///     This is used to find the window to execute the navigate in.
    ///     This is usually the viewmodel instance which calls this method. 
    /// </param>
    /// <param name="presentationBundle">The presentation bungle.</param>
    /// <param name="cancellationToken">Any cancellation token.</param>
    /// <returns>True if navigation was successful.</returns>
    public virtual Task<bool> Navigate<TViewModel, TParameter>(TParameter param, IMvxViewModel source, IMvxBundle? presentationBundle = null,
        CancellationToken cancellationToken = default) where TViewModel : IMvxViewModel<TParameter>
        where TParameter : notnull
    {
        return Navigate(typeof(TViewModel), param, source, presentationBundle, cancellationToken);
    }

    /// <summary>
    ///     Loads a view model targeting the window for the given source.
    /// </summary>
    /// <typeparam name="TParameter">The parameter</typeparam>
    /// <param name="viewModelType">The viewmodel type.</param>
    /// <param name="param">The parameter value.</param>
    /// <param name="source">
    ///     This is used to find the window to execute the navigate in.
    ///     This is usually the viewmodel instance which calls this method. 
    /// </param>
    /// <param name="presentationBundle">The presentation bungle.</param>
    /// <param name="cancellationToken">Any cancellation token.</param>
    /// <returns>True if navigation was successful.</returns>
    public virtual Task<bool> Navigate<TParameter>(Type viewModelType, TParameter param, IMvxViewModel source, IMvxBundle? presentationBundle = null,
        CancellationToken cancellationToken = default)
        where TParameter : notnull
    {
        var mvxViewModelInstanceRequest = new MvxViewModelInstanceRequestWithSource(viewModelType, source)
        {
            PresentationValues = presentationBundle?.SafeGetData()
        };
        mvxViewModelInstanceRequest.ViewModelInstance = ViewModelLoader.LoadViewModel(mvxViewModelInstanceRequest, param, null);
        return NavigateAsync(mvxViewModelInstanceRequest, mvxViewModelInstanceRequest.ViewModelInstance, presentationBundle, cancellationToken);
    }

    /// <summary>
    ///     Navigates to a view for the given type.
    /// </summary>
    /// <param name="viewModelType">The type of the viewmodel to navigate to.</param>
    /// <param name="source">
    ///     This is used to find the window to execute the navigate in.
    ///     This is usually the viewmodel instance which calls this method. 
    /// </param>
    /// <param name="presentationBundle">A presentation bundle.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    public virtual Task<bool> Navigate(Type viewModelType, IMvxViewModel source, IMvxBundle? presentationBundle = null,
        CancellationToken cancellationToken = default)
    {
        var request = new MvxViewModelInstanceRequestWithSource(viewModelType, source)
        {
            PresentationValues = presentationBundle?.SafeGetData()
        };
        request.ViewModelInstance = ViewModelLoader.LoadViewModel(request, null);
        return NavigateAsync(request, request.ViewModelInstance, presentationBundle, cancellationToken);
    }

    /// <summary>
    ///     Navigates to the viewmodel for the given type.
    /// </summary>
    /// <typeparam name="TViewModel">The type of the viewmodel to navigate to.</typeparam>
    /// <param name="source">
    ///     This is used to find the window to execute the navigate in.
    ///     This is usually the viewmodel instance which calls this method. 
    /// </param>
    /// <param name="presentationBundle">The presentation bundle.</param>
    /// <param name="cancellationToken">Any cancellation token.</param>
    /// <returns>True if successful, false otherwise.</returns>
    public virtual Task<bool> Navigate<TViewModel>(IMvxViewModel source,
        IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default)
        where TViewModel : IMvxViewModel
    {
        return Navigate(typeof(TViewModel), source, presentationBundle, cancellationToken);
    }

    /// <summary>
    ///     Navigates to a view for the given viewmodel.
    /// </summary>
    /// <param name="viewModel">The viewmodel to navigate to.</param>
    /// <param name="source">
    ///     This is used to find the window to execute the navigate in.
    ///     This is usually the viewmodel instance which calls this method. 
    /// </param>
    /// <param name="presentationBundle">The presentation bundle.</param>
    /// <param name="cancellationToken">Any cancellation token.</param>
    /// <returns>True if successful, false otherwise.</returns>
    public virtual Task<bool> Navigate(
        IMvxViewModel viewModel, IMvxViewModel source, IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default)
    {
        var request = new MvxViewModelInstanceRequestWithSource(viewModel, source) { PresentationValues = presentationBundle?.SafeGetData() };
        ViewModelLoader.ReloadViewModel(viewModel, request, null);
        return NavigateAsync(request, viewModel, presentationBundle, cancellationToken);
    }

    /// <summary>
    ///     Navigates to a view for the given viewmodel.
    /// </summary>
    /// <typeparam name="TParameter">The parameter type.</typeparam>
    /// <param name="viewModel">The viewmodel to navigate to.</param>
    /// <param name="param">Any parameters.</param>
    /// <param name="source">
    ///     This is used to find the window to execute the navigate in.
    ///     This is usually the viewmodel instance which calls this method. 
    /// </param>
    /// <param name="presentationBundle">The presentation bundle.</param>
    /// <param name="cancellationToken">Any cancellation token.</param>
    /// <returns>True if successful, false otherwise.</returns>
    public virtual Task<bool> Navigate<TParameter>(IMvxViewModel<TParameter> viewModel, TParameter param, IMvxViewModel source,
        IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default)
        where TParameter : notnull
    {
        var request = new MvxViewModelInstanceRequestWithSource(viewModel, source) { PresentationValues = presentationBundle?.SafeGetData() };
        ViewModelLoader.ReloadViewModel(viewModel, param, request, null);
        return NavigateAsync(request, viewModel, presentationBundle, cancellationToken);
    }

    /// <summary>
    ///     Shows the ViewModel for the given request.
    /// </summary>
    /// <param name="request">The request to show the viewmodel for.</param>
    /// <param name="viewModel">The viewmodel for the navigation arguments.</param>
    /// <param name="presentationBundle">The presentation bundle.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>True is successful. False otherwise.</returns>
    protected virtual async Task<bool> NavigateAsync(MvxViewModelInstanceRequestWithSource request, IMvxViewModel viewModel,
        IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(viewModel);

        var args = new MvxNavigateEventArgs(viewModel, NavigationMode.Show, cancellationToken);
        OnWillNavigate(this, args);

        if (args.Cancel)
        {
            return false;
        }

        bool hasNavigated = await ViewDispatcher.ShowViewModel(request).ConfigureAwait(false);
        if (!hasNavigated)
        {
            return false;
        }

        if (viewModel.InitializeTask?.Task != null)
        {
            await viewModel.InitializeTask.Task.ConfigureAwait(false);
        }

        OnDidNavigate(this, args);
        return true;
    }
}
