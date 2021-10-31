// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MvvmCross.Core;
using MvvmCross.Exceptions;
using MvvmCross.IoC;
using MvvmCross.Logging;
using MvvmCross.Navigation.EventArguments;
using MvvmCross.Presenters.Hints;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace MvvmCross.Navigation
{
#nullable enable
    /// <inheritdoc cref="IMvxNavigationService"/>
    public class MvxNavigationService : IMvxNavigationService
    {
        private readonly IMvxIoCProvider _iocProvider;

        private readonly Lazy<ILogger?> _log = new Lazy<ILogger?>(() =>
            MvxLogHost.GetLog<MvxNavigationService>());

        public IMvxViewDispatcher ViewDispatcher { get; }

        protected Lazy<IMvxViewsContainer> ViewsContainer { get; }

        protected Dictionary<Regex, Type> Routes { get; } = new Dictionary<Regex, Type>();

        protected IMvxViewModelLoader ViewModelLoader { get; set; }

        protected ConditionalWeakTable<IMvxViewModel, TaskCompletionSource<object?>> TaskCompletionResults { get; } =
            new ConditionalWeakTable<IMvxViewModel, TaskCompletionSource<object?>>();

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
            ViewsContainer = new Lazy<IMvxViewsContainer>(() => _iocProvider.Resolve<IMvxViewsContainer>());
        }

        public void LoadRoutes(IEnumerable<Assembly> assemblies)
        {
            if (assemblies == null)
                throw new ArgumentNullException(nameof(assemblies));

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
            ValidateArguments(path);

            try
            {
                var matches = Routes.Where(t => t.Key.IsMatch(path)).ToList();

                switch (matches.Count)
                {
                    case 0:
                        entry = default;
                        _log.Value?.Log(LogLevel.Trace, "Unable to find routing for {path}", path);
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

                _log.Value?.Log(LogLevel.Warning, "The following regular expressions match the provided url ({count}), each RegEx must be unique (otherwise try using IMvxRoutingFacade): {matches}",
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
            if (regex == null)
                throw new ArgumentNullException(nameof(regex));

            if (match == null)
                throw new ArgumentNullException(nameof(match));

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
            ValidateArguments(path);

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
            ValidateArguments(path, param);

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
            return Task.FromResult(ViewsContainer.Value.GetViewType(typeof(TViewModel)) != null);
        }

        public virtual Task<bool> CanNavigate(Type viewModelType)
        {
            return Task.FromResult(ViewsContainer.Value.GetViewType(viewModelType) != null);
        }

        protected virtual async Task<bool> Navigate(MvxViewModelRequest request, IMvxViewModel viewModel,
            IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default)
        {
            ValidateArguments(request, viewModel);

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

        public virtual async Task<bool> Navigate<TParameter>(string path, TParameter param,
            IMvxBundle? presentationBundle = null, CancellationToken cancellationToken = default)
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
            ValidateArguments(hint);

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
            ValidateArguments(viewModel);

            var args = new MvxNavigateEventArgs(viewModel, NavigationMode.Close, cancellationToken);
            OnWillClose(this, args);

            if (args.Cancel)
                return false;

            var close = await ViewDispatcher.ChangePresentation(new MvxClosePresentationHint(viewModel)).ConfigureAwait(false);
            OnDidClose(this, args);

            return close;
        }

        public virtual async Task<bool> Close<TResult>(
            IMvxViewModelResult<TResult> viewModel, TResult result, CancellationToken cancellationToken = default)
        {
            ValidateArguments(viewModel);

            TaskCompletionResults.TryGetValue(viewModel, out var tcs);

            //Disable cancelation of the Task when closing ViewModel through the service
            viewModel.CloseCompletionSource = null;

            try
            {
                var closeResult = await Close(viewModel, cancellationToken).ConfigureAwait(false);
                if (closeResult)
                {
                    tcs?.TrySetResult(result);
                    TaskCompletionResults.Remove(viewModel);
                }
                else
                    viewModel.CloseCompletionSource = tcs;
                return closeResult;
            }
            catch (Exception ex)
            {
                tcs?.TrySetException(ex);
                return false;
            }
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

        private static void ValidateArguments(MvxViewModelRequest request, IMvxViewModel viewModel)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));
        }

        private static void ValidateArguments<TParameter>(string path, TParameter param)
        {
            ValidateArguments(path);

            if (param == null)
                throw new ArgumentNullException(nameof(param));
        }

        private static void ValidateArguments(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));
        }

        private static void ValidateArguments(MvxPresentationHint hint)
        {
            if (hint == null)
                throw new ArgumentNullException(nameof(hint));
        }

        private static void ValidateArguments(IMvxViewModel viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));
        }

        private static void ValidateArguments<TResult>(IMvxViewModelResult<TResult> viewModel)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));
        }
    }
#nullable restore
}
